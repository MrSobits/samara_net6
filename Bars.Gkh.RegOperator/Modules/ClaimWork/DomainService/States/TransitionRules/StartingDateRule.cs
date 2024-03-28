namespace Bars.Gkh.RegOperator.Modules.ClaimWork.DomainService.States.TransitionRules
{
    using System;

    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.ClaimWork.Debtor;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.DomainService.States;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Проверка включения типа по дате начала отсчета <see cref="BaseClaimWork.StartingDate"/>
    /// </summary>
    public class StartingDateRule : IClwTransitionRule
    {
        private readonly IGkhConfigProvider configProv;

        public StartingDateRule(IGkhConfigProvider configProv)
        {
            this.configProv = configProv;
        }

        #region Implementation of IClwDocumentCreationRule

        /// <summary>
        /// Проверка возможности сформировать документ
        /// </summary>
        /// <param name="docType">Тип документа для формирования</param>
        /// <param name="claimWork">Основание ПИР</param>
        /// <param name="useCache">Использовать кэш для получения документов</param>
        public bool CanCreateDocumentOfType(ClaimWorkDocumentType docType, BaseClaimWork claimWork, bool useCache = false)
        {
            ArgumentChecker.NotNull(claimWork, "claimWork");

            var clw = claimWork as DebtorClaimWork;
            if (clw != null)
            {
                var configSection = this.configProv.Get<DebtorClaimWorkConfig>();

                var config = clw.DebtorType == DebtorType.Legal
                    ? configSection.Legal
                    : configSection.Individual;

                var dateIsLess = ClwUtils.GetDate(docType, config, clw.StartingDate) < DateTime.Now;

                return dateIsLess;
            }

            return true;
        }

        #endregion

    }
}