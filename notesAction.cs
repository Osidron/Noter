using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noter
{
     public class notesAction
    {
        public static note_data getNoteByID(int ID, ApplicationContext db)
        {
            List<note_data> notes = db.noteList.ToList();
            note_data requiredNote = notes.FirstOrDefault(i => i.ID == ID);
            return requiredNote;
        }
        public static Boolean overWriteExactNoteByID(note sampleNote, int ID, ApplicationContext db)
        {
            note_data requiredNote = db.noteList.Find(ID);
            if (requiredNote != null)
            {
                requiredNote = new note_data { title = sampleNote.title, text = sampleNote.text, ID = ID };
                db.SaveChanges();
                return true;
            }
            else return false;
        }
        public static void putNewNoteInDB(note sampleNote, ApplicationContext db)
        {
            note_data dbNote = new note_data { text = sampleNote.text, title = sampleNote.title };
            db.noteList.Add(dbNote);
            db.SaveChanges();
            Console.WriteLine(db.noteList.ToList().FirstOrDefault(x => x.title == sampleNote.title).title);
        }

        public static void removeNoteFromDBByID(int ID, ApplicationContext db)
        {
            note_data? removableNote = (from n in db.noteList where n.ID == ID select n).SingleOrDefault();
            if (removableNote != null)
            {
                db.noteList.Remove(removableNote);
                db.SaveChanges();
            }
        }
        public static List<note_data> returnCurrentListOfNotes(ApplicationContext db)
        {
            return db.noteList.ToList();
        }

        public static Boolean checkIfDBEmpty(ApplicationContext db)
        {
            if (db.noteList.ToList().Count == 0) return true;
            else return false;
        }
        public static int maxID(ApplicationContext db)
        {
            if (!notesAction.checkIfDBEmpty(db)) return db.noteList.ToList().LastOrDefault().ID;
            else return 0;
        }
        public static void removeAllNotes(ApplicationContext db)
        {
            for (int i = 0; i <= maxID(db); i++)
            {
                notesAction.removeNoteFromDBByID(i, db);
            }

        }
    }
}
