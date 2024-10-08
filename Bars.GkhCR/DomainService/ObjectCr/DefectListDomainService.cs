﻿using System.Collections.Generic;
using Bars.B4.DomainService.BaseParams;
using Bars.B4.Modules.FileStorage.DomainService;

namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.DomainService;

    using Entities;

    public class DefectListDomainService : GkhFileStorageDomainService<DefectList>
    {
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<DefectList>();
            var filesForDelete = new Dictionary<string, FileInfo>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    // Получаем обновления, исключая из обработки поля FileInfo
                    var value = record.AsObject(FileProperties.Select(x => x.Name).ToArray());

                    // Устанавливаем новые значения + получаем список файлов на удаление
                    foreach (var fileData in baseParams.Files)
                    {
                        filesForDelete.Add(fileData.Key, SetFileInfoValue(value, fileData));
                    }

                    // Пробегаем по всем полям типа FileInfo и проверяем пришли ли новые файлы с клиента.
                    // Если нет, то проверяем удаление файла вызвав SetFileInfoValue - выставит null + вернет текущий файл для удаления
                    foreach (var fileProperty in FileProperties)
                    {
                        if (record.Properties[fileProperty.Name] == null
                            && !filesForDelete.ContainsKey(fileProperty.Name))
                        {
                            filesForDelete.Add(fileProperty.Name, SetFileInfoValue(value, new KeyValuePair<string, FileData>(fileProperty.Name, null)));
                        }
                    }

                    UpdateInternal(value);

                    values.Add(value);
                }
            });

            this.DeleteFiles(filesForDelete.Values.Where(x => x != null).ToList());
            return new BaseDataResult(values);
        }

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            var filesForDelete = new List<FileInfo>();
            this.InTransaction(() =>
            {
                foreach (var id in ids)
                {
                    filesForDelete.AddRange(this.GetFileInfoValues(this.Get(id)));
                    this.DeleteInternal(id);
                }
            });

            this.DeleteFiles(filesForDelete.Where(x => x != null).ToList());
            return new BaseDataResult(ids);
        }

        private void DeleteFiles(List<FileInfo> files)
        {
            // удаление файла вынесен и обернут в try catch, т.к. есть дефектные ведомости, которые создались из деф. ведомостей долгосрочной программы
            // и они ссылаются на один и тот же файл, и соответственно если у нас есть ссылки на этот файл, то он не должен удалится
            using (var dataTransaction = this.BeginTransaction())
            {
                try
                {
                    files.ForEach(this.DeleteFileFromDb);
                    dataTransaction.Commit();
                    files.ForEach(this.DeleteFileFromStorage);
                }
                catch
                {
                    dataTransaction.Rollback();
                }
            }
        }
    }
}