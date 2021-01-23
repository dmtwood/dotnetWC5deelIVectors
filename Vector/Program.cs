using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VectorLib;

namespace Vector
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new Vector<int>();
            numbers.PushBack(12);
            numbers.PushBack(13);
            numbers.PushBack(11);
            numbers.PushBack(19);
            numbers.PopBack();

            Console.WriteLine(numbers); //Uitvoer: 12, 13, 11

            foreach (int number in numbers)
            {
                Console.WriteLine(number);
            }
            /* Uitvoer:
             * 12
             * 13
             * 11
             */

            Uitbreiding();

            Queries();
        }

        private static void Uitbreiding()
        {
            // initialisatie met andere Enumerable
            var lijst = new List<int> { 0, 1, 2, 3, 4 };
            var set = new HashSet<int> { 5, 6, 7, 8, 9 };

            var vectorFromList = new Vector<int>(lijst);
            var vectorFromHashSet = new Vector<int>(set);

            foreach (int getal in vectorFromList)
            {
                Console.Write($"{getal} ");
            }

            Console.WriteLine();

            /* Uitvoer:
            * 0 1 2 3 4
            */

            foreach (int getal in vectorFromHashSet)
            {
                Console.Write($"{getal} ");
            }

            Console.WriteLine();

            /* Uitvoer:
             * 5 6 7 8 9
             */

            // initialisatie met collection initialiser
            var vector = new Vector<int> { 5, 6, 7, 8 };

            foreach (int getal in vector)
            {
                Console.Write($"{getal} ");
            }
            
            Console.WriteLine();

            /* Uitvoer:
             * 5 6 7 8 
             */
        }

        private static void Queries()
        {
            Vector<Email> emails = InitMails();

            var emailsVandaag = emails
                .Where(mail => mail.Received.Date == DateTime.Now.Date)
                .OrderByDescending(mail => mail.Received);

            var emailsVandaagQuerySyntax = from mail in emails
                                           where mail.Received.Date == DateTime.Now.Date
                                           orderby mail.Received descending
                                           select mail;

            var emailsDitJaar = emails
                .Where(mail => mail.Received.Year == DateTime.Now.Year)
                .OrderByDescending(mail => mail.Received);

            Console.WriteLine("Geef een zoekterm in: ");
            string zoekterm = Console.ReadLine();
            var emailsMetKeyword = emails.Where(mail => mail.Body.ToLowerInvariant().Contains(zoekterm.ToLowerInvariant())).OrderByDescending(mail => mail.Received);
            Console.WriteLine(string.Join(" ", emailsMetKeyword));

            DateTimeFormatInfo formatInfo = DateTimeFormatInfo.CurrentInfo;
            Calendar calendar = formatInfo.Calendar;

            // email.Where(...).Count() werkt ook, maar dit is een kortere notatie
            int emailsDezeWeek = emails.Count(mail => mail.Received.Year == DateTime.Now.Year
            && calendar.GetWeekOfYear(mail.Received, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday));

            int emailsDezeWeekQuerySyntax = (from mail in emails
                                             where mail.Received.Year == DateTime.Now.Year
                                             && calendar.GetWeekOfYear(mail.Received, CalendarWeekRule.FirstDay, DayOfWeek.Monday) == calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                             select mail).Count();

            // Where kan hier ook, maar dan krijg je potentiëel meerdere mails terug als ze op exact hetzelfde moment zijn toegekomen, daarom First (geeft enkel het eerste resultaat terug)
            Email recentsteEmail = emails.First(mail => mail.Received == emails.Max(mail => mail.Received));

            Email recentsteEmailQuerySyntax = (from mail in emails
                                               where mail.Received == (from m in emails select m.Received).Max()
                                               select mail).First();

            // gezien emailsVandaag de recentste mails eerst zet, kan je die query ook herbruiken
            Email recentsteMailAlternatief = emailsVandaag.First();
        }

        private static Vector<Email> InitMails()
        {
            return new Vector<Email>()
            {
                new Email {
                    From = "wim.hambrouck@ehb.be",
                    To = new List<string> { "dt_studenten@ehb.be" },
                    Subject = "Aankondiging",
                    Body = "Beste studenten,\n\nGebruik enkel de voorbeeldoplossing als je écht vast zit. Door zelf dingen uit te zoeken, leer je het meeste bij!\n\nMet vriendelijke groeten,\nWim Hambrouck",
                    Received = DateTime.Now
                },
                new Email {
                    From = "wim.hambrouck@ehb.be",
                    To = new List<string> { "test@test.be", "thisisnotavalidemail.com" },
                    Subject = "Onderwerp",
                    Body = RandomString(1000),
                    Received = new DateTime(2019, 10, 28)
                },
                new Email {
                    From = "wim.hambrouck@ehb.be",
                    To = new List<string> { "test@test.be", "persoon@domain.land" },
                    Subject = "Onderwerp",
                    Body = RandomString(1000),
                    Received = DateTime.Now.AddDays(-1)
                },
                new Email {
                    From = "wim.hambrouck@ehb.be",
                    To = new List<string> { "test@test.be", "persoon@domain.land" },
                    Subject = "Onderwerp",
                    Body = RandomString(1000),
                    Received = DateTime.Now.AddYears(-1)
                },
                new Email {
                    From = "wim.hambrouck@ehb.be",
                    To = new List<string> { "test@test.be", "persoon@domain.land" },
                    Subject = "Onderwerp",
                    Body = RandomString(1000),
                    Received = DateTime.Now.AddHours(-5)
                }
            };
        }

        private static readonly Random random = new Random();
        /// <summary>
        /// Genereert willekeurige string van een bepaalde lengte. Bron: https://stackoverflow.com/a/1344242
        /// </summary>
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789 ";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
