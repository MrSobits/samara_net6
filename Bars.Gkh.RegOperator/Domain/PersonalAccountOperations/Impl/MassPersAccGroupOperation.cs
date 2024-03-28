namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    using Gkh.Domain;
    /// <summary>
    /// Операция массового включения/исключения из групп ЛС
    /// </summary>
    public class MassPersAccGroupOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Сервис для работы с группами ЛС
        /// </summary>
        public IPersonalAccountGroupService PersonalAccountGroupService { get; set; }

        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> AccountDomain { get; set; }

        /// <summary>
        /// Сервис фильтрации
        /// </summary>
        public IPersonalAccountFilterService AccountFilterService { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Ключ
        /// </summary>
        public static string Key => "MassPersAccGroupOperation";

        /// <summary>
        /// Код
        /// </summary>
        public override string Code => MassPersAccGroupOperation.Key;

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name => "Группы лицевых счетов";

        /// <summary>
        /// Права доступа
        /// </summary>
        public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.MassPersAccGroupOperation.View";

        /// <summary>
        /// Выполнить операцию
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        /// <returns></returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            var accIds = baseParams.Params.GetAs<string>("accIds").ToLongArray();
            var groupIds = baseParams.Params.GetAs<string>("workGroupIds").ToLongArray();
            var isIncludeOperation = baseParams.Params.GetAs("isIncludeOperation", true, true);

            var accountQuery = this.AccountDomain.GetAll().ToDto();         

            accountQuery = accIds.IsNotEmpty() 
                ? accountQuery.Where(x => accIds.Contains(x.Id)) 
                : accountQuery
                    .FilterByBaseParams(baseParams, this.AccountFilterService)
                    .Filter(baseParams.GetLoadParam(), this.Container);

            IDataResult result = null;

            this.Container.InTransaction(
                () =>
                {
                    result = isIncludeOperation
                        ? this.PersonalAccountGroupService.AddPersonalAccountsToGroups(accountQuery, groupIds)
                        : this.PersonalAccountGroupService.RemovePersonalAccountsFromGroups(accountQuery, groupIds);
                });

            return result;
        }
    }
}
