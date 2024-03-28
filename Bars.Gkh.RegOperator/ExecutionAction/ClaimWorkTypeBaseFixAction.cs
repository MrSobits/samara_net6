namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Extensions;
    using Bars.Gkh.Domain;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Entities;

    /// <summary>
    /// Действие Проставление корректных типов оснований претензионно-исковой работы
    /// </summary>
    public class ClaimWorkTypeBaseFixAction : BaseExecutionAction
    {
        private readonly IDomainService<BaseClaimWork> baseClaimWorkDomainService;
        private readonly IDomainService<BuildContractClaimWork> buildContractClaimWorkDomainService;
        private readonly IDomainService<DebtorClaimWork> debtorClaimWorkDomainService;

        /// <summary>
        /// Конструктор действия
        /// </summary>
        /// <param name="baseClaimWorkDomainService">Домен-сервис для Основание претензионно исковой работы</param>
        /// <param name="buildContractClaimWorkDomainService">Домен-сервис для Основание претензионно исковой работы для Договоров Подряда</param>
        /// <param name="debtorClaimWorkDomainService">Домен-сервис для Основание претензионно исковой работы для неплательщиков</param>
        public ClaimWorkTypeBaseFixAction(
            IDomainService<BaseClaimWork> baseClaimWorkDomainService,
            IDomainService<BuildContractClaimWork> buildContractClaimWorkDomainService,
            IDomainService<DebtorClaimWork> debtorClaimWorkDomainService)
        {
            this.baseClaimWorkDomainService = baseClaimWorkDomainService;
            this.buildContractClaimWorkDomainService = buildContractClaimWorkDomainService;
            this.debtorClaimWorkDomainService = debtorClaimWorkDomainService;
        }

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Проставление корректных типов оснований претензионно-исковой работы";

        /// <summary>
        /// Название действия
        /// </summary>
        public override string Name => "Проставление корректных типов оснований претензионно-исковой работы";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var baseClaimWorks = this.baseClaimWorkDomainService.GetAll().ToList();
            var buildContractClaimWorks = this.buildContractClaimWorkDomainService.GetAll().Select(x => x.Id).ToHashSet();
            var debtorClaimWorks = this.debtorClaimWorkDomainService.GetAll().Select(x => x.Id).ToHashSet();

            foreach (var baseClaimWork in baseClaimWorks)
            {
                if (debtorClaimWorks.Contains(baseClaimWork.Id))
                {
                    baseClaimWork.ClaimWorkTypeBase = ClaimWorkTypeBase.Debtor;
                }
                else
                {
                    if (buildContractClaimWorks.Contains(baseClaimWork.Id))
                    {
                        baseClaimWork.ClaimWorkTypeBase = ClaimWorkTypeBase.BuildContract;
                    }
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, baseClaimWorks, 10000, true, true);

            return new BaseDataResult
            {
                Success = true
            };
        }
    }
}