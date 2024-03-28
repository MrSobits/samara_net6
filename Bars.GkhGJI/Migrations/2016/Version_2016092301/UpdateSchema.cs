namespace Bars.GkhGji.Migrations._2016.Version_2016092301
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;
    [Migration("2016092301")]
    [MigrationDependsOn(typeof(Version_2016091300.UpdateSchema))]
    public class UpdateSchema : Migration
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