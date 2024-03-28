namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.FileStorage;
    using B4.Utils;

    using Bars.B4.DataAccess;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain.ParameterVersioning;
    using Gkh.Entities;

    /// <summary>
    /// Сервис истории изменения лс
    /// </summary>
    public class PersonalAccountOperationLogService : IPersonalAccountOperationLogService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<EntityLogLight> EntityLogLightDomainService { get; set; }

        public IDomainService<PersonalAccountChange> AccountChangeDomain { get; set; }

        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <inheritdoc />
        public IDataResult GetOperationLog(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var id = baseParams.Params.GetAs("id", 0L);

            var result = this.GetEntityLogLightEntries(loadParams, id)
                .Union(this.GetPersonalAccountChangeEntries(loadParams, id))
                .OrderBy(x => x.DateApplied)
                .AsQueryable()
                .Filter(loadParams, this.Container)
                .Order(loadParams);

            return new ListDataResult(result.Paging(loadParams), result.Count());
        }

        private IEnumerable<PersonalAccountOperationLogEntry> GetPersonalAccountChangeEntries(LoadParam loadParam, long id)
        {
            var enumValues = Enum.GetNames(typeof (PersonalAccountChangeType))
                    .Select(x => typeof(PersonalAccountChangeType).GetMember(x).FirstOrDefault())
                .Select(
                    x =>
                    new
                    {
                            Value = Enum.Parse(typeof(PersonalAccountChangeType), x.Name),
                        Display = x.GetDisplayName()
                    })
                .ToDictionary(x => x.Value, x => x.Display);

            var entries = this.AccountChangeDomain
                .GetAll()
                .Where(x => x.PersonalAccount.Id == id)
                .Select(x => new
                {
                    ParameterName =
                    enumValues.ContainsKey(x.ChangeType)
                        ? enumValues[x.ChangeType]
                        : string.Empty,
                    PropertyDescription = x.Description,
                    DateActualChange = x.ActualFrom,
                    User = x.Operator,
                    DateApplied = x.Date,
                    x.NewValue,
                    x.Document,
                    x.ChangeType,
                    x.Reason
                })
                .AsEnumerable()
                .Select(x => new PersonalAccountOperationLogEntry
                {
                    ParameterName = x.ParameterName ?? string.Empty,
                    PropertyDescription = x.PropertyDescription ?? string.Empty,
                    DateActualChange = x.DateActualChange,
                    User = x.User,
                    DateApplied = x.DateApplied,
                    PropertyValue = this.RenderOperationValue(x.ChangeType, x.NewValue) ?? string.Empty,
                    Document = x.Document,
                    Reason = x.Reason ?? string.Empty
                });

            return entries;
        }

        private string RenderOperationValue(PersonalAccountChangeType changeType, string value)
        {
            if (changeType == PersonalAccountChangeType.OwnerChange)
            {
                var id = value.ToLong();
                var domain = this.Container.ResolveDomain<PersonalAccountOwner>();
                try
                {
                    return domain.Get(id).Return(x => x.Name, value);
                }
                finally
                {
                    this.Container.Release(domain);
                }
            }

            return value;
        }

        private IEnumerable<PersonalAccountOperationLogEntry> GetEntityLogLightEntries(LoadParam loadParam, long id)
        {
            var parameters = new[]
            {
                "account_open_date", "account_close_date", "account_external_num", "area_share",
                "Льготная категория", "Присвоение статуса \"Не активен\"", "Запрет перерасчета",
                "Печать в открытом периоде", "Снятие признака печати в открытом периоде", "room"
            };

            var roomParameters = new[]
                             {
                                 "room_area",
                                 "Тип собственности помещения"
                             };

            var roomId = this.AccountDomain.Get(id).Room.Return(x => x.Id);

            return this.EntityLogLightDomainService.GetAll()
                .Where(x => (x.EntityId == id && 
                    parameters.Contains(x.ParameterName) && 
                    x.ClassName == "BasePersonalAccount") 
                    ||
                    (x.EntityId == roomId && 
                    x.ParameterName == "room_area" && 
                    x.ClassName == "Room"))
                .Select(x => new
                {
                    x.ParameterName,
                    x.PropertyDescription,
                    x.PropertyValue,
                    x.DateActualChange,
                    x.User,
                    x.DateApplied,
                    x.Document,
                    x.Reason
                })
                .AsEnumerable()
                .Select(x => new PersonalAccountOperationLogEntry
                {
                    ParameterName =
                        VersionedEntityHelper.GetFriendlyName(
                            x.ParameterName) ?? string.Empty,
                    PropertyDescription = x.PropertyDescription ?? string.Empty,
                    PropertyValue = VersionedEntityHelper.RenderValue(x.ParameterName, x.PropertyValue) ?? string.Empty,
                    User = x.User ?? string.Empty,
                    DateActualChange =
                        x.DateActualChange,
                    DateApplied =
                        x.DateApplied.GetValueOrDefault(),
                    Document = x.Document,
                    Reason = x.Reason ?? string.Empty
                });
        }
    }

    public class PersonalAccountOperationLogEntry
    {
        public string ParameterName { get; set; }

        public string PropertyDescription { get; set; }

        public string PropertyValue { get; set; }

        public string User { get; set; }

        public DateTime? DateActualChange { get; set; }

        public DateTime DateApplied { get; set; }

        public FileInfo Document { get; set; }

        public string Reason { get; set; }
    }
}