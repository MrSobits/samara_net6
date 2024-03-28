namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers
{
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;

    using Bars.B4;
    using Bars.B4.Application;

    using Bars.B4.Modules.FileStorage;
    using System.Drawing;

    using Bars.B4.Utils;

    using PdfSharp.Drawing;
    using PdfSharp.Pdf.IO;
    using PdfSharp.Pdf;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    public class SmevPrintPdfHelper : ISmevPrintPdfHelper
    {
        public IWindsorContainer Container { get; set; }

        protected virtual Bitmap Stamp => Properties.Resources.stamp2;
       

        //TODO: заменить хардкод на сертификат из ответа
        #region HARDCODECERTIFICATE
        private string _certificate =
            "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";
        #endregion

        public byte[] GetPdfExtract(Bars.B4.Modules.FileStorage.FileInfo file, string resource)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var container = ApplicationContext.Current.Container;
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var fopDirectory = appSettings.GetAs<string>("FopDirectory");
            var logger = this.Container.Resolve<ILogger>();

            //загружаем xml
            var xmlStream = fileManager.GetFile(file);
            var xmlPath = Path.GetTempFileName();
            var xmlFileStream = new FileStream(xmlPath, FileMode.Create);
            xmlStream.Seek(0, SeekOrigin.Begin);
            xmlStream.CopyTo(xmlFileStream);
            xmlStream.Close();
            xmlFileStream.Close();

            var res = container.Resolve<IResourceManifestContainer>();

            var xslPath = Path.GetTempFileName();
            // TODO: Расскоментировать
           /* var xslFileStream = new FileStream(xslPath, FileMode.Create);
            using (var xsltStream = res.Resources[resource].GetStream())
            {
                xsltStream.Seek(0, SeekOrigin.Begin);
                xsltStream.CopyTo(xslFileStream);
            }

            xslFileStream.Close();*/

            var pdfPath = Path.GetTempFileName();

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.UseShellExecute = false;
            startInfo.Arguments = $"/C  {fopDirectory}/fop -c {fopDirectory}/cfg.xml -r -xml \"{xmlPath}\" -xsl \"{xslPath}\" -pdf \"{pdfPath}\"";
            startInfo.WorkingDirectory = fopDirectory;
            startInfo.RedirectStandardOutput = true;
            process.StartInfo = startInfo;
            process.Start();
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            // Write the redirected output to this application's window.
            logger.LogInformation(output);
            process.WaitForExit();
            if (System.IO.File.Exists(xmlPath))
            {
                System.IO.File.Delete(xmlPath);
            }

            if (System.IO.File.Exists(xslPath))
            {
                System.IO.File.Delete(xslPath);
            }

            byte[] result;
            
            if (this.Stamp != null && this._certificate != null)
            {
                var bmp = Utils.Utils.GetFullStamp(this.Stamp, this._certificate);
                var signedStream = new MemoryStream();
                using (var unsignedPdf = File.Open(pdfPath, FileMode.Open))
                {
                    unsignedPdf.Seek(0, SeekOrigin.Begin);
                    unsignedPdf.CopyTo(signedStream);
                    
                    signedStream.Position = 0;
                    PdfDocument pdfDocument = PdfReader.Open(signedStream, 0);
                    PdfPage newPage = pdfDocument.Pages[pdfDocument.Pages.Count-1];
                    XGraphics gfx = XGraphics.FromPdfPage(newPage);

                    MemoryStream strm = new MemoryStream();
                    bmp.Save(strm, System.Drawing.Imaging.ImageFormat.Png);

                    gfx.DrawImage(XImage.FromStream(strm), 200, 700, 225, 99);
                    pdfDocument.Save(signedStream);
                    result = signedStream.ReadAllBytes();
                    signedStream.Close();
                }
                
            }
            else
                result = System.IO.File.ReadAllBytes(pdfPath);

            if (System.IO.File.Exists(pdfPath))
            {
                System.IO.File.Delete(pdfPath);
            }

            return result;
        }
    }
}