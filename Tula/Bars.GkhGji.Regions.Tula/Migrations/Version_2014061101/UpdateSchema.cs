namespace Bars.GkhGji.Regions.Tula.Migrations.Version_2014061101
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tula.Migrations.Version_2014061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_RESOL_PROS_DEFINITION",
                new RefColumn("RESOL_PROS_ID", ColumnProperty.NotNull,  "GJI_RPROS_DEF_RPROS", "GJI_RESOLPROS", "ID"),
                new RefColumn("ISSUED_DEFINITION_ID", "GJI_RPROS_DEF_ISD", "GKH_DICT_INSPECTOR", "ID"),
                new Column("EXECUTION_DATE", DbType.DateTime),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUM", DbType.String, 50),
                new Column("DESCRIPTION", DbType.String, 500),
                new Column("TYPE_DEFINITION", DbType.Int32, 4, ColumnProperty.NotNull, 10));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_RESOL_PROS_DEFINITION");
        }
    }
}