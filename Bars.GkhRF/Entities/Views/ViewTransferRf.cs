namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Вьюха на Договор рег. фонда
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Перечисления договоров рег. фонда
     * с агрегированными показателями из реестра Договоров рег. фонда:
     * количество объектов
     */
    public class ViewTransferRf : PersistentObject
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual string ManagingOrganizationName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNum { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Количество объектов
        /// </summary>
        public virtual int ContractRfObjectsCount { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual string MunicipalityName { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        public virtual long? ContragentId { get; set; }
    }
}