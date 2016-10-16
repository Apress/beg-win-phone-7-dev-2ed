using System;
using System.Collections.Generic;
using System.Linq;

namespace NotepadServiceRole
{
    public class Service1 : IService
    {
        public Guid AddUser(Guid userId, string userName)
        {
            using (var context = new NotepadDBEntities())
            {
                context.AddToUsers(new User()
                    {
                        UserId = userId,
                        Name = userName,
                    });
                context.SaveChanges();

                return userId;
            }
        }

        public NoteDto AddNote(Guid userId, string notedescription, string noteText)
        {
            using (var context = new NotepadDBEntities())
            {
                Note note = new Note()
                    {
                        Description = notedescription,
                        UserId = userId,
                        NoteText = noteText,
                    };
                context.AddToNotes(note);
                context.SaveChanges();

                return new NoteDto() 
                    {
                        NoteId = note.NoteId,
                        Description = note.Description,
                        NoteText = note.NoteText,
                    };
            }
        }

        public void UpdateNote(int noteId, string noteText)
        {
            using (var context = new NotepadDBEntities())
            {
                var note = context
                                .Notes
                                .Where(n => n.NoteId.Equals(noteId)
                                      ).Single();
                note.NoteText = noteText;
                context.SaveChanges();
            }
        }

        public void DeleteNote(Guid userId, int noteId)
        {
            using (var context = new NotepadDBEntities())
            {
                var note = context
                                .Notes
                                .Where(n => n.NoteId.Equals(noteId)).Single();
                context.Notes.DeleteObject(note);
                context.SaveChanges();
            }
        }

        public List<NoteDto> GetNotes(Guid userId)
        {
            using (var context = new NotepadDBEntities())
            {
                var notes = (
                                from eachNote in context.Notes
                                where eachNote.UserId == userId
                                orderby eachNote.Description ascending
                                select new NoteDto
                                {
                                    NoteId = eachNote.NoteId,
                                    Description = eachNote.Description,
                                    NoteText = eachNote.NoteText,
                                }
                            ).ToList();

                return notes;
            }
        }

        public NoteDto GetNote(Guid userId, int noteId)
        {
            using (var context = new NotepadDBEntities())
            {
                var notes = (
                                from eachNote in context.Notes
                                where eachNote.NoteId == noteId  
                                        && eachNote.UserId == userId
                                select new NoteDto
                                {
                                    NoteId = eachNote.NoteId,
                                    Description = eachNote.Description,
                                    NoteText = eachNote.NoteText,
                                }
                            ).SingleOrDefault();

                return notes;
            }
        }
    }
}
