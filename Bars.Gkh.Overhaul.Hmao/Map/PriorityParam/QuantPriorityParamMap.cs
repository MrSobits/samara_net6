namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Маппинг для "Количественный параметр очередности"
    /// </summary>
    public class QuantPriorityParamMap : BaseImportableEntityMap<QuantPriorityParam>
    {
        public QuantPriorityParamMap()
            : base("Количественный параметр очередности", "OVRHL_PRIOR_PARAM_QUANT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Code, "Code").Column("CODE").Length(100);
            this.Property(x => x.MaxValue, "MaxValue").Column("MAX_VALUE").Length(100);
            this.Property(x => x.MinValue, "MinValue").Column("MIN_VALUE").Length(100);
            this.Property(x => x.Point, "Point").Column("POINT");
        }
    }
}