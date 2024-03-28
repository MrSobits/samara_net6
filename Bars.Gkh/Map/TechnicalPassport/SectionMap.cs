namespace Bars.Gkh.Map.TechnicalPassport
{
    using Bars.Gkh.Entities.TechnicalPassport;

    public class SectionMap : GkhBaseEntityMap<Section>
    {
        public SectionMap()
            : base("TP_SECTION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Название").Column("NAME");
            this.Property(x => x.Title, "Заголовок").Column("TITLE");
            this.Property(x => x.Code, "Код").Column("CODE");
            this.Property(x => x.Order, "Порядковый номер").Column("ORDER");
            this.Reference(x => x.Parent, "Родительская секция").Column("PARENT_ID");
        }
    }
}