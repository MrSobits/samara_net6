namespace Bars.GkhCr.Migration.Version_2015031600
{
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
	using System.Data;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2015031500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        Database.AddEntityTable("CR_OBJ_PROTOCOL_TW",
				new RefColumn("PROTOCOL_ID", "CR_OBJ_PROTOCOL_TW_P", "CR_OBJ_PROTOCOL", "ID"),
				new RefColumn("TYPE_WORK_ID", "CR_OBJ_PROTOCOL_TW_T", "CR_OBJ_TYPE_WORK", "ID"),
				new Column("EXTERNAL_ID", DbType.String, 36));
        }

        public override void Down()
        {
			Database.RemoveTable("CR_OBJ_PROTOCOL_TW");
        }
    }
}