namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System;
    using Bars.B4.DataAccess;
    using Enum;
    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class VersionRecordStage1 : BaseEntity
    {
        public virtual VersionRecordStage2 Stage2Version { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual int Year { get; set; }

        // Данное поле пришлось сделать необязательным поскольку в РТ могут забивать записи вручную ниже добавил дополнительную колонку
        [Obsolete("Это поле используется только при расчете, в дальнейшем работа идет с StrElement")]
        public virtual RealityObjectStructuralElement StructuralElement { get; set; }

        public virtual StructuralElement StrElement { get; set; }

        public virtual decimal Volume { get; set; }

        public virtual decimal Sum { get; set; }

        public virtual decimal SumService { get; set; }

        /// <summary>
        /// тип записи ДПКР
        /// </summary>
        public virtual TypeDpkrRecord TypeDpkrRecord { get; set; }
    }
}