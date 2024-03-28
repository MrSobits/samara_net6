namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractEgrnRight"</summary>
    public class ExtractEgrnRightMap : PersistentObjectMap<ExtractEgrnRight>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnRightMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnRightMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrnRight";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Type, "Тип собственности").Column(nameof(ExtractEgrnRight.Type).ToLower());
            this.Property(x => x.Number, "Номер права").Column(nameof(ExtractEgrnRight.Number).ToLower());
            this.Property(x => x.Share, "Доля собственности").Column(nameof(ExtractEgrnRight.Share).ToLower());

            this.Reference(x => x.EgrnId, "Выписка").Column(nameof(ExtractEgrnRight.EgrnId).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class ExtractEgrnRightNhMapping : ClassMapping<ExtractEgrnRight>
    {
        public ExtractEgrnRightNhMapping()
        {
            this.Schema(ExtractEgrnRightMap.SchemaName);
        }
    }
}