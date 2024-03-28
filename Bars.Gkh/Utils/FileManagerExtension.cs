namespace Bars.Gkh.Utils
{
    using System;
    using System.IO;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.FileManager;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// Расширение <see cref="IFileManager"/>
    /// </summary>
    public static class FileManagerExtension
    {
        /// <summary>
        /// Получить файловый поток для чтения
        /// <para>Возвращает null, если файл не найден</para>
        /// </summary>
        /// <exception cref="MissingMethodException"></exception>
        public static FileStream GetFileStream(this IFileManager fileManager, FileInfo fileInfo, bool noError = false)
        {
            var systemFileManager = fileManager as FileSystemFileManager;

            if (systemFileManager == null)
            {
                throw new MissingMethodException(nameof(FileSystemFileManager), "GetFileStream");
            }

            try
            {
                return new FileStream(systemFileManager.GetFilePath(fileInfo), FileMode.Open, FileAccess.Read);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
            catch (DirectoryNotFoundException)
            {
                return null;
            }
            catch
            {
                if (noError)
                {
                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// Проверяет наличие файла в файловом хранилище
        /// </summary>
        /// <exception cref="MissingMethodException"></exception>
        public static bool IsExistsFile(this IFileManager fileManager, FileInfo fileInfo)
        {
            var systemFileManager = fileManager as FileSystemFileManager;

            if (systemFileManager == null)
            {
                throw new MissingMethodException(nameof(FileSystemFileManager), "IsExistsFile");
            }

            return File.Exists(systemFileManager.GetFilePath(fileInfo));
        }

        /// <summary>
        /// Сохраняет файл по полному пути
        /// </summary>
        /// <param name="fileManager">Файловый менеджер</param>
        /// <param name="filePath">Полный путь до сохраняемого файла</param>
        /// <exception cref="MissingMethodException"></exception>
        public static FileInfo SaveFile(this IFileManager fileManager, string filePath)
        {
            var systemFileManager = fileManager as FileSystemFileManager;

            if (systemFileManager == null)
            {
                throw new MissingMethodException(nameof(FileSystemFileManager), "IsExistsFile");
            }

            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return systemFileManager.SaveFile(fs, filePath);
            }
        }
    }
}