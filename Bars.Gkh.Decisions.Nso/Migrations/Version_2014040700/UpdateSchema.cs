namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014040700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014040700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014040100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //gkh.migration_2014021200
            if (!Database.ColumnExists("GKH_OBJ_D_PROTOCOL", "AUTHORIZED_PERSON"))
            {
                Database.AddColumn("GKH_OBJ_D_PROTOCOL", new Column("AUTHORIZED_PERSON", DbType.String, 200, ColumnProperty.Null));
            }

            //gkh.migration_2014030601
            if (!Database.ColumnExists("GKH_OBJ_D_PROTOCOL", "STATE_ID"))
            {
                Database.AddRefColumn("GKH_OBJ_D_PROTOCOL", new RefColumn("STATE_ID", "GKH_OBJ_D_PROT_STATE", "B4_STATE", "ID"));
            }

            //gkh.migration_2014020604
            if (!Database.ColumnExists("GKH_GENERIC_DECISION", "JSON_OBJECT"))
            {
                Database.AddColumn("GKH_GENERIC_DECISION", new Column("JSON_OBJECT", DbType.String, 20000, ColumnProperty.Null));
            }

            //gkh.migration_2014020604
            if (!Database.ColumnExists("GKH_GENERIC_DECISION", "FILE_ID"))
            {
                Database.AddRefColumn("GKH_GENERIC_DECISION", new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_GEN_DEC_FILE", "B4_FILE_INFO", "ID"));
            }
        }

        public override void Down()
        {
            //пусто, т.к. удаление будет в миграции 1
        }
    }
}