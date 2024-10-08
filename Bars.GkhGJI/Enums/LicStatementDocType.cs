﻿namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Прилагаемые к заявке на внесение изменений в реестр лицензий документы
    /// </summary>
    public enum LicStatementDocType
    {
        /// <summary>
        /// Заявка на внесение изменений в реестр лицензий
        /// </summary>
        [Display("Заявка на внесение изменений в реестр лицензий")]
        LicStatement = 0,

        /// <summary>
        /// копия протокола и решения общего собрания собственников
        /// </summary>
        [Display("Копия протокола и решения общего собрания собственников")]
        ProtocolOwner = 10,

        /// <summary>
        /// Копия протокола конкурса по отбору управляющей организации
        /// </summary>
        [Display("Копия протокола конкурса по отбору управляющей организации")]
        ProtocolOMSU = 20,

        /// <summary>
        /// Копия договора управления
        /// </summary>
        [Display("Копия договора управления")]
        ManagingContract = 30,

        /// <summary>
        /// Копия акта приема-передачи технической документации и иных документов
        /// </summary>
        [Display("Копия акта приема-передачи технической документации и иных документов")]
        AcceptanceCertificate = 40,

        /// <summary>
        /// Документ, подтверждающий полномочия представителя
        /// </summary>
        [Display("Документ, подтверждающий полномочия представителя")]
        DocumentRepresentative = 50,

        /// <summary>
        /// Копия заявления одной из сторон договора в связи с окончанием срока его действия
        /// </summary>
        [Display("Копия заявления одной из сторон договора в связи с окончанием срока его действия")]
        ContractEndStatement = 60
    }
}