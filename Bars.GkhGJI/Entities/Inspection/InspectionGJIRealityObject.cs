namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Проверяемые дома в инспекционной проверки ГЖИ
    /// </summary>
    public class InspectionGjiRealityObject : BaseGkhEntity, IEntityUsedInErp, IEntityUsedInErknm
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Номера квартир
        /// </summary>
        public virtual string RoomNums { get; set; }

        /// <summary>
        /// Гуид ЕРП
        /// </summary>
        public virtual string ErpGuid { get; set; }

        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}