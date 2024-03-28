namespace Bars.GkhDi.Migrations.Version_2016021000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016021000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2015120700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        this.Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("PATRONYMIC", DbType.String, 300, ColumnProperty.Null));
	        this.Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("FACILS_COMMENT", DbType.String, 300, ColumnProperty.Null));
	        this.Database.AddColumn("DI_DISINFO_COM_FACILS", new Column("SIGNING_CONTRACT_DATE", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
	        this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "PATRONYMIC");
	        this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "FACILS_COMMENT");
	        this.Database.RemoveColumn("DI_DISINFO_COM_FACILS", "SIGNING_CONTRACT_DATE");
        }
    }
}
