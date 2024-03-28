namespace Bars.Gkh.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractBig"</summary>
    public class RosRegExtractBigMap : PersistentObjectMap<RosRegExtractBig>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractBigMap()
            :
                base("Bars.Gkh.Regions.Chelyabinsk.Entities", "RosRegExtractBig")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            //this.Property(x => x.XML, "XML").Column("XML");
            this.Property(x => x.CadastralNumber, "CadastralNumber").Column("CadastralNumber");
            this.Property(x => x.Address, "Address").Column("Address");
            this.Property(x => x.ExtractDate, "ExtractDate").Column("ExtractDate");
            this.Property(x => x.ExtractNumber, "ExtractNumber").Column("ExtractNumber");
            this.Property(x => x.RoomArea, "RoomArea").Column("RoomArea");
        }
    }
    public class RosRegExtractBigNhMaping : ClassMapping<RosRegExtractBig>
    {
        public RosRegExtractBigNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
