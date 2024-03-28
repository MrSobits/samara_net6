namespace Bars.Gkh.Gis.DomainService.JExtractor.Impl
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using B4;

    public class JExtractorService : IJExtractorService
    {
        protected string ExecutablePath = string.Format("{0}\\ExternalPrograms\\jar32\\jar32.exe"
            , Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path)));
        
        /// <summary>
        /// Распаковать архив в указанную директорию
        /// </summary>
        /// <param name="archive">путь к архиву</param>
        /// <param name="output">директория, куда требуется распаковать архив</param>
        /// <returns></returns>
        public IDataResult ExtractToDirectory(string archive, string output)
        {
            try
            {
                if (archive.Split('.').Last() != "j")
                {
                    return new BaseDataResult(false, "Файл не является архивом формата .j");
                }

                if (!Directory.Exists(output))
                {
                    Directory.CreateDirectory(output);
                }

                var arguments = string.Format("e {0} -o{1}", archive, output);
                var processInfo = new ProcessStartInfo(ExecutablePath, arguments)
                {
                    UseShellExecute = true
                };

                using (var process = Process.Start(processInfo))
                {
                    var result = process.WaitForExit(180000);
                    return new BaseDataResult(result, result ? "Данные успешно распакованы" : "Ошибка при распаковке данных");
                }
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}