namespace Bars.Gkh.Migrations._2015.Version_2015100400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT", new Column("FAX", DbType.String, 100, ColumnProperty.Null));

            Database.AddRefColumn("GKH_MANAGING_ORGANIZATION", new RefColumn("FILE_STATUTE_ID", "GKH_MANAGING_ORG_FS", "B4_FILE_INFO", "ID"));
            Database.AddRefColumn("GKH_MANAGING_ORGANIZATION", new RefColumn("FIAS_DISPATCH_ID", "GKH_MANAGING_ORG_DW", "B4_FIAS_ADDRESS", "ID"));
            Database.AddColumn("GKH_MANAGING_ORGANIZATION", new Column("IS_DISPATCH_CORR_FACT", DbType.Boolean, ColumnProperty.NotNull, false));
            Database.AddColumn("GKH_MANAGING_ORGANIZATION", new Column("DISPATCH_PHONE", DbType.String, 300, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT", "FAX");

            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "FILE_STATUTE_ID");
            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "FIAS_DISPATCH_ID");
            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "IS_DISPATCH_CORR_FACT");
            Database.RemoveColumn("GKH_MANAGING_ORGANIZATION", "DISPATCH_PHONE");
        }
    }
}