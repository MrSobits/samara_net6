namespace Bars.GkhDi.Migrations.Version_2013041900
{
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2013041200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // перенакатываем, т.к. AddTable  накатывал RefColumn как Column
            Database.AddIndex("IND_DI_PERC_DINFO_ID", true, "DI_PERC_DINFO", "ID");
            Database.AddForeignKey("FK_DI_PERC_DINFO_ID", "DI_PERC_DINFO", "ID", "DI_PERC_CALC", "ID");

            Database.AddIndex("IND_DI_PERC_CALC_DI", false, "DI_PERC_DINFO", "DIS_INFO_ID");
            Database.AddForeignKey("FK_DI_PERC_CALC_DI", "DI_PERC_DINFO", "DIS_INFO_ID", "DI_DISINFO", "ID");

            Database.AddIndex("IND_DI_PERC_REAL_OBJ_ID", true, "DI_PERC_REAL_OBJ", "ID");
            Database.AddForeignKey("FK_DI_PERC_REAL_OBJ_ID", "DI_PERC_REAL_OBJ", "ID", "DI_PERC_CALC", "ID");

            Database.AddIndex("IND_DI_PERC_CALC_RO", false, "DI_PERC_REAL_OBJ", "REAL_OBJ_ID");
            Database.AddForeignKey("FK_DI_PERC_CALC_RO", "DI_PERC_REAL_OBJ", "REAL_OBJ_ID", "DI_DISINFO_REALOBJ", "ID");

            Database.AddIndex("IND_DI_PERC_SERVICE_ID", true, "DI_PERC_SERVICE", "ID");
            Database.AddForeignKey("FK_DI_PERC_SERVICE_ID", "DI_PERC_SERVICE", "ID", "DI_PERC_CALC", "ID");

            Database.AddIndex("IND_DI_PERC_CALC_SERV", false, "DI_PERC_SERVICE", "SERVICE_ID");
            Database.AddForeignKey("FK_DI_PERC_CALC_SERV", "DI_PERC_SERVICE", "SERVICE_ID", "DI_BASE_SERVICE", "ID");

            Database.AddIndex("IND_DI_ARCH_PERC_DI_ID", true, "DI_ARCH_PERC_DINFO", "ID");
            Database.AddForeignKey("FK_DI_ARCH_PERC_DI_ID", "DI_ARCH_PERC_DINFO", "ID", "DI_ARCH_PERC_CALC", "ID");

            Database.AddIndex("IND_DI_ARCH_PERC_DI", false, "DI_ARCH_PERC_DINFO", "DIS_INFO_ID");
            Database.AddForeignKey("FK_DI_ARCH_PERC_DI", "DI_ARCH_PERC_DINFO", "DIS_INFO_ID", "DI_DISINFO", "ID");

            Database.AddIndex("IND_DI_ARCH_PERC_RO_ID", true, "DI_ARCH_PERC_REAL_OBJ", "ID");
            Database.AddForeignKey("FK_DI_ARCH_PERC_RO_ID", "DI_ARCH_PERC_REAL_OBJ", "ID", "DI_ARCH_PERC_CALC", "ID");

            Database.AddIndex("IND_DI_ARCH_PERC_RO", false, "DI_ARCH_PERC_REAL_OBJ", "REAL_OBJ_ID");
            Database.AddForeignKey("FK_DI_ARCH_PERC_RO", "DI_ARCH_PERC_REAL_OBJ", "REAL_OBJ_ID", "DI_DISINFO_REALOBJ", "ID");
        }

        public override void Down()
        {
            // не нужно
        }
    }
}