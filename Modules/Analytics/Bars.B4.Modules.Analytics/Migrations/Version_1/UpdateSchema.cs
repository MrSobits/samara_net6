namespace Bars.B4.Modules.Analytics.Migrations.Version_1
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("1")]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //Database.AddEntityTable("AL_STORED_FILTER",
            //    new Column("NAME", DbType.String, 200),
            //    new Column("DESCRIPTION", DbType.String, 500),
            //    new Column("DATA_FILTER", DbType.Binary),
            //    new Column("PROVIDER_KEY", DbType.String, 200));

            Database.AddEntityTable("AL_DATA_SOURCE",
                new Column("PROVIDER_KEY", DbType.String, 200),
                new Column("NAME", DbType.String, 200),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("SYS_FILTER_BYTES", DbType.Binary),
                new Column("DATA_FILTER_BYTES", DbType.Binary),
                new RefColumn("PARENT", ColumnProperty.Null, "DATA_SOURCE_PARENT", "AL_DATA_SOURCE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("AL_DATA_SOURCE");
            //Database.RemoveEntityTable("AL_STORED_FILTER");
        }
    }
}
