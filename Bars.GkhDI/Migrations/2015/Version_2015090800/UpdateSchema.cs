namespace Bars.GkhDi.Migrations.Version_2015090800
{
    using System;
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015090800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015090700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CFMAR_MANAGMENT", DbType.Decimal));
            Database.AddColumn("DI_DISINFO_REALOBJ", new Column("CFMAR_ALL", DbType.Decimal));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CFMAR_MANAGMENT");
            Database.RemoveColumn("DI_DISINFO_REALOBJ", "CFMAR_ALL");
        }
    }
}