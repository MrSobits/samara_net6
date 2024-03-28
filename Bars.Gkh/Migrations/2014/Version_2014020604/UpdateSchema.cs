namespace Bars.Gkh.Migrations.Version_2014020604
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020604")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014020603.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //перенесено в модуль Decisions

            //Database.AddColumn("GKH_GENERIC_DECISION", new Column("JSON_OBJECT", DbType.String, 20000, ColumnProperty.Null));
            //Database.AddRefColumn("GKH_GENERIC_DECISION", new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_GEN_DEC_FILE", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            //Database.RemoveColumn("GKH_GENERIC_DECISION", "JSON_OBJECT");
            //Database.RemoveRefColumn("GKH_GENERIC_DECISION", "FILE_ID");
        }
    }
}