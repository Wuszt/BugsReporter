using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BugsReporterClientView
{
    public partial class App : Application
    {
        void AppStart(object sender, StartupEventArgs e)
        {
            MainWindow window = new MainWindow(GetAttachmentsArrayFromArgs(e), CheckIfShouldAttachScreenshotFromArgs(e));
            window.Show();
        }

        private string[] GetAttachmentsArrayFromArgs(StartupEventArgs startupArgs)
        {
            List<string> result = new List<string>();

            var args = startupArgs.Args;

            int counter = 0;

            for(int i=0;i<args.Length;++i)
            {
                if (args[i] == "att" + counter)
                {
                    result.Add(args[i + 1]);
                    ++counter;
                }
            }

            return result.ToArray();
        }

        private bool CheckIfShouldAttachScreenshotFromArgs(StartupEventArgs e)
        {
            return e.Args.Any(x => x == "attachScreenShot");
        }
    }
}
