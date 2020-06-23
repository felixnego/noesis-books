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
        Task<IEnumerable<CategoryDTO>> GetTopCategories();
        Task<IEnumerable<BookListDTO>> TopInCategory(long categoryId);
        Task<IEnumerable<BookListDTO>> SearchBooks(string searchTerms);
        Boolean SearchAuthorNames(List<BookAuthor> authors, string term);
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

        public Boolean SearchAuthorNames(List<BookAuthor> authors, string term) {
            bool result = false;

            authors.ForEach(authorObj => {
                if (authorObj.Author.Name.Contains(term)) result = true;
            });

            return result;
        }


    }
}
