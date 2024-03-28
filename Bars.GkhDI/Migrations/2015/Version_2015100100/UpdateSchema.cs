namespace Bars.GkhDi.Migrations.Version_2015100100
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015091500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("DI_DICT_PERIODICITY", "NAME", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE DI_DICT_PERIODICITY SET NAME = '' WHERE NAME is NULL");
            this.Database.AlterColumnSetNullable("DI_DICT_PERIODICITY", "NAME", false);
        }
    }
}
