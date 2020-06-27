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
    public interface INoteService
    {
        Task<Note> AddNote(long userId, long bookId, Note note);
        Task<Note> UpdateNote(long noteid, Note newNote);
        Task<Note> DeleteNote(long noteid, UserNote userNote);
    }

    public class NoteService : INoteService
    {
        private readonly NoesisApiContext _context;

        public NoteService(NoesisApiContext context)
        {
            _context = context;
        }

        public async Task<Note> AddNote(long userId, long bookId, Note note)
        {
            _context.Note.Add(note);
            await _context.SaveChangesAsync();
            long noteId = note.Id;

            var userNote = new UserNote { UserId = userId, NoteId = noteId, BookId = bookId };
            _context.UserNotes.Add(userNote);
            await _context.SaveChangesAsync();

            var updatedBook = await _context.Book.FindAsync(bookId);
            updatedBook.UserNotes.Add(userNote);
            await _context.SaveChangesAsync();

            var updatedUser = await _context.User.FindAsync(userId);
            updatedUser.UserNotes.Add(userNote);
            await _context.SaveChangesAsync();

            return note;
        }

        public async Task<Note> UpdateNote(long noteid, Note newNote)
        {
            var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == noteid);

            note.Text = newNote.Text;
            await _context.SaveChangesAsync();

            return note;

        }

        public async Task<Note> DeleteNote(long noteid, UserNote userNote)
        {
            var note = await _context.Note.FirstOrDefaultAsync(n => n.Id == noteid);

            _context.UserNotes.Remove(userNote);
            _context.Note.Remove(note);
            await _context.SaveChangesAsync();

            return note;
        }
    }
}
