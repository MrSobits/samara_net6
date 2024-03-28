namespace Bars.GkhRf.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Вьюха на Договор рег. фонда
    /// </summary>
    /*
     * Данная вьюха предназначена для реестра Договоров рег. фонда
     * с агрегированными показателями из реестра Объектов договора рег. фонда:
     * количество объектов
     */
    public class ViewContractRf : PersistentObject
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
        /// Управляющая организация(id)
        /// </summary>
        public virtual long ManagingOrganizationId { get; set; }

        /// <summary>
        /// Идентификатор контрагента
        /// </summary>
        public virtual long? ContragentId { get; set; }

        /// <summary>
        /// Идентификатор муниципального образования
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// Cумма общих площадей из жилых домов вкладки "Дома, включенные в договор"
        /// </summary>
        public virtual decimal? SumAreaMkd { get; set; }

        /// <summary>
        /// Cумма площадей в собственности граждан из жилых домов вкладки "Дома, включенные в договор"
        /// </summary>
        public virtual decimal? SumAreaLivingOwned { get; set; }
    }
}