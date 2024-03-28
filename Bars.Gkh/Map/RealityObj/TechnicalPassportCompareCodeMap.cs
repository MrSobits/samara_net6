namespace Bars.Gkh.Map.RealityObj
{
    using Bars.Gkh.Entities.RealityObj;

    public class TechnicalPassportCompareCodeMap : GkhBaseEntityMap<TechnicalPassportCompareCode>
    {
        public TechnicalPassportCompareCodeMap()
            : base("TP_COMPARE_CODE")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.FormCode, "Форма").Column("FORM_CODE");
            this.Property(x => x.CellCode, "Код").Column("CELL_CODE");
            this.Property(x => x.Value, "Значение").Column("VALUE");
            this.Property(x => x.CodeMjf, "Код в МЖФ").Column("CODE_MJF");
            this.Property(x => x.CodeRis, "Код в РИС ЖКХ").Column("CODE_RIS");
            this.Property(x => x.CodeReforma, "Код в Реформе").Column("CODE_REFORMA");
        }
    }
}