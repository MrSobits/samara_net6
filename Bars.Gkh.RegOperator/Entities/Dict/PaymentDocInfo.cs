namespace Bars.Gkh.RegOperator.Entities.Dict
{
    using System;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Enums;

    /// <summary>
    /// Справочник "Информационное поле в документе оплаты Физ.лица"
    /// </summary>
    public class PaymentDocInfo : BaseImportableEntity
    {
        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality MoSettlement { get; set; }

        /// <summary>
        /// AOGUID Fias населенного пункта
        /// </summary>
        public virtual string LocalityAoGuid { get; set; }

        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        public virtual string LocalityName { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Способ формирования фонда
        /// </summary>
        public virtual FundFormationType FundFormationType { get; set; }

        /// <summary>
        /// Дата начала
        /// </summary>
        public virtual DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Информация
        /// </summary>
        public virtual string Information { get; set; }

        /// <summary>
        /// Для всего региона
        /// </summary>
        public virtual bool IsForRegion { get; set; }
    }
}