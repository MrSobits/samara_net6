namespace Bars.Gkh.RegOperator.ExecutionAction
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.IoC;
    using Microsoft.Extensions.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Действие Обновление реестра должников
    /// </summary>
    [Repeatable]
    public class CreateDebtorsAction : BaseExecutionAction
    {
        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "Обновление реестра должников";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Обновление реестра должников с последующим обновлением реестров ПИР";

        private BaseDataResult Execute()
        {
            var updateClaimWorks = this.ExecutionParams.Params.GetAs<bool>("UpdateClaimWorks");

            var debtorService = this.Container.Resolve<IDebtorService>();

            using (this.Container.Using(debtorService))
            {
                try
                {
                    var param = new DynamicDictionary();
                    param.SetValue("updateClaimWorks", updateClaimWorks);
                    param.SetValue("createDocuments", updateClaimWorks);             

                    var documentTypes = this.GetConfigs();
                    param.SetValue("documentTypes", documentTypes);

                    debtorService.Create(new BaseParams {Params = param});
                }
                catch (ValidationException exception)
                {
                    this.Container.Resolve<ILogger>()
                        .LogError(exception, "Ошибка при постановке задачи на обновление реестра должников");
                }
            }

            return new BaseDataResult();
        }

        private List<Tuple<ClaimWorkDocumentType, DebtorType>> GetConfigs()
        {
            var configProv = this.Container.Resolve<IGkhConfigProvider>();

            using (this.Container.Using(configProv))
            {
                var documentTypes = new List<Tuple<ClaimWorkDocumentType, DebtorType>>();

                var individualConfig = configProv.Get<DebtorClaimWorkConfig>().Individual;
                var legalConfig = configProv.Get<DebtorClaimWorkConfig>().Legal;

                if (individualConfig.DebtNotification.NotifFormationKind == DocumentFormationType.AutoForm)
                {
                    documentTypes.Add(new Tuple<ClaimWorkDocumentType, DebtorType>(ClaimWorkDocumentType.Notification, DebtorType.Individual));
                }

                if (individualConfig.Pretension.PretensionFormationKind == DocumentFormationType.AutoForm)
                {
                    documentTypes.Add(new Tuple<ClaimWorkDocumentType, DebtorType>(ClaimWorkDocumentType.Pretension, DebtorType.Individual));
                }

                if (legalConfig.DebtNotification.NotifFormationKind == DocumentFormationType.AutoForm)
                {
                    documentTypes.Add(new Tuple<ClaimWorkDocumentType, DebtorType>(ClaimWorkDocumentType.Notification, DebtorType.Legal));
                }

                if (legalConfig.Pretension.PretensionFormationKind == DocumentFormationType.AutoForm)
                {
                    documentTypes.Add(new Tuple<ClaimWorkDocumentType, DebtorType>(ClaimWorkDocumentType.Pretension, DebtorType.Legal));
                }

                return documentTypes;
            }
        }
    }
}