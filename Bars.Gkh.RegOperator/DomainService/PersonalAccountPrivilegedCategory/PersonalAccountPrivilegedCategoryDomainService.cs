namespace Bars.Gkh.RegOperator.DomainService.PersonalAccountPrivilegedCategory
{
    using System.Collections.Generic;
    using B4;
    using B4.Utils;
    using Entities.PersonalAccount;
    using Gkh.Entities;

    /// <summary>
    /// Перопределенная логика для вклинивания в UpDate
    /// </summary>
    public class PersonalAccountPrivilegedCategoryDomainService : BaseDomainService<PersonalAccountPrivilegedCategory>
    {
        /// <summary>
        /// Домен сервис для легковесное сущность для хранения изменения сущности
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }
        /// <summary>
        /// Сервис сохронение логово 
        /// </summary>
        public IPersonalAccountPrivilegedCategoryService CategoryService { get; set; }

        /// <summary>
        /// Обновление 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<PersonalAccountPrivilegedCategory>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var privilegedCategoryId = record.Entity.PrivilegedCategory.Id;
                    var privilegedCategoryName = record.Entity.PrivilegedCategory.Name;
                    var dateFrom = record.Entity.DateFrom;
                    var dateTo = record.Entity.DateTo;
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);

                    if (dateTo != value.DateTo)
                    {
                        CategoryService.SaveLog(value, "DateTo", "Изменение льготы действует по " + dateTo.ToStr() + " Изменение: " + value.DateTo.ToStr(), value.DateTo.ToStr());
                    }
                    if (dateFrom != value.DateFrom)
                    {
                        CategoryService.SaveLog(value, "DateFrom", "Изменение льготы действует с "+ dateFrom.ToStr()+" Изменение: " + value.DateFrom.ToStr(),  value.DateFrom.ToStr());
                    }
                    if (privilegedCategoryId != value.PrivilegedCategory.Id)
                    {
                        CategoryService.SaveLog(value, "PrivilegedCategory", "Изменение льготы "+ privilegedCategoryName + " Изменение: "+ value.PrivilegedCategory.Name, value.PrivilegedCategory.Name);
                    }
                }
            });

            return new BaseDataResult(values);
        }
    }
}