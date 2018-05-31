using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugsReporterDebugDataGenerator
{
    class Program
    {
        private const int c_dataAmount = 3000;
        private const string c_serverAddress = "http://localhost:18982/api/";

        static void Main(string[] args)
        {
            Random rr = new Random();

            for(int i=0;i<c_dataAmount;++i)
            {
                Console.WriteLine(i + "/" + c_dataAmount);

                BugsReporterClient.IssuesSender sender = new BugsReporterClient.IssuesSender(c_serverAddress, false, new string[]{ "lol.txt", "Screenshot.jpeg" });

                string error = GetRandomError();
                string title = error;

                int spaceIndex = title.IndexOf(" ");

                if (spaceIndex > -1)
                    title = title.Substring(0, spaceIndex);

                sender.SendBug(error, "Some guy" + rr.Next(), title, "Whatever");
            }
        }

        private static string GetRandomError()
        {
            Random rr = new Random();

            int winner = rr.Next(0, 12);

            switch(winner)
            {
                case 0:
                    return new System.NullReferenceException().ToString();

                case 1:
                    return new System.DllNotFoundException().ToString();

                case 2:
                    return new System.DivideByZeroException().ToString();

                case 3:
                    return new System.DuplicateWaitObjectException().ToString();

                case 4:
                    return new System.FieldAccessException().ToString();

                case 5:
                    return new System.FormatException().ToString();

                case 6:
                    return new System.NotImplementedException().ToString();

                case 7:
                    return new System.IndexOutOfRangeException().ToString();

                case 8:
                    return new System.InsufficientExecutionStackException().ToString();

                case 9:
                    return new System.InsufficientMemoryException().ToString();

                case 10:
                    return new System.AggregateException().ToString();

                case 11:
                    return new System.AccessViolationException().ToString();

                case 12:
                    return new System.ApplicationException().ToString();

                default:
                    return "Random error";
            }
        }
    }
}
