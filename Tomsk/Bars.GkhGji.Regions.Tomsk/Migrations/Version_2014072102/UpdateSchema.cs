namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072102
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014072102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014072101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_TOMSK_RESOLUTION_DESCR", new Column("PETITION_TEXT", DbType.Binary, ColumnProperty.Null));
            Database.AddColumn("GJI_TOMSK_RESOLUTION_DESCR", new Column("EXPLANATION_TEXT", DbType.Binary, ColumnProperty.Null));

            Database.AddColumn("GJI_TOMSK_RESOLUTION", new Column("HAS_PETITION", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("GJI_TOMSK_RESOLUTION", new Column("PETITION_TEXT", DbType.String, 2000));
            Database.AddColumn("GJI_TOMSK_RESOLUTION", new Column("FIO_ATTEND", DbType.String, 500));
            Database.AddColumn("GJI_TOMSK_RESOLUTION", new Column("EXPLANATION_TEXT", DbType.String, 2000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION_DESCR", "PETITION_TEXT");
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION_DESCR", "EXPLANATION_TEXT");

            Database.RemoveColumn("GJI_TOMSK_RESOLUTION", "HAS_PETITION");
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION", "PETITION_TEXT");
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION", "FIO_ATTEND");
            Database.RemoveColumn("GJI_TOMSK_RESOLUTION", "EXPLANATION_TEXT");
        }
    }
}