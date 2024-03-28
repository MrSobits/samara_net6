namespace Bars.Gkh.Migration.Version_2015080700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015081000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015080500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddPersistentObjectTable("GKH_FIAS_HOUSE",
                new Column("POSTALCODE", DbType.String, 6),
                new Column("OKATO", DbType.String, 11),
                new Column("OKTMO", DbType.String, 15),
                new Column("BUILDNUM", DbType.String, 5),
                new Column("STRUCNUM", DbType.String, 5),
                new Column("HOUSENUM", DbType.String, 10),
                new Column("HOUSEID", DbType.String, 50),
                new Column("HOUSEGUID", DbType.String, 50),
                new Column("AOGUID", DbType.String, 50));

            Database.AddColumn("GKH_REALITY_OBJECT", "HOUSEGUID", DbType.String, 50);
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "HOUSEGUID");

            Database.RemoveTable("GKH_FIAS_HOUSE");
        }
    }
}