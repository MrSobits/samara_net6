namespace Bars.GkhRf.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;
    using Bars.GkhRf.Enums;

    public class ContractRfDomainService : FileStorageDomainService<ContractRf>
    {        
        public override ContractRf Get(object id)
        {
            var obj = this.Container.Resolve<IDomainService<ContractRf>>().Load(id);

            if (obj.ManagingOrganization != null)
            {
                var contragent = this.Container.Resolve<IDomainService<Contragent>>().Get(obj.ManagingOrganization.Contragent.Id);

                obj.ManagingOrganization.ContragentName = contragent.Name;
            }

            return obj;
        }

        public override IDataResult Update(BaseParams baseParams)
        {
            var fileInfoService = Container.Resolve<IDomainService<FileInfo>>();
            var values = new List<ContractRf>();
            InTransaction(() =>
            {                
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var oldValue = this.GetAll()
                        .Select(x => new
                        {
                            x.Id,
                            x.DateEnd
                        })
                        .FirstOrDefault(x => x.Id == record.Entity.Id);

                    // Исключаем из обработки поля FileInfo
                    var value = record.AsObject(FileProperties.Select(x => x.Name).ToArray());

                    // Устанавливаем новые значения + получаем список файлов на удаление
                    var filesForDelete = baseParams.Files.ToDictionary(
                        fileData => fileData.Key, fileData => SetFileInfoValue(value, fileData));

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

#warning Поскольку в FileManager запретили удаление файлов, то неудаляем пока
                    /*
                    // Удаляем файлы из списка на удаление
                    foreach (var file in filesForDelete.Values.Where(file => file != null))
                    {
                        fileInfoService.Delete(file.Id);
                    }
                    */

                    if (oldValue != null && value.DateEnd != oldValue.DateEnd)
                    {
                        this.UpdateDateExclude(value);
                    }

                    values.Add(value);
                }
            });

            return new BaseDataResult(values);
        }

        private void UpdateDateExclude(ContractRf contractRf)
        {
            // все дома из включенных переносятся в исключенные, и проставляется дата исключения равная дате окончания договора
            var serviceRfObject = this.Container.Resolve<IDomainService<ContractRfObject>>();

            var contractObjs = serviceRfObject.GetAll()
                .Where(x => x.ContractRf.Id == contractRf.Id)
                .ToList();

            foreach (var rec in contractObjs)
            {
                rec.TypeCondition = TypeCondition.Exclude;

                if (rec.ExcludeDate == null)
                {
                    rec.ExcludeDate = contractRf.DateEnd;
                }

                serviceRfObject.Update(rec);
            }
        }
    }
}