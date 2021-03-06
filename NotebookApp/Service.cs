﻿using System;
using System.Linq;

namespace NotebookApp
{
    public class Service 
    {
        internal static void Menu() //меню программы
        {
            Console.WriteLine("Добро пожаловать в электронную записную телефонную книжку Notebook.Pro 1.0!");
            Console.WriteLine("\nВыберите дальнейшее действие:\n");
            Console.WriteLine("1 - Создать новую запись;");
            Console.WriteLine("2 - Просмотреть ранее созданную запись;");
            Console.WriteLine("3 - Редактировать ранее созданную запись;");
            Console.WriteLine("4 - Просмотреть все записи;");
            Console.WriteLine("5 - Удалить запись;");
            Console.WriteLine("\n0 - Выйти из системы.");
        }

        internal static void EditNoteMenu(Note note) //меню редактирования ранее созданных записей
        {
            Console.Clear();
            Console.WriteLine("***Редактирование ранее созданных записей***");
            Console.WriteLine(note);
            Console.WriteLine("\nВыберите поле, которое желаете отредактировать, или 0, чтобы вернуться в главное меню:\n");
            Console.WriteLine("1 - Фамилия;");
            Console.WriteLine("2 - Имя;");
            Console.WriteLine("3 - Отчество;");
            Console.WriteLine("4 - Номер телефона;");
            Console.WriteLine("5 - Страна;");
            Console.WriteLine("6 - Дата рождения;");
            Console.WriteLine("7 - Организация;");
            Console.WriteLine("8 - Должность;");
            Console.WriteLine("9 - Прочие заметки.");
        }

        internal static void Choice(Notebook noteBook) //выбор пункта основного меню
        {
            byte choice;
            Console.Write("\nЯ хочу ");

            try
            {
                choice = byte.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Clear();
                        Console.WriteLine("***Создание новой записи***\n");
                        noteBook.CreateNewNote();
                        break;

                    case 2:
                        Console.Clear();
                        noteBook.ReadNote();
                        break;

                    case 3:
                        Console.Clear();
                        Console.WriteLine("***Редактирование ранее созданных записей***\n");

                        if (noteBook.listOfNotes.Count == 0)
                        {
                            Console.WriteLine(
                                "Записи не найдены!\nНажмите любую клавишу, чтобы вернуться в главное меню.");
                            ToMainMenu(noteBook);
                            break;
                        }

                        Console.WriteLine("Список записей: ");
                        for (var i = 0; i < noteBook.listOfNotes.Count; i++)
                        {
                            var note = noteBook.listOfNotes[i];
                            Console.WriteLine(" {0}. {1} {2}, {3} ({4} - идентификационный номер записи);", i + 1,
                                note.surname, note.name, note.phoneNumber, note.notesNumber);
                        }

                        Console.Write(
                            "\nВведите идентификационный номер записи, которую вы желаете изменить, или 0, чтобы вернуться в главное меню: ");
                        var desiredNote = noteBook.GetNote();
                        if (desiredNote == null)
                        {
                            Console.WriteLine(
                                "\nЗапись с данным номером не найдена.\nНажмите любую клавишу, чтобы вернуться в главное меню.");
                            ToMainMenu(noteBook);
                        }
                        else
                        {
                            noteBook.EditNote(desiredNote);
                        }

                        break;

                    case 4:
                        Console.Clear();
                        Console.WriteLine("***Просмотр всех записей***\n");
                        noteBook.ShowAllNotes();
                        if (noteBook.listOfNotes.Count != 0)
                        {
                            Console.WriteLine("\nНажмите любую клавишу, чтобы вернуться в главное меню.");
                            ToMainMenu(noteBook);
                        }

                        break;

                    case 5:
                        Console.Clear();
                        noteBook.DeleteNote();
                        break;

                    case 0:
                        Console.Clear();
                        Console.WriteLine("До новых встреч!\nНажмите любую клавишу для выхода.");
                        Console.ReadKey();
                        Environment.Exit(0);
                        break;

                    default:
                        Console.Write("\nВведённый Вами символ некорректен! \nПопробуйте ещё раз.\n");
                        Choice(noteBook);
                        break;
                }
            }

            catch (Exception e)
            {
                Console.Write("\nВведённый Вами символ некорректен! \nПопробуйте ещё раз.\n");
                Choice(noteBook);
            }
        }

        internal static Note
            EditNoteChoice(ref Note note, Notebook noteBook) //выбор пункта меню редактирования ранее созданных записей
        {
            byte choice;
            Console.Write("\nЯ хочу изменить поле ");

            try
            {
                choice = byte.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.Write("\nИзмените фамилию: ");
                        var surname = Console.ReadLine();
                        surname = CheckNameString(surname, new[] {'-'});
                        note.surname = surname;
                        break;

                    case 2:
                        Console.Write("\nИзмените имя: ");
                        var name = Console.ReadLine();
                        name = CheckNameString(name, new[] {'-'});
                        note.name = name;
                        break;

                    case 3:
                        Console.Write("\nИзмените отчество: ");
                        var patronymic = Console.ReadLine();
                        patronymic = CheckNameString(patronymic, new char[] { });
                        note.patronymic = patronymic;
                        break;

                    case 4:
                        Console.Write("\nИзмените номер телефона: ");
                        var phoneNumber = Console.ReadLine();
                        long phoneNumberCheck;
                        while (long.TryParse(phoneNumber, out phoneNumberCheck) != true)
                        {
                            Console.Write("\nВведённый Вами символ некорректен!\nПопробуйте ещё раз: ");
                            phoneNumber = Console.ReadLine();
                        }

                        note.phoneNumber = phoneNumber;
                        break;

                    case 5:
                        Console.Write("\nИзмените название страны: ");
                        var country = Console.ReadLine();
                        country = CheckNameString(country, new[] {'-', ' '});
                        note.country = country;
                        break;

                    case 6:
                        Console.Write("\nИзмените дату рождения:  (ДД.MM.ГГГГ): ");
                        var birthDateString = Console.ReadLine();
                        while (true)
                            if (birthDateString.Any(c => !char.IsDigit(c) && c != '.') || birthDateString[2] != '.' ||
                                birthDateString[5] != '.' || birthDateString.Length != 10)
                            {
                                Console.Write("Введённые Вами данные недопустимы!\nПопробуйте ещё раз: ");
                                birthDateString = Console.ReadLine();
                            }
                            else
                            {
                                break;
                            }

                        var birthDate = DateTime.ParseExact(birthDateString, "dd.MM.yyyy", null);
                        note.birthDate = birthDate;
                        break;

                    case 7:
                        Console.Write("\nИзмените название организации: ");
                        var organization = Console.ReadLine();
                        ;
                        note.organization = organization;
                        break;

                    case 8:
                        Console.Write("\nИзмените название должности: ");
                        var position = Console.ReadLine();
                        note.position = position;
                        break;

                    case 9:
                        Console.Write("\nИзмените текст заметки: ");
                        var notes = Console.ReadLine();
                        note.notes = notes;
                        break;

                    case 0:
                        Console.Clear();
                        Menu();
                        Choice(noteBook);
                        Console.ReadKey();
                        break;

                    default:
                        Console.Write("\nВведённый Вами символ некорректен! \nПопробуйте ещё раз.\n");
                        Choice(noteBook);
                        break;
                }
            }

            catch (FormatException e)
            {
                Console.Write("\nВведённый Вами символ некорректен! \nПопробуйте ещё раз.\n");
                Choice(noteBook);
            }

            return note;
        }

        public static bool isIncorrectNameString(string s, char[] symbols) //проверка на правильность ввода строки 
        {
            //метод вынесен отдельно для того, чтобы было удобнее его протестировать
            if (s.Length == 0)
                return true;
            if (!char.IsLetter(s[0]))
                return true;
            if (symbols.Any(c => s.Contains(c + "" + c)))
                return true;
            var isIncorrect = s.Any(c => !char.IsLetter(c) && !symbols.Contains(c));
            return isIncorrect;
        }

        internal static string CheckNameString(string s, char[] symbols) //проверка на правильность ввода строки 
        {
            while (true)
                if (isIncorrectNameString(s, symbols))
                {
                    Console.Write("\nВведённая Вами строка содержит недопустимые символы!\nПопробуйте ещё раз: ");
                    s = Console.ReadLine();
                }
                else
                {
                    break;
                }

            return s;
        }

        internal static void ToMainMenu(Notebook noteBook) //вспомогательный метод, выводящий меню в консоль
        {
            Console.ReadKey();
            Console.Clear();
            Menu();
            Choice(noteBook);
            Console.ReadKey();
            Console.WriteLine("edit");
        }
    }
}