using System.Collections.Generic;
using Bars.B4.DataAccess;
using Bars.B4.DomainService.BaseParams;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.FileStorage.DomainService;

namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ProtocolCrDomainService : FileStorageDomainService<ProtocolCr>
    {
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<ProtocolCr>();
            var filesForDelete = new Dictionary<string, FileInfo>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    // Получаем обновления, исключая из обработки поля FileInfo
                    var value = record.AsObject(FileProperties.Select(x => x.Name).ToArray());

                    // Устанавливаем новые значения + получаем список файлов на удаление
                    baseParams.Files.ForEach(fileData => filesForDelete[fileData.Key] = SetFileInfoValue(value, fileData));

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

            // удаление файла вынесен и обернут в try catch, т.к. есть протоколы, которые создались из протоколов долгосрочной программы
            // и они ссылаются на один и тот же файл, и соответственно если у нас есть ссылки на этот файл, то он не должен удалится
            using (IDataTransaction dataTransaction = this.BeginTransaction())
            {
                try
                {
                    foreach (var file in filesForDelete.Values.Where(file => file != null))
                    {
                        FileInfoService.Delete(file.Id);
                    }

                    dataTransaction.Commit();
                }
                catch
                {
                   dataTransaction.Rollback();              
                }
            }

            return new BaseDataResult(values);
        }

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            var filesForDelete = new List<FileInfo>();
            InTransaction(() =>
            {
                foreach (var id in ids)
                {
                    filesForDelete.AddRange(GetFileInfoValues(Get(id)));
                    DeleteInternal(id);
                }
            });

            // удаление файла вынесен и обернут в try catch, т.к. есть протоколы, которые создались из протоколов долгосрочной программы
            // и они ссылаются на один и тот же файл, и соответственно если у нас есть ссылки на этот файл, то он не должен удалится
            using (IDataTransaction dataTransaction = this.BeginTransaction())
            {
                try
                {
                    foreach (var file in filesForDelete.Where(file => file != null))
                    {
                        FileInfoService.Delete(file.Id);
                    }

                    dataTransaction.Commit();
                }
                catch
                {
                    dataTransaction.Rollback();
                }
            }

            return new BaseDataResult(ids);
        }
    }
}