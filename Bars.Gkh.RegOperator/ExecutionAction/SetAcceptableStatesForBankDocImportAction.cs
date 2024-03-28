namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.Dto;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Utils;

    /// <summary>
    /// РегОператор - Проставить статусы ЛС для подтиверждения оплат
    /// </summary>
    public class SetAcceptableStatesForBankDocImportAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Code => nameof(SetAcceptableStatesForBankDocImportAction);

        /// <inheritdoc />
        public override string Name => "РегОператор - Проставить статусы ЛС для подтиверждения оплат";

        /// <inheritdoc />
        public override string Description => "Действие проставляет настройки для статусов по подтверждению оплат";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var stateRepo = this.Container.Resolve<IStateRepository>();
            var configProvider = this.Container.Resolve<IGkhConfigProvider>();

            using (this.Container.Using(stateRepo, configProvider))
            {
                this.Container.GetGkhConfig<RegOperatorConfig>()
                    .GeneralConfig
                    .BankDocumentImportAccountStates = stateRepo.GetAllStates<BasePersonalAccount>()
                        .Select(
                            x => new StateDto
                            {
                                Id = x.Id,
                                Name = x.Name,
                                Code = x.Code
                            })
                        .ToList();

                configProvider.SaveChanges();
            }

            return new BaseDataResult();
        }
    }
}