namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhRf.Enums;

    /// <summary>
    /// Вьюха на Заявку на перечисление средств
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Заявок на перечисление средств
     * с агрегированными показателями из реестра Перечисление ден средств средств рег. фонда:
     * количество объектов, 
     * итого сумма
     */
    public class ViewRequestTransferRf : PersistentObject
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ManagingOrganizationName { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Тип программы заявки перечисления рег.фонда
        /// </summary>
        public virtual TypeProgramRequest TypeProgramRequest { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual int DocumentNum { get; set; }

        /// <summary>
        /// Дата от
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Количество объектов
        /// </summary>
        public virtual int? TransferFundsCount { get; set; }

        /// <summary>
        /// Итого сумма
        /// </summary>
        public virtual decimal? TransferFundsSum { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        public virtual long? ContragentId { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }
    }
}