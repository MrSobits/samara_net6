namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils.Caching;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с группами ЛС
    /// </summary>
    public class PersonalAccountGroupService : IPersonalAccountGroupService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для работы с группами
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        /// <summary>
        /// Домен-сервис для работы с группами
        /// </summary>
        public IDomainService<PersAccGroup> PersAccGroupDomain { get; set; } 

        /// <summary>
        /// Домен-сервис для работы со связями ЛС с группами
        /// </summary>
        public IDomainService<PersAccGroupRelation> PersAccGroupRelationDomain { get; set; }

        /// <summary>
        /// Список групп, в которых состоит указанный ЛС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns>Список ЛС</returns>
        public DataResult.ListDataResult<PersAccGroup> ListGroupsByAccount(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var persAccId = baseParams.Params.GetAs<long>("accId");

            var data = this.PersAccGroupRelationDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == persAccId)
                .Select(x => x.Group)
                .Filter(loadParams, this.Container);

            var count = data.Count();

            return new DataResult.ListDataResult<PersAccGroup>(data.Order(loadParams).Paging(loadParams).ToList(), count);
        }

        /// <summary>
        /// Добавить лицевой счёт в группу
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        /// <returns></returns>
        public IDataResult AddPersonalAccountToGroups(BaseParams baseParams)
        {
            var personalAccountId = baseParams.Params.GetAs<long>("accId");
            var groupIds = baseParams.Params.GetAs("groupIds", string.Empty).ToLongArray();

            var personalAccount = this.BasePersonalAccountDomain.Get(personalAccountId);

            if (personalAccount.IsNull())
            {
                return BaseDataResult.Error("Не найден указанный лицевой счёт");
            }

            return this.AddPersonalAccountToGroups(personalAccount, groupIds);
        }

        /// <summary>
        /// Добавить лицевой счёт в группу
        /// </summary>
        /// <param name="account">Лицевой счёт. включаемый в группу</param>
        /// <param name="groupIds">Идентификатор группы, в которую включается лицевой счёт</param>
        /// <returns></returns>
        public IDataResult AddPersonalAccountToGroups(BasePersonalAccount account, long[] groupIds)
        {
            var groups = this.PersAccGroupDomain.GetAll().Where(x => groupIds.Contains(x.Id));

            if (!groups.Any())
            {
                return BaseDataResult.Error("Не удалось получить группы лицевых счетов");
            }

            var paExistsInGroup = this.PersAccGroupRelationDomain.GetAll()
                .Where(x => x.PersonalAccount.Id == account.Id && groups.Any(y => y.Id == x.Group.Id))
                .Select(x => x.Group)
                .ToList();

            var listToAdd = new List<PersAccGroupRelation>();
            foreach (var group in groups)
            {
                if (!paExistsInGroup.Contains(group))
                {
                    listToAdd.Add(new PersAccGroupRelation
                    {
                        PersonalAccount = account,
                        Group = group
                    });
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, listToAdd);
            this.InvalidateCache();

            return new BaseDataResult();
        }

        /// <summary>
        /// Удалить лицевой счёт из групп
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        /// <returns></returns>
        public IDataResult RemovePersonalAccountFromGroups(BaseParams baseParams)
        {
            var personalAccountId = baseParams.Params.GetAs<long>("accId");
            var groupIds = baseParams.Params.Get("groupIds", string.Empty).ToLongArray();

            var personalAccount = this.BasePersonalAccountDomain.Get(personalAccountId);

            if (personalAccount.IsNull())
            {
                return BaseDataResult.Error("Не найден указанный лицевой счёт");
            }

            return this.RemovePersonalAccountFromGroups(personalAccount, groupIds);
        }

        /// <summary>
        /// Удалить лицевой счёт из групп
        /// </summary>
        /// <param name="account">Лицевой счёт. удаляемый из групп</param>
        /// <param name="groupIds">Идентификаторы групп, из которых исключается лицевой счёт лицевой счёт</param>
        /// <returns></returns>
        public IDataResult RemovePersonalAccountFromGroups(BasePersonalAccount account, long[] groupIds)
        {

            var personalAccGroups = this.PersAccGroupRelationDomain.GetAll().Where(x => x.PersonalAccount.Id == account.Id && groupIds.Contains(x.Group.Id)).ToList();

            if (personalAccGroups.IsNotEmpty())
            {
                NhExtentions.InTransaction(this.Container, () => personalAccGroups.ForEach(x => this.PersAccGroupRelationDomain.Delete(x.Id)));
            }

            this.InvalidateCache();
            return new BaseDataResult();
        }

        /// <summary>
        /// Массовое исключение ЛС из групп
        /// </summary>
        /// <param name="accounts">Лицевые счета</param>
        /// <param name="groupIds">Группы</param>
        /// <returns>Результат операции</returns>
        public IDataResult RemovePersonalAccountsFromGroups(IQueryable<PersonalAccountDto> accounts, long[] groupIds)
        {
            var personalAccGroups = this.PersAccGroupRelationDomain.GetAll()
                .Where(x => groupIds.Contains(x.Group.Id))
                .Where(x => accounts.Any(y => y.Id == x.PersonalAccount.Id))
                .ToList();

            if (personalAccGroups.IsNotEmpty())
            {
                NhExtentions.InTransaction(this.Container, () => personalAccGroups.ForEach(x => this.PersAccGroupRelationDomain.Delete(x.Id)));
            }

            this.InvalidateCache();
            return new BaseDataResult();
        }

        /// <summary>
        /// Массовое включени ЛС в группы
        /// </summary>
        /// <param name="accounts">Аккаунты</param>
        /// <param name="groupIds">Идентификаторы групп</param>
        /// <returns>Результат операции</returns>
        public IDataResult AddPersonalAccountsToGroups(IQueryable<PersonalAccountDto> accounts, long[] groupIds)
        {
            var groups = this.PersAccGroupDomain.GetAll().Where(x => groupIds.Contains(x.Id));

            if (!groups.Any())
            {
                return BaseDataResult.Error("Не удалось получить группы лицевых счетов");
            }

            var paExistsInGroup = this.PersAccGroupRelationDomain.GetAll()
                .Where(x => groups.Any(y => y.Id == x.Group.Id))
                .Where(x => accounts.Any(y => y.Id == x.PersonalAccount.Id))
                .GroupBy(x => x.Group.Id)
                .ToDictionary(x => x.Key, x => x.Select(y => y.PersonalAccount.Id).ToList());

            var listToAdd = new List<PersAccGroupRelation>();
            var accountsList = accounts.Select(x => x.Id).ToList();
            foreach (var group in groups)
            {
                var groupRelations = paExistsInGroup.Get(group.Id);

                foreach (var account in accountsList)
                {
                    if (groupRelations.IsEmpty() || !groupRelations.Contains(account))
                    {
                        listToAdd.Add(new PersAccGroupRelation
                        {
                            PersonalAccount = this.BasePersonalAccountDomain.Load(account),
                            Group = group
                        });
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, listToAdd);
            this.InvalidateCache();
            return new BaseDataResult();
        }

        private void InvalidateCache()
        {
            CountCacheHelper.InvalidateCache<BasePersonalAccount>(this.Container);
        }
    }
}
