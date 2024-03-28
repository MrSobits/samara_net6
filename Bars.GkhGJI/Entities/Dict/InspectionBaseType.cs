namespace Bars.GkhGji.Entities.Dict
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Справочник Основание проверки
    /// </summary>
    public class InspectionBaseType : BaseEntity, IUsedInTorIntegration
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вид проверки
        /// </summary>
        public virtual InspectionKind InspectionKindId { get; set; }

        /// <summary>
        /// Значение передается в ЕРП
        /// </summary>
        public virtual bool SendErp { get; set; }

        /// <summary>
        /// Идентификатор для интеграции
        /// </summary>
        public virtual Guid? TorId { get; set; }

        /// <summary>
        /// Код в ЕРКНМ
        /// </summary>
        public virtual string ErknmCode { get; set; }

        /// <summary>
        /// Наличие текстового поля
        /// </summary>
        public virtual bool HasTextField { get; set; }

        /// <summary>
        /// Наличие даты
        /// </summary>
        public virtual bool HasDate { get; set; }

        /// <summary>
        /// Наличие индикатора риска
        /// </summary>
        public virtual bool HasRiskIndicator { get; set; }
    }
}