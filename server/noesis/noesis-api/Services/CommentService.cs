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
    public interface ICommentService
    {
        Task<Comment> AddComment(long userId, long bookId, Comment comment);
        Task<Comment> UpdateComment(long commentId, Comment newComment);
        Task<Comment> DeleteComment(long commentId, UserComment userComment);
    }

    public class CommentService : ICommentService
    {
        private readonly NoesisApiContext _context;

        public CommentService(NoesisApiContext context)
        {
            _context = context;
        }

        public async Task<Comment> AddComment(long userId, long bookId, Comment comment)
        {
            comment.AddedOn = DateTime.Now;

            _context.Comment.Add(comment);
            await _context.SaveChangesAsync();
            long commentId = comment.Id;

            var userComment = new UserComment { UserId = userId, CommentId = commentId, BookId = bookId };
            _context.UserComments.Add(userComment);
            await _context.SaveChangesAsync();

            var updatedBook = await _context.Book.FindAsync(bookId);
            updatedBook.UserComments.Add(userComment);
            await _context.SaveChangesAsync();

            var updatedUser = await _context.User.FindAsync(userId);
            updatedUser.UserComments.Add(userComment);
            await _context.SaveChangesAsync();

            return comment;

        }

        public async Task<Comment> UpdateComment(long commentId, Comment newComment)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == commentId);

            comment.Text = newComment.Text;
            await _context.SaveChangesAsync();

            return comment;
        }

        public async Task<Comment> DeleteComment(long commentId, UserComment userComment)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(c => c.Id == commentId);

            _context.UserComments.Remove(userComment);
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();

            return comment;
        }
    }
}
