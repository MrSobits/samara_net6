namespace Bars.GkhDi.Migrations.Version_2013041001
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveConstraint("DI_DISINFO_FIN_ACTIVITY", "FK_DI_DISINFO_FA_DI");
            Database.AddForeignKey("FK_DI_DISINFO_FA_DI", "DI_DISINFO_FIN_ACTIVITY", "DISINFO_ID", "DI_DISINFO", "ID");

            Database.ChangeColumn("DI_DISINFO_DOCUMENTS", new Column("DESCR_PROJ_CONTR", DbType.String, 2000));
            Database.ChangeColumn("DI_DICT_TEMPL_SERVICE", new Column("CODE", DbType.String, 300));
            Database.ChangeColumn("DI_DICT_PERIODICITY", new Column("CODE", DbType.String, 300));
            Database.ChangeColumn("DI_DICT_SUPERVISORY_ORG", new Column("CODE", DbType.String, 300));
            Database.ChangeColumn("DI_DICT_TAX_SYSTEM", new Column("SHORT_NAME", DbType.String, 250));

            Database.ChangeColumn("DI_DISINFO_FIN_ACTIVITY", new Column("DESCRIPTION", DbType.String, 2000));

            if (!Database.ConstraintExists("DI_BASE_SERVICE", "FK_DI_BASEEERV_DRO") && !Database.ConstraintExists("DI_BASE_SERVICE", "FK_DI_BASE_SERV_DRO"))
            {
                Database.AddForeignKey("FK_DI_BASE_SERV_DRO", "DI_BASE_SERVICE", "DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID");
            }
        }

        public override void Down()
        {
            if (Database.ConstraintExists("DI_BASE_SERVICE", "FK_DI_BASE_SERV_DRO"))
            {
                Database.RemoveConstraint("DI_BASE_SERVICE", "FK_DI_BASE_SERV_DRO");
            }
        }
    }
}