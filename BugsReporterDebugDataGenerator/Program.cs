using System;
using System.Threading;

namespace BugsReporterDebugDataGenerator
{
    class Program
    {
        private static int s_currentProgress;
        private const int c_dataAmount = 2000;
        private const string c_serverAddress = "http://wusztserver.ddns.net:776/api/";
        #region LoremIpsum
        private const string c_loremIpsum = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse ac tempor dui, facilisis dignissim magna. Nam vitae feugiat nisi, in hendrerit nunc. Suspendisse ut molestie mi, eu egestas orci. Pellentesque semper vestibulum est, non commodo purus. Nulla ultricies ut sem auctor vulputate. Curabitur efficitur dolor aliquam ullamcorper vulputate. Ut rhoncus non ipsum a pharetra. In in tortor aliquet, pulvinar ante vel, maximus leo.Suspendisse at finibus tellus.Fusce a libero eget lacus blandit viverra. Lorem ipsum dolor sit amet, consectetur adipiscing elit.Quisque tristique est tincidunt massa eleifend, eu mattis dolor lacinia. Vestibulum at enim et elit euismod scelerisque sed at dolor. Morbi id metus vel dui varius convallis.Quisque posuere est eget justo condimentum, at dictum risus luctus. Nullam porta sagittis dignissim. Aliquam accumsan molestie eleifend. Pellentesque at tincidunt augue, pharetra faucibus odio.Suspendisse potenti. Quisque eleifend quam eget sapien fringilla maximus.Proin vel metus auctor, elementum ipsum at, ornare elit. Nam convallis, mauris ut volutpat posuere, metus elit scelerisque mauris, ut aliquam felis nulla a sem.Vestibulum at tellus eros. Proin vel neque augue. Praesent rhoncus erat ante, id cursus odio mattis vel.Donec venenatis interdum quam id iaculis. Nunc placerat, arcu quis gravida eleifend, orci nisl viverra diam, a posuere ex eros sit amet lorem.Nam semper lorem eu turpis tristique, nec molestie purus consequat. Phasellus in nulla risus. Aenean nec ante vel nibh rutrum pulvinar.Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas.Praesent porttitor dapibus mauris in aliquam.Pellentesque posuere eleifend faucibus. Morbi ac erat venenatis turpis vulputate congue.Donec at sollicitudin ligula. Vestibulum rhoncus aliquet dui, vel tincidunt ipsum imperdiet eget.In hac habitasse platea dictumst.Ut velit purus, sagittis ac libero sit amet, faucibus placerat arcu.Aenean at elit lectus. Praesent pellentesque facilisis ultricies. Nullam et velit mollis, tempus lectus et, viverra neque. Phasellus volutpat elit a malesuada blandit. Donec ante tortor, rhoncus vitae pulvinar eu, molestie in lorem.Vivamus mollis, felis vel eleifend dictum, turpis eros condimentum ante, sit amet posuere arcu metus sit amet ligula.Aenean tincidunt augue mi, sit amet fermentum nulla consectetur in. Nulla congue neque eget felis dapibus, ut elementum felis viverra. Aliquam velit nisl, faucibus in malesuada et, placerat ac dui.In fermentum vitae metus et mattis. Duis lorem mauris, egestas eu ante ut, iaculis faucibus turpis.Etiam auctor metus ac molestie tempor. Nullam sit amet sem purus.Quisque imperdiet arcu mollis venenatis fermentum. Aliquam et interdum dui. Etiam auctor, velit ut ultrices fringilla, ligula metus venenatis nunc, sit amet venenatis ante justo vel nulla.Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Nulla sollicitudin dui sit amet eros dictum, eget ornare enim faucibus.Morbi et orci et neque feugiat vestibulum. Cras sapien turpis, semper vestibulum ultricies in, rhoncus viverra mi.";
        #endregion
        static void Main(string[] args)
        {
            s_currentProgress = 0;
            for(int i=0;i<8;++i)
            {
                Thread thread = new Thread(GenerateData);
                thread.Start();
            }

            while (s_currentProgress < c_dataAmount)
            {
                Console.WriteLine(s_currentProgress + "/" + c_dataAmount);
                Thread.Sleep(1000);
            }
        }

