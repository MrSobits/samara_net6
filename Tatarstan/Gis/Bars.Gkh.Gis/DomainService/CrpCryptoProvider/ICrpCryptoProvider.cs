namespace Bars.Gkh.Gis.DomainService.CrpCryptoProvider
{
    public interface ICrpCryptoProvider
    {
        /// <summary>
        /// Расшифровка файла с помощью дешифратора LCDeast
        /// </summary>
        /// <param name="pathToFile">Полный путь к зашифрованному файлу</param>
        /// <returns>Путь к расшифрованному файлу</returns>
        string Decrypt(string pathToFile);
    }
}