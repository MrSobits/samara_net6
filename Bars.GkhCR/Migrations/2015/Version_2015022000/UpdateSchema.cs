namespace Bars.GkhCr.Migrations.Version_2015022000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015022000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015021200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_FIN_SOURCE_RES", new Column("YEAR", DbType.Int32));
            Database.AddRefColumn("CR_OBJ_FIN_SOURCE_RES", new RefColumn("TYPE_WORK_ID", "CR_FIN_SOU_TW_WRK", "CR_OBJ_TYPE_WORK", "ID"));

            if (Database.TableExists("CR_HMAO_OBJ_FIN_SRC_RES"))
            {
                Database.ExecuteNonQuery(
                    "update CR_OBJ_FIN_SOURCE_RES FS set TYPE_WORK_ID = (SELECT TYPE_WORK_ID FROM CR_HMAO_OBJ_FIN_SRC_RES HFS Where HFS.ID = FS.ID)");

                Database.RemoveTable("CR_HMAO_OBJ_FIN_SRC_RES");
            }

            if (Database.TableExists("CR_NSO_OBJ_FIN_SRC_RES"))
            {
                Database.ExecuteNonQuery(
                    "update CR_OBJ_FIN_SOURCE_RES FS set TYPE_WORK_ID = (SELECT TYPE_WORK_ID FROM CR_NSO_OBJ_FIN_SRC_RES NFS Where NFS.ID = FS.ID)");

                Database.RemoveTable("CR_NSO_OBJ_FIN_SRC_RES");
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_FIN_SOURCE_RES", "TYPE_WORK_ID");
            Database.RemoveColumn("CR_OBJ_FIN_SOURCE_RES", "YEAR");
        }
    }
}