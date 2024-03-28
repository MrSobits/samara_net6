namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    using System.Reflection;
    using B4.Modules.FileStorage.DomainService;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using System;

    /// <summary>
    /// Данный класс предназначен для того чтобы заменять file storage домен сервис 
    /// поскольку в регионах Сущности заменяются через subclass, то необходимо избежать того что ктото в коде сделает
    /// new Tbase(), затем Сохранит его через IDisposalService<Tbase> необходимо чтобы после такого действия Сохранение шло через сущность Tderived
    /// Который добавился в регионных модулях
    /// </summary>
    public class NewFileStorageDomainService<T> : FileStorageDomainService<T> where T : class, IEntity
    {
        private readonly Type _fileInfoType = typeof(FileInfo);

        ///// <summary>Свойства сущности с типом FileInfo</summary>
        //protected PropertyInfo[] FileProperties => _fileProperties ?? (_fileProperties = (from x in typeof(T).GetProperties()
        //                                                                                  where x.PropertyType == _fileInfoType
        //                                                                                  select x).ToArray());

        ///// <summary>Фаловый менеджер</summary>
        //protected IFileManager FileManager => _fileManager ?? (_fileManager = base.Container.Resolve<IFileManager>());

        //protected IDomainService<FileInfo> FileInfoService => _fileInfoService ?? (_fileInfoService = base.Container.Resolve<IDomainService<FileInfo>>());


        private PropertyInfo[] _fileProperties;

        private IDomainService<FileInfo> _fileInfoService;

        private IFileManager _fileManager;

        public override void Delete(object id)
        {
            InTransaction(delegate
            {
                IEnumerable<FileInfo> fileInfoValues = GetFileInfoValues(Get(id));
                DeleteInternal(id);
                foreach (FileInfo current in fileInfoValues.Where((FileInfo file) => file != null))
                {
                    try
                    {
                        FileInfoService.Delete(current.Id);
                    }
                    catch
                    { }
                }
            });
        }

        //protected IEnumerable<FileInfo> GetFileInfoValues(T value)
        //{
        //    FileInfo[] files = new FileInfo[FileProperties.Length];
        //    for (int i = 0; i < FileProperties.Length; i++)
        //    {
        //        files[i] = GetFileInfoValue(value, FileProperties[i]);
        //    }
        //    return files;
        //}
        //protected FileInfo GetFileInfoValue(T value, PropertyInfo propertyInfo)
        //{
        //    FileInfo result = null;
        //    if (value != null && propertyInfo.PropertyType == typeof(FileInfo))
        //    {
        //        result = (FileInfo)propertyInfo.GetValue(value, new object[0]);
        //    }
        //    return result;
        //}
    }
}
