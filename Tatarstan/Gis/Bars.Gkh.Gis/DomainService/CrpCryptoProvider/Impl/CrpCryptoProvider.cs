namespace Bars.Gkh.Gis.DomainService.CrpCryptoProvider.Impl
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

    public class CrpCryptoProvider : ICrpCryptoProvider
    {
        protected string ExecutablePath =
            $"{Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path))}\\ExternalPrograms\\crp\\Bars.Security.CrpCryptoProvider.exe";

        /// <summary>
        /// Расшифровка файла с помощью дешифратора LCDeast
        /// </summary>
        /// <param name="pathToFile">Полный путь к зашифрованному файлу</param>
        /// <returns>Путь к расшифрованному файлу</returns>
        public string Decrypt(string pathToFile)
        {
            if (!File.Exists(pathToFile))
                throw new FileNotFoundException("Файл " + pathToFile + " не существует.");

            var arguments = $" --decrypt \"{pathToFile}\"";
            var startInfo = new ProcessStartInfo(this.ExecutablePath, arguments);
            using (var process = Process.Start(startInfo))
            {
                process.WaitForExit();
                var code = process.ExitCode;
                if (code == 0)
                {
                    return pathToFile.Replace(Path.GetExtension(pathToFile), ".txt");
                }
            }

            throw new Exception($"Ошибка при расшифровке файла '{pathToFile}'");
        }
    }
}