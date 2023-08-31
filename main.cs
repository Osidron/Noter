using Npgsql.Replication;
using System.Reflection;

namespace Noter
{
    internal class main
    {
        static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                const int minMode = 0;
                const int maxMode = 5;
                while (true) {
                    Console.WriteLine("Введите режим работы");
                    Console.WriteLine("0. Покинуть программу");
                    Console.WriteLine("1. Внести заметку");
                    Console.WriteLine("2. Редактировать заметку");
                    Console.WriteLine("3. Просмотреть список заметок");
                    Console.WriteLine("4. Удалить заметку");
                    Console.WriteLine("5. Удалить все заметки");
                    if (!int.TryParse(Console.ReadLine(), out int mode) || mode > maxMode || mode < minMode)
                    {
                        Console.WriteLine("Введен некорректный режим");
                        Environment.Exit(0);
                    }
                    if(mode == 0)
                    {
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
                            notesAction.putNewNoteInDB(new note { text = newText, title = newTitle }, db);
                            Console.WriteLine("Заметка успешно сохранена");
                            List<note_data> _notes = notesAction.returnCurrentListOfNotes(db);
                            for (int i = 0; i < _notes.Count; i++)
                            {
                                Console.WriteLine(_notes[i].ID + " " + _notes[i].title);
                            }
                            break;

                        case 2:
                            Console.WriteLine("Введите номер заметки, которую вы хотите перезаписать");
                            if (int.TryParse(Console.ReadLine(), out int ID))
                            {
                                if (ID >= notesAction.maxID(db) && ID < 0)
                                {
                                    Console.WriteLine("Заметки по введенному номеру не существует");
                                    break;
                                }
                                Console.WriteLine("Введите новое название и текст для заметки через Enter");
                                if (notesAction.overWriteExactNoteByID(new note { title = Console.ReadLine(), text = Console.ReadLine() }, ID, db))
                                {
                                    Console.WriteLine("Заметка была успешно перезаписана");
                                }
                            }
                            else Console.WriteLine("Введен некорректный номер");
                            break;

                        case 3:
                            if (notesAction.maxID(db) != 0)
                            {
                                List<note_data> notes = notesAction.returnCurrentListOfNotes(db);
                                for (int i = 0; i < notes.Count; i++)
                                {
                                    Console.WriteLine(notes[i].ID + " " + notes[i].title);
                                }
                                Console.WriteLine("Чтобы просмотреть конкретную заметку - введите ее номер. Для выхода, введите -1");
                                if (int.TryParse(Console.ReadLine(), out ID))
                                {
                                    if (db.noteList.FirstOrDefault(x => x.ID == ID) != null)
                                    {
                                        Console.WriteLine(notesAction.getNoteByID(ID, db).title + "\n" + notesAction.getNoteByID(ID, db).text);
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
                                notesAction.removeNoteFromDBByID(ID, db);
                                Console.WriteLine("Заметка была успешно удалена");
                                break;
                            }
                            else
                            {
                                Console.WriteLine("Номер был введен неверно, либо заметки с таким номером не существует");
                                break;
                            }
                        case 5:
                            Console.WriteLine("Вы уверены, что хотите удалить все заметки? y - Да, n - Нет");
                            if (Console.ReadLine() == "y")
                            {
                                notesAction.removeAllNotes(db);
                                Console.WriteLine("Удаление прошло успешно");
                            }

                            break;

                    }
                    Console.WriteLine();
                }
            }
        }
    }
}