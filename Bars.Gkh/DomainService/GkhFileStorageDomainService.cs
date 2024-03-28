using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Application;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class GkhFileStorageDomainService<T> : FileStorageDomainService<T> where T : BaseGkhEntity
    {
        private DirectoryInfo fileStorageDirectory;

        private DirectoryInfo FileStorageDirectory
        {
            get
            {
                if (this.fileStorageDirectory != null)
                {
                    return this.fileStorageDirectory;
                }

                var configPath = this.Container.Resolve<IConfigProvider>().GetConfig().ModulesConfig["Bars.B4.Modules.FileSystemStorage"]
                    .GetAs("FileDirectory", string.Empty);
                var path = Path.IsPathRooted(configPath) 
                    ? configPath 
                    : ApplicationContext.Current.MapPath("~/" + configPath.TrimStart('~', '/'));
                this.fileStorageDirectory =new DirectoryInfo(path);
                
                if (!this.fileStorageDirectory.Exists)
                {
                    this.fileStorageDirectory.Create();
                }

                return this.fileStorageDirectory;
            }
        }

        private string GetFileFullPath(FileInfo file) =>
            file != null
            ? Path.Combine(this.FileStorageDirectory.FullName,
                file.ObjectCreateDate.Year.ToString(),
                file.ObjectCreateDate.Month.ToString(),
                $"{file.Id}.{file.Extention}")
            : null;

        /// <summary>
        /// Удаляет файл из хранилища.
        /// </summary>
        /// <param name="file"></param>
        public void DeleteFileFromStorage(FileInfo file)
        {
            var fullPath = this.GetFileFullPath(file);

            if (!File.Exists(fullPath))
            {
                return;
            }
            
            File.Delete(fullPath);
        }

        /// <summary>
        /// Удаляет запись о файле из БД.
        /// </summary>
        public void DeleteFileFromDb(FileInfo file)
        {
            if (file == null)
            {
                return;
            }

            this.FileInfoService.Delete(file.Id);
        }

        /// <inheritdoc />
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            this.InTransaction(() =>
            {
                var filesForDelete = new List<FileInfo>();
                
                foreach (var id in ids)
                {
                    filesForDelete.AddRange(this.GetFileInfoValues(this.Get(id)));
                    this.Delete((object)id);
                }

                filesForDelete.Where(file => file != null).ForEach(this.DeleteFileFromStorage);
            });

            return new BaseDataResult(ids);
        }
        
        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<T>();
            var filesForDelete = new Dictionary<string, FileInfo>();
            var saveParam = this.GetSaveParam(baseParams);
            this.InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    // Получаем обновления, исключая из обработки поля FileInfo
                    var value = record.AsObject(this.FileProperties.Select(x => x.Name).ToArray());

                    // Устанавливаем новые значения + получаем список файлов на удаление
                    foreach (var fileData in baseParams.Files)
                    {
                        //модификация для сжатия изображения и подобных операций
                        this.ModifyFileData(fileData.Value);
                        filesForDelete.Add(fileData.Key, this.SetFileInfoValue(value, fileData));
                    }

                    // Пробегаем по всем полям типа FileInfo и проверяем пришли ли новые файлы с клиента.
                    // Если нет, то проверяем удаление файла вызвав SetFileInfoValue - выставит null + вернет текущий файл для удаления
                    foreach (var fileProperty in this.FileProperties)
                    {
                        if (record.Properties[fileProperty.Name] == null
                            && !filesForDelete.ContainsKey(fileProperty.Name))
                        {
                            filesForDelete.Add(fileProperty.Name, this.SetFileInfoValue(value, new KeyValuePair<string, FileData>(fileProperty.Name, null)));
                        }
                    }

                    this.UpdateInternal(value);

                    values.Add(value);
                }

                //удаление файлов
                var files = filesForDelete.Values.Where(x => x != null).ToList();
                files.ForEach(this.DeleteFileFromDb);
                files.ForEach(this.DeleteFileFromStorage);
            });

            return new BaseDataResult(values);
        }

        /// <summary>
        /// Модифицирует файлы
        /// </summary>
        protected virtual void ModifyFileData(FileData fileData)
        {

        }
    }
}
