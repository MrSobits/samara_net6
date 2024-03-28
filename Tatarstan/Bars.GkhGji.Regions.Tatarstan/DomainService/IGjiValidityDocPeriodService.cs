namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;

    using Bars.B4;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Интерфейс сервиса "Настройки правил проставления вида проверок"
    /// </summary>
    public interface IGjiValidityDocPeriodService
    {
        /// <summary>
        /// Проверка вхождения даты в период действия документа
        /// </summary>
        IDataResult DocPeriodValidation(DateTime? checkingDate, TypeDocumentGji typeDocumentGji);
    }
}