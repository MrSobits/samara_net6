namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016082500
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016082500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016072900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
	    public override void Up()
	    {
	        if (!this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "EXTENS_TIME"))
	        {
	            this.Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("EXTENS_TIME", DbType.DateTime));
	        }
	    }

	    public override void Down()
        {
            if (this.Database.ColumnExists("GJI_APPEAL_CITIZENS", "EXTENS_TIME"))
            {
                this.Database.RemoveColumn("GJI_APPEAL_CITIZENS", "EXTENS_TIME");
            }
        }
    }
}