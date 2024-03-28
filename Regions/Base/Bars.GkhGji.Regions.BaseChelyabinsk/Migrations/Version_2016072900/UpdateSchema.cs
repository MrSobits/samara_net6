namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016072900
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016072900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2016062300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
	    public override void Up()
	    {
	        this.Database.AddTable("CHELYABINSK_GJI_REMINDER",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new RefColumn("APPEAL_CITS_EXECUTANT_ID", "GUARANTOR_GKH_GJI_APPCIT_EXECUTANT", "GJI_APPCIT_EXECUTANT", "ID"));

	        this.Database.ExecuteNonQuery(@"insert into CHELYABINSK_GJI_REMINDER (id) select id from GJI_REMINDER");
        }

	    public override void Down()
        {
	        this.Database.RemoveTable("CHELYABINSK_GJI_REMINDER");
        }
    }
}