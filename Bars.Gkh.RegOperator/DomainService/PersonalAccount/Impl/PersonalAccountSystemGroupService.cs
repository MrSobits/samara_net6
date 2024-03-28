namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils.Caching;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с системными группами
    /// </summary>
    public class PersonalAccountSystemGroupService : IPersonalAccountSystemGroupService
    {
        public IPersonalAccountGroupService PersonalAccountGroupService { get; set; }
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomainService { get; set; }
        public IDomainService<PersAccGroup> PersAccGroupDomainService { get; set; }
        public IDomainService<PersAccGroupRelation> PersAccGroupRelationDomainService { get; set; }
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Добавление лицевых счетов в системную группу
        /// </summary>
        /// <param name="accountsId">Идентификаторы ЛС</param>
        /// <param name="systemGroupName"></param>
        /// <param name="isNeedCreateSystemGroup">Создавать ли групу если ее нету в базе</param>
        public IDataResult AddPersonalAccountsToSystemGroup(List<long> accountsId, string systemGroupName, bool isNeedCreateSystemGroup = false)
        {
            if (accountsId.Count > 0)
            {
                var isExistSystemGroup = this.IsExistCurrentSystemGroup(systemGroupName);

                if (isExistSystemGroup)
                {
                    var systemGroupId = this.GetSystemGroupId(systemGroupName);
                    CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
                    return this.AddPersonalAccountsToSystemGroup(accountsId, systemGroupId);
                }

                if (isNeedCreateSystemGroup)
                {
                    var systemGroupId = this.CreateNewSystemGroup(systemGroupName);
                    CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
                    return this.AddPersonalAccountsToSystemGroup(accountsId, systemGroupId);
                }
                
                return BaseDataResult.Error($"Системной группы с именем \"{systemGroupName}\" не существует");
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Массовое исключение ЛС из системных групп
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <returns>Результат операции</returns>
        public IDataResult RemovePersonalAccountsFromSystemGroups(BaseParams baseParams, string systemGroupName)
        {
            var accountsIdString = baseParams.Params.GetAs<string>("accountsId");

            if (accountsIdString != null)
            {
                var accountsId = accountsIdString.ToLongArray();

                if (!this.IsContainsPersonalAccountsInSystemGroups(accountsId, this.GetSystemGroupId(systemGroupName)))
                {
                    return BaseDataResult.Error($"Необходимо выбрать ЛС, которые относятся к группе \"{systemGroupName}\"");
                }

                IQueryable<PersonalAccountDto> personalAccountsDto = this.BasePersonalAccountDomainService.GetAll().WhereContains(x => x.Id, accountsId).ToDto();
                var result = this.PersonalAccountGroupService.RemovePersonalAccountsFromGroups(personalAccountsDto, new[] { this.GetSystemGroupId(systemGroupName) });

                CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
                return result.Success ? result : BaseDataResult.Error(result.Message);
            }


            return BaseDataResult.Error("Ошибка в базовых параметрах запроса");
        }

        /// <summary>
        /// Возвращает количество лицевых счетов которые сосотоят в текущей системной группе "Сформирован документ в открытом периоде"
        /// </summary>
        /// <param name="systemGroupName">Имя системной группы</param>
        /// <returns>Результат запроса</returns>
        public IDataResult GetCountPersonalAccountsInSystemGroup(string systemGroupName)
        {
            return this.IsExistCurrentSystemGroup(systemGroupName) ?
                new BaseDataResult(this.PersAccGroupRelationDomainService.GetAll().Count(x => x.Group.Id == this.GetSystemGroupId(systemGroupName))) :
                new BaseDataResult(false, $"Системной группы {systemGroupName} не существует");
        }

        /// <summary>
        /// Исключение всех ЛС из системной группы
        /// </summary>
        /// <param name="systemGroupName"></param>
        public List<long> RemoveAllPersonalAccountsFromSystemGroup(string systemGroupName)
        {
            var personalAccRelationGroups = this.PersAccGroupRelationDomainService.GetAll()
                                                .WhereContains(x => x.Group.Id, new[] { this.GetSystemGroupId(systemGroupName) })
                                                .ToList();

            if (personalAccRelationGroups.IsNotEmpty())
            {
                NhExtentions.InTransaction(this.Container, () => personalAccRelationGroups.ForEach(x => this.PersAccGroupRelationDomainService.Delete(x.Id)));            
            }
            
            CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
            return personalAccRelationGroups.Select(x => x.PersonalAccount.Id).ToList();
        }

        private IDataResult AddPersonalAccountsToSystemGroup(List<long> accountsId, long systemGroupId)
        {
            var accountsIdArr = this.FilterAccountsForAddToSystemGroup(accountsId.ToArray(), systemGroupId);

            if (accountsIdArr.Length > 0)
            {
                long[] groupIds = { systemGroupId };
                IQueryable<PersonalAccountDto> personalAccountsDto =
                    this.BasePersonalAccountDomainService.GetAll().WhereContains(x => x.Id, accountsId).ToDto();
                
                CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
                return this.PersonalAccountGroupService.AddPersonalAccountsToGroups(personalAccountsDto, groupIds);
            }

            return new BaseDataResult {Success = false};
        }

        private long[] FilterAccountsForAddToSystemGroup(long[] accountsId, long systemGroupId)
        {
            var accountThatContainsInSystemGroup = this.PersAccGroupRelationDomainService.GetAll()
                .WhereContains(x => x.PersonalAccount.Id, accountsId)
                .Where(x => x.Group.Id == systemGroupId)
                .Select(x => x.PersonalAccount.Id)
                .AsEnumerable()
                .ToArray();

            return accountsId.Except(accountThatContainsInSystemGroup).ToArray();
        }

        private bool IsExistCurrentSystemGroup(string systemGroupName)
        {
            PersAccGroup systemGroup = this.PersAccGroupDomainService.GetAll().FirstOrDefault(x => x.Name == systemGroupName);
            return systemGroup != null;
        }

        private long GetSystemGroupId(string systemGroupName)
        {
            return this.PersAccGroupDomainService.GetAll().FirstOrDefault(x => x.Name == systemGroupName)?.Id ?? 0;
        }

        private long CreateNewSystemGroup(string groupName)
        {
            PersAccGroup systemGroup = new PersAccGroup
            {
                Name = groupName,
                IsSystem = YesNo.Yes
            };
            this.PersAccGroupDomainService.Save(systemGroup);

            return this.PersAccGroupDomainService.GetAll().FirstOrDefault(x => x.Name == groupName)?.Id ?? 0;
        }

        /// <summary>
        /// Состоят ли ЛС в системной группе
        /// </summary>
        /// <remarks>Если хоть один не состоит возвращает false</remarks>
        /// <param name="accountsId">Идентификаторы ЛС</param>
        /// <param name="systemGroupId">Идентификатор системной группы</param>
        /// <returns>Результат</returns>
        private bool IsContainsPersonalAccountsInSystemGroups(long[] accountsId, long systemGroupId)
        {
            List<long> personalAccGroupsIds = new List<long>();

            var queryPersonalAccGroupsIds = this.PersAccGroupRelationDomainService.GetAll()
                .Where(x => x.Group.Id == systemGroupId)
                .WhereContains(x => x.PersonalAccount.Id, accountsId)
                .Select(x => new { x.PersonalAccount.Id });

            foreach (var item in queryPersonalAccGroupsIds)
            {
                personalAccGroupsIds.Add(item.Id);
            }

            return !accountsId.Except(personalAccGroupsIds).Any();
        }
    }
}
