namespace Bars.Gkh.Migrations.Version_2013120500
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013120500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013120200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("GKH_OBJ_RESORG", new RefColumn("FILE_INFO_ID", "GKH_RESORG_FILE", "B4_FILE_INFO", "ID"));

            Database.AddColumn("GKH_OBJ_RESORG", new Column("CONTRACT_NUMBER", DbType.String));
            Database.AddColumn("GKH_OBJ_RESORG", new Column("CONTRACT_DATE", DbType.DateTime));
            Database.AddColumn("GKH_OBJ_RESORG", new Column("NOTE", DbType.String, 300));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_OBJ_RESORG", "FILE_INFO_ID");
            Database.RemoveColumn("GKH_OBJ_RESORG", "CONTRACT_NUMBER");
            Database.RemoveColumn("GKH_OBJ_RESORG", "CONTRACT_DATE");
            Database.RemoveColumn("GKH_OBJ_RESORG", "NOTE");
        }
    }
}