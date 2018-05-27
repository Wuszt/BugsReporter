using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace BugsReporterClient
{
    public class Attachments : IDisposable
    {
        private string[] m_files = null;

        private Image m_screenShot = null;

        private readonly System.Drawing.Imaging.ImageFormat c_screenFormat = System.Drawing.Imaging.ImageFormat.Jpeg;

        private readonly string c_tmpZipFilePath = "tmp.zip";

        private string ScreenShotFileName
        {
            get
            {
                return "Screenshot." + c_screenFormat.ToString().ToLower();
            }
        }

        private Attachments(string[] files, Image screenShot)
        {
            m_files = files;

            m_screenShot = screenShot;
        }

        public Attachments(string[] files, bool makeAndAttachScreenshot = false) : this(files, makeAndAttachScreenshot ? Attachments.GetScreenShot() : null)
        {
        }

        public Attachments(string[] files, string screenshotPath) : this(files, Image.FromFile(screenshotPath))
        { }

        public void UpdateCustomFiles(string[] files)
        {
            m_files = files;
        }

        public void ResetScreenShot(bool remake)
        {
            m_screenShot = remake ? GetScreenShot() : null;
        }

        public void GetCompressedAttachments()
        {
            if (File.Exists(c_tmpZipFilePath))
                File.Delete(c_tmpZipFilePath);

            using (ZipArchive archive = ZipFile.Open(c_tmpZipFilePath, ZipArchiveMode.Create))
            {
                for (int i = 0; i < m_files.Length; ++i)
                {
                    archive.CreateEntryFromFile(m_files[i], Path.GetFileName(m_files[i]));
                }

                if (m_screenShot != null)
                {
                    var entry = archive.CreateEntry(ScreenShotFileName);
                    m_screenShot.Save(entry.Open(), c_screenFormat);
                }
            }
        }

        public void Dispose()
        {
            if (m_screenShot != null)
                m_screenShot.Dispose();
        }

        public static Image GetScreenShot()
        {
            Bitmap bmpScreenCapture = new Bitmap(SystemInformation.VirtualScreen.Width, SystemInformation.VirtualScreen.Height);

            using (Graphics g = Graphics.FromImage(bmpScreenCapture))
            {
                g.CopyFromScreen(SystemInformation.VirtualScreen.Left,
                                 SystemInformation.VirtualScreen.Top,
                                 0, 0,
                                 bmpScreenCapture.Size,
                                 CopyPixelOperation.SourceCopy);
            }

            return bmpScreenCapture;
        }
    }
}
