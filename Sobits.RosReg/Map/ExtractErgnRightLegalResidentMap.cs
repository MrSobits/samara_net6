namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.ExtractEgrnRightLegalResident"</summary>
    public class ExtractEgrnRightLegalResidentMap : JoinedSubClassMap<ExtractEgrnRightLegalResident>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnRightLegalResidentMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnRightLegalResidentMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrnRightLegalResident";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Ogrn, "Огрн").Column(nameof(ExtractEgrnRightLegalResident.Ogrn).ToLower());
            
        }
    }
    public class ExtractEgrnRightLegalResidentNhMapping : JoinedSubclassMapping<ExtractEgrnRightLegalResident>
    {
        public ExtractEgrnRightLegalResidentNhMapping()
        {
            this.Schema(ExtractEgrnRightLegalResidentMap.SchemaName);
        }
   }
}