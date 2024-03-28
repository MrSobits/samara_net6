namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    using System.Collections.Generic;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Modules.FileStorage.DomainService;


    using Entities;

    public class SpecialDefectListDomainService : FileStorageDomainService<SpecialDefectList>
    {
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<SpecialDefectList>();
            var filesForDelete = new Dictionary<string, FileInfo>();
            this.InTransaction(() =>
            {
                var saveParam = this.GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    // Получаем обновления, исключая из обработки поля FileInfo
                    var value = record.AsObject(this.FileProperties.Select(x => x.Name).ToArray());

                    // Устанавливаем новые значения + получаем список файлов на удаление
                    foreach (var fileData in baseParams.Files)
                    {
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
            });

            // удаление файла вынесен и обернут в try catch, т.к. есть дефектные ведомости, которые создались из деф. ведомостей долгосрочной программы
            // и они ссылаются на один и тот же файл, и соответственно если у нас есть ссылки на этот файл, то он не должен удалится
            using (IDataTransaction dataTransaction = this.BeginTransaction())
            {
                try
                {
                    foreach (var file in filesForDelete.Values.Where(file => file != null))
                    {
                        this.FileInfoService.Delete(file.Id);
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
            this.InTransaction(() =>
            {
                foreach (var id in ids)
                {
                    filesForDelete.AddRange(this.GetFileInfoValues(this.Get(id)));
                    this.DeleteInternal(id);
                }
            });

            // удаление файла вынесен и обернут в try catch, т.к. есть дефектные ведомости, которые создались из деф. ведомостей долгосрочной программы
            // и они ссылаются на один и тот же файл, и соответственно если у нас есть ссылки на этот файл, то он не должен удалится
            using (IDataTransaction dataTransaction = this.BeginTransaction())
            {
                try
                {
                    foreach (var file in filesForDelete.Where(file => file != null))
                    {
                        this.FileInfoService.Delete(file.Id);
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