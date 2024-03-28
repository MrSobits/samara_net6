namespace Bars.Gkh.RegOperator.Domain.PersonalAccountOperations.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.RegOperator.Domain.Extensions;
    using Bars.Gkh.RegOperator.Domain.Repository;
    using Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Castle.Windsor;

    /// <summary>
    /// Операция запрет перерасчета
    /// </summary>
    public class BanRecalcOperation : PersonalAccountOperationBase
    {
        /// <summary>
        /// Провайдер сессий NHibernate
        /// </summary>
        public ISessionProvider SessionProvider { get; set; }

        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис лицевых счетов
        /// </summary>
        public IDomainService<BasePersonalAccount> PersonalAccountDomain { get; set; }

        /// <summary>
        /// FileManager
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Репозиторий периода начислений
        /// </summary>
        public IChargePeriodRepository ChargePeriodRepository { get; set; }

        /// <summary>
        /// Интерфейс создания запрета перерасчета
        /// </summary>
        public IPersonalAccountBanRecalcManager PersonalAccountBanRecalcManager { get; set; }

        /// <summary>
        /// Key
        /// </summary>
        public static string Key => "BanRecalcOperation";

        /// <summary>
        /// Code
        /// </summary>
        public override string Code => BanRecalcOperation.Key;

        /// <summary>
        /// Name
        /// </summary>
        public override string Name => "Запрет перерасчета";

        /// <summary>
		/// Права доступа
		/// </summary>
		public override string PermissionKey => "GkhRegOp.PersonalAccount.Registry.Action.BanRecalcOperation";

        /// <summary>
        /// Execute
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public override IDataResult Execute(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var dateStart = baseParams.Params.GetAs<DateTime>("DateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("DateEnd");
            var file = baseParams.Files.Get("File");
            var reason = baseParams.Params.GetAs<string>("Reason");
            var accountIds = baseParams.Params.GetAs<long[]>("persAccIds");
            var isCharge = baseParams.Params.GetAs<bool>("IsCharge") ? BanRecalcType.Charge : 0;
            var isPenalty = baseParams.Params.GetAs<bool>("IsPenalty") ? BanRecalcType.Penalty : 0;
            var type = isCharge | isPenalty;

            return this.Container.InTransactionWithResult(
                () =>
                {
                    if (!Enum.IsDefined(typeof(BanRecalcType), type))
                    {
                        return BaseDataResult.Error("Необходимо выбрать хотя бы один тип операции");
                    }

                    var persAccQuery = this.PersonalAccountDomain.GetAll().ToDto();

                    persAccQuery = accountIds.IsNotEmpty()
                        ? persAccQuery.Where(x => accountIds.Contains(x.Id))
                        : persAccQuery
                            .Filter(loadParam, this.Container)
                            .FilterByBaseParams(baseParams, this.Container);

                    var fileInfo = file != null ? this.FileManager.SaveFile(file) : null;

                    foreach (var accId in persAccQuery.Select(x => x.Id).ToList())
                    {
                        this.PersonalAccountBanRecalcManager.CreateBanRecalc(
                            new BasePersonalAccount {Id = accId},
                            dateStart,
                            dateEnd,
                            type,
                            fileInfo,
                            reason);
                    }

                    this.PersonalAccountBanRecalcManager.SaveBanRecalcs();

                    return new BaseDataResult();
                });
        }

        /// <summary>
        /// Вернуть данные для UI
        /// </summary>
        public override IDataResult GetDataForUI(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var persAccIds = baseParams.Params.GetAs<long[]>("persAccIds");

            var persAccQuery = this.PersonalAccountDomain.GetAll().ToDto();

            persAccQuery = persAccIds.IsNotEmpty()
                ? persAccQuery.Where(x => persAccIds.Contains(x.Id))
                : persAccQuery
                    .Filter(loadParam, this.Container)
                    .FilterByBaseParams(baseParams, this.Container);

            return new ListDataResult(persAccQuery.Paging(loadParam).ToList(), persAccQuery.Count());
        }
    }
}