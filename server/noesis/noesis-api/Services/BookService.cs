using System;
using noesis_api.Models;
using noesis_api.DTOs;
using noesis_api.Contexts;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace noesis_api.Services
{
    public interface IBookService
    {
        Task<IEnumerable<BookListDTO>> GetPageOfBooks(int page);
        Task<BookDetailDTO> GetBookDetails(long id);
        Task<IEnumerable<CategoryDTO>> GetTopCategories();
        Task<IEnumerable<BookListDTO>> TopInCategory(long categoryId);
        Task<IEnumerable<BookListDTO>> SearchBooks(string searchTerms);
        Task<Book> AddBook(BookDetailDTO book);
        Task<Book> UpdateBook(BookDetailDTO book);
        Task<Book> DeleteBook(long id);
        Task<IEnumerable<CategoryDTO>> GetAllCategories();
        Task<IEnumerable<AuthorListDTO>> GetAllAuthors();
        Task<UserRating> AddRating(long bookId, UserRating userRating);
    }

    public class BookService : IBookService
    {
        private readonly NoesisApiContext _context;
        private readonly IMapper _mapper;
        private readonly int booksPerPage = 10;

        public BookService(NoesisApiContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookListDTO>> GetPageOfBooks(int page)
        {

            var books = await _context.Book
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.BookAuthors).ThenInclude(b => b.Author)
                .Include(ba => ba.UserRatings)
                .Skip(page * booksPerPage)
                .Take(booksPerPage)
                .ToListAsync();

            return _mapper.Map<IEnumerable<BookListDTO>>(books);

        }

        public async Task<BookDetailDTO> GetBookDetails(long id)
        {
            var book = await _context.Book
                .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                .Include(b => b.BookAuthors).ThenInclude(b => b.Author)
                .Include(b => b.UserComments).ThenInclude(b => b.Comment)
                .Include(b => b.UserComments).ThenInclude(b => b.User)
                .Include(b => b.UserNotes).ThenInclude(b => b.Note)
                .Include(b => b.UserNotes).ThenInclude(b => b.User)
                .Include(ba => ba.UserRatings)
                .FirstOrDefaultAsync(b => b.Id == id);

            return _mapper.Map<BookDetailDTO>(book);
        }

        public async Task<IEnumerable<CategoryDTO>> GetTopCategories()
        {
            string query = @"select c.id, c.categorydescription 
                    from bookcategory bc 
                    join category c on bc.categoryid = c.id 
                    group by bc.categoryid 
                    order by count(bc.bookid) desc 
                    limit 3; ";

            var categories = await _context.Category.FromSqlRaw(query).ToListAsync();

            return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
        }

        public async Task<IEnumerable<BookListDTO>> TopInCategory(long categoryId)
        {
            string query = @"select b.*
                    from Book b
                    join bookcategory bc on bc.bookid = b.id
                    join userratings us on us.bookid = b.id
                    where bc.categoryid = " + categoryId +
                    @" group by b.id, b.title
                    order by AVG(us.ratingvalue) DESC
                    limit 6;";
            
            var books = await _context.Book
              .FromSqlRaw(query)
              .ToListAsync();

            books.ForEach(book =>
            {
                _context.Entry(book)
                    .Collection(b => b.UserRatings)
                    .Load();
            });

            return _mapper.Map<IEnumerable<BookListDTO>>(books);

        }

        public async Task<IEnumerable<BookListDTO>> SearchBooks(string searchTerms)
        {
            if (searchTerms != null)
            {
                var bookSet = _context.Book as IQueryable<Book>;

                var booksFromDb = await bookSet
                    .Include(b => b.BookCategories).ThenInclude(bc => bc.Category)
                    .Include(b => b.BookAuthors).ThenInclude(b => b.Author)
                    .Include(ba => ba.UserRatings)
                    .Where(b => b.Title.ToLower().Contains(searchTerms.ToLower()) ||
                        b.BookAuthors.Any(ba => ba.Author.Name.ToLower().Contains(searchTerms.ToLower())))
                    .ToListAsync();

                return _mapper.Map<IEnumerable<BookListDTO>>(booksFromDb);
            }

            return null;
        }

        public async Task<Book> AddBook(BookDetailDTO book)
        {
            var bookForCreation = _mapper.Map<Book>(book);

            _context.Book.Add(bookForCreation);
            await _context.SaveChangesAsync();

            AddAuthors(book, bookForCreation.Id);

            AddCategories(book, bookForCreation.Id);

            return bookForCreation;
        }

        public async Task<Book> UpdateBook(BookDetailDTO book)
        {
            var bookEntity = _mapper.Map<Book>(book);


            _context.Entry(bookEntity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            _context.BookAuthors.RemoveRange(_context.BookAuthors.Where(x => x.BookId == bookEntity.Id));
            _context.BookCategory.RemoveRange(_context.BookCategory.Where(x => x.BookId == bookEntity.Id));
            await _context.SaveChangesAsync();

            AddAuthors(book, bookEntity.Id);
            AddCategories(book, bookEntity.Id);

            return bookEntity;
        }

        public void AddAuthors(BookDetailDTO book, long bookCreatedId)
        {
            book.BookAuthors.ForEach(a => {
                long authorId = 0;

                if (a.Id > 0)
                {
                    authorId = a.Id;
                }
                else
                {
                    var newAuthor = new Author { Name = a.Name };
                    _context.Author.Add(newAuthor);
                    _context.SaveChanges();
                    authorId = newAuthor.Id;
                }
                var bookAuthor = new BookAuthor { BookId = bookCreatedId, AuthorId = authorId };
                _context.BookAuthors.Add(bookAuthor);
                _context.SaveChanges();
            });
        }

        public void AddCategories(BookDetailDTO book, long bookCreatedId)
        {
            book.BookCategories.ForEach(c =>
            {
                long catId = 0;

                if (c.Id > 0)
                {
                    catId = c.Id;
                }
                else
                {
                    var newCategory = new Category { CategoryDescription = c.CategoryDescription };
                    _context.Category.Add(newCategory);
                    _context.SaveChanges();
                    catId = newCategory.Id;
                }
                var bookCategory = new BookCategory { BookId = bookCreatedId, CategoryId = catId };
                _context.BookCategory.Add(bookCategory);
                _context.SaveChanges();
            });
        }

        public async Task<IEnumerable<CategoryDTO>> GetAllCategories()
        {
            var initialSet = await _context.Category.ToListAsync();

            return _mapper.Map<IEnumerable<CategoryDTO>>(initialSet);
        }

        public async Task<IEnumerable<AuthorListDTO>> GetAllAuthors()
        {
            var initialSet = await _context.Author.ToListAsync();

            return _mapper.Map<IEnumerable<AuthorListDTO>>(initialSet);
        }

        public async Task<Book> DeleteBook(long id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return null;
            }

            _context.Book.Remove(book);
            await _context.SaveChangesAsync();

            return book;
        }

        public async Task<UserRating> AddRating(long bookId, UserRating userRating)
        {
            var existingRating = await _context.UserRatings.Where(
                ur => ur.UserId == userRating.UserId && ur.BookId == userRating.BookId
                ).SingleOrDefaultAsync();

            if (existingRating != null)
            {
                existingRating.RatingValue = userRating.RatingValue;
                await _context.SaveChangesAsync();
            }
            else
            {
                _context.UserRatings.Add(userRating);

                var book = await _context.Book.FindAsync(bookId);
                book.UserRatings.Add(userRating);

                var user = await _context.User.FindAsync(userRating.UserId);
                user.UserRatings.Add(userRating);

                await _context.SaveChangesAsync();
            }

            return userRating;
        }
    }
}