        private static void GenerateData()
        {
            Random rr = new Random();

            while(s_currentProgress < c_dataAmount)
            {
                s_currentProgress++;

                BugsReporterClient.IssuesSender sender = new BugsReporterClient.IssuesSender(c_serverAddress, false, new string[] { "lol.txt", "Screenshot.jpeg" });

                string error = "";

                try
                {
                    GetRandomStack();
                }
                catch (System.Exception e)
                {
                    error = e.ToString() + "\n" + e.StackTrace;
                }

                string title = error;

                int spaceIndex = title.IndexOf(" ");

                if (spaceIndex > -1)
                    title = title.Substring(0, spaceIndex);

                int startIndex = rr.Next(0, c_loremIpsum.Length - 251);
                int length = rr.Next(10, 250);

                sender.SendIssue(error, "Some guy" + rr.Next(), title, c_loremIpsum.Substring(startIndex, length));
            }
        }

        //private static string GetRandomError()
        //{
        //    Random rr = new Random();

        //    int winner = rr.Next(0, 12);

        //    switch (winner)
        //    {
        //        case 0:
        //            return new System.NullReferenceException().ToString();

        //        case 1:
        //            return new System.DllNotFoundException().ToString();

        //        case 2:
        //            return new System.DivideByZeroException().ToString();

        //        case 3:
        //            return new System.DuplicateWaitObjectException().ToString();

        //        case 4:
        //            return new System.FieldAccessException().ToString();

        //        case 5:
        //            return new System.FormatException().ToString();

        //        case 6:
        //            return new System.NotImplementedException().ToString();

        //        case 7:
        //            return new System.IndexOutOfRangeException().ToString();

        //        case 8:
        //            return new System.InsufficientExecutionStackException().ToString();

        //        case 9:
        //            return new System.InsufficientMemoryException().ToString();

        //        case 10:
        //            return new System.AggregateException().ToString();

        //        case 11:
        //            return new System.AccessViolationException().ToString();

        //        case 12:
        //            return new System.ApplicationException().ToString();

        //        default:
        //            return "Random error";
        //    }
        //}

        private static void ThrowRandomException()
        {
            Random rr = new Random();

            int winner = rr.Next(0, 12);

            switch (winner)
            {
                case 0:
                    throw new System.NullReferenceException();

                case 1:
                    throw new System.DllNotFoundException();

                case 2:
                    throw new System.DivideByZeroException();

                case 3:
                    throw new System.DuplicateWaitObjectException();

                case 4:
                    throw new System.FieldAccessException();

                case 5:
                    throw new System.FormatException();

                case 6:
                    throw new System.NotImplementedException();

                case 7:
                    throw new System.IndexOutOfRangeException();

                case 8:
                    throw new System.InsufficientExecutionStackException();

                case 9:
                    throw new System.InsufficientMemoryException();

                case 10:
                    throw new System.AggregateException();

                case 11:
                    throw new System.AccessViolationException();

                case 12:
                    throw new System.ApplicationException();

                default:
                    throw new System.Exception();
            }
        }

        private static void GetRandomStack(Random rr = null)
        {
            if (rr == null)
                rr = new Random();

            int winner = rr.Next(0, 3);

            if (winner == 0)
                ThrowRandomException();
            else
                ProxyFunction(rr);
        }

        private static void ProxyFunction(Random rr = null)
        {
            int winner = rr.Next(0, 7);

            switch (winner)
            {
                case 0:
                    FakeFunction(rr);
                    break;

                case 1:
                    Foo(rr);
                    break;

                case 2:
                    Func(rr);
                    break;

                case 3:
                    WtfIsThisFunction(rr);
                    break;

                default:
                    GetRandomStack(rr);
                    break;
            }
        }

        private static void FakeFunction(Random rr)
        {
            ProxyFunction(rr);
        }

        private static void Foo(Random rr)
        {
            ProxyFunction(rr);
        }

        private static void Func(Random rr)
        {
            ProxyFunction(rr);
        }

        private static void WtfIsThisFunction(Random rr)
        {
            ProxyFunction(rr);
        }
    }
}
