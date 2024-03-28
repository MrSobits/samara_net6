namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ConfigSections
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ConfigSections;

    /// <summary>
    /// Сервис "Настройки правил проставления вида проверок"
    /// </summary>
    public class GjiValidityDocPeriodService : IGjiValidityDocPeriodService
    {
        public IDomainService<GjiValidityDocPeriod> GjiValidityDocPeriodDomain { get; set; }

        /// <inheritdoc />
        public IDataResult DocPeriodValidation(DateTime? checkingDate, TypeDocumentGji typeDocumentGji)
        {
            if (!this.GjiValidityDocPeriodDomain.GetAll().Any(x => x.TypeDocument == typeDocumentGji))
            {
                return new BaseDataResult(false, $"Для документа с типом {typeDocumentGji.GetDisplayName()} не указаны периоды действия");
            }

            if (!checkingDate.HasValue ||
                !this.GjiValidityDocPeriodDomain.GetAll()
                    .Any(x =>
                        x.TypeDocument == typeDocumentGji &&
                        x.StartDate <= checkingDate &&
                        (!x.EndDate.HasValue || x.EndDate.Value >= checkingDate)))
            {
                return new BaseDataResult(false, "Указанная дата находится за рамками периода действия документа");
            }

            return new BaseDataResult();
        }
    }
}