using Npgsql.Replication;
using System.Reflection;

namespace Noter
{
    internal class main
    {
        private class notesAction
        {
            public static note_data getNoteByID(int ID)
            {
                List<note_data> notes = db.noteList.ToList();
                note_data requiredNote = notes.FirstOrDefault(i => i.ID == ID);
                return requiredNote;
            }
            public static Boolean overWriteExactNoteByID(note sampleNote, int ID)
            {
                using (ApplicationContext db = new ApplicationContext())
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
            }
            public static void putNewNoteInDB(note sampleNote)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    note_data dbNote = new note_data { text = sampleNote.text, title = sampleNote.title };
                    db.noteList.Add(dbNote);
                    db.SaveChanges();
                    Console.WriteLine(db.noteList.ToList().FirstOrDefault(x => x.title == sampleNote.title).title);
                }
            }

            public static void removeNoteFromDBByID(int ID)
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    note_data removableNote = (from n in db.noteList where n.ID == ID select n).SingleOrDefault();
                    db.noteList.Remove(removableNote);
                    db.SaveChanges();
                }
            }
            public static List<note_data> returnCurrentListOfNotes()
            {
                using (ApplicationContext db = new ApplicationContext())
                { 
                    return db.noteList.ToList();
                }
            }
            
            public static Boolean checkIfDBEmpty()
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    if (db.noteList.ToList().Count == 0) return true;
                    else return false;
                }
            }
            public static int maxID()
            {
                using (ApplicationContext db = new ApplicationContext())
                {
                    return db.noteList.ToList().Count - 1;
                }
            }
        }
        internal class Convert
        {
            public static string noteToString(note_data note)
            {
                return $"{note.title}\n{note.text}";
            }
            public static int stringToInt(string str)
            {
                Boolean isParsed = int.TryParse(str, out int i);
                if (isParsed) 
                {
                    return i;        
                }
                return -1;

            }
        }
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                Console.WriteLine("Введите режим работы");
                Console.WriteLine("1. Внести заметку");
                Console.WriteLine("2. Редактировать заметку");
                Console.WriteLine("3. Просмотреть список заметок");
                Console.WriteLine("4. Удалить заметку");
                if (!int.TryParse(Console.ReadLine(), out int mode) || mode > 4 || mode < 1)
                {
                    Console.WriteLine("Введен некорректный режим");
                    Environment.Exit(0);
                }
                switch (mode)
                {
                    case 1:
                        Console.WriteLine("Введите название заметки");
                        string? newTitle = Console.ReadLine();
                        if (newTitle == null)
                        {
                            newTitle = "<Без названия>";
                        }
                        Console.WriteLine("Введите текст заметки");
                        string newText = Console.ReadLine();
                        notesAction.putNewNoteInDB(new note { text = newText, title = newTitle });
                        Console.WriteLine("Заметка успешно сохранена");
                        break;

                    case 2:
                        Console.WriteLine("Введите номер заметки, которую вы хотите перезаписать");
                        if (int.TryParse(Console.ReadLine(), out int ID))
                        {
                            if (ID > notesAction.maxID() && ID < 0)
                            {
                                Console.WriteLine("Заметки по введенному номеру не существует");
                                break;
                            }
                            Console.WriteLine("Введите новое название и текст для заметки через Enter");
                            if (notesAction.overWriteExactNoteByID(new note { title = Console.ReadLine(), text = Console.ReadLine() }, ID))
                            {
                                Console.WriteLine("Заметка была успешно перезаписана");
                            }
                        }
                        else Console.WriteLine("Введен некорректный номер");
                        break;

                    case 3:
                        if (notesAction.maxID() != -1)
                        {
                            List<note_data> notes = notesAction.returnCurrentListOfNotes();
                            for (int i = 0; i < notes.Count; i++)
                            {
                                Console.WriteLine(notes[i].ID + " " + notes[i].title + "+");
                            }
                            Console.WriteLine("Чтобы просмотреть конкретную заметку - введите ее номер. Для выхода, введите -1");
                            if (int.TryParse(Console.ReadLine(), out ID))
                            {
                                if (ID >= 0)
                                {
                                    Console.WriteLine(notesAction.getNoteByID(ID).title + "\n" + notesAction.getNoteByID(ID).text);
                                }
                                else
                                {
                                    if (ID == -1) Console.WriteLine("Вы покинули программу");
                                    else
                                    {
                                        Console.WriteLine("Номер был введен неверно, либо заметки с таким номером не существует");
                                        break;
                                    }
                                }
                            }
                        }
                        else Console.WriteLine("Невозможно вывести список заметок, так как ни одной заметки не было создано");
                        break;

                    case 4:
                        Console.WriteLine("Введите номер удаляемой заметки");
                        ID = Convert.stringToInt(Console.ReadLine());
                        if (ID >= 0)
                        {
                            notesAction.removeNoteFromDBByID(ID);
                            Console.WriteLine("Заметка была успешно удалена");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Номер был введен неверно, либо заметки с таким номером не существует");
                            break;
                        }

                }
                Console.ReadKey();
            }
        }
    }
}