namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Overhaul.Entities;
    using System;

    /// <summary>
    /// Версиоинирование первого этапа
    /// </summary>
    public class VersionRecordStage1 : BaseImportableEntity
    {
        public virtual VersionRecordStage2 Stage2Version { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual RealityObjectStructuralElement StructuralElement { get; set; }

        public virtual int Year { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual decimal SumService { get; set; }

        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Состояние записи
        /// </summary>
        public virtual VersionRecordState VersionRecordState { get; set; }

        /// <summary>
        /// Дата последней смены состояния
        /// </summary>
        public virtual DateTime StateChangeDate { get; set; }
    }
}