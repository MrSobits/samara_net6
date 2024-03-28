namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014021100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014013000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_ACTVISUAL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("RO_ID", "GJI_ACTVIS_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("CONCLUSION", DbType.String, 2000),
                new Column("INSPECTION_RESULT", DbType.String, 2000),
                new Column("FLAT", DbType.String, 10),
                new Column("HOUR", DbType.Int32),
                new Column("MINUTE", DbType.Int32));

            Database.AddForeignKey("FK_GJI_ACTVIS_DOC", "GJI_ACTVISUAL", "ID", "GJI_DOCUMENT", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_ACTVISUAL", "FK_GJI_ACTVIS_DOC");
            Database.RemoveIndex("IND_GJI_ACTVIS_ID", "GJI_ACTVISUAL");

            Database.RemoveTable("GJI_ACTVISUAL");
        }
    }
}
