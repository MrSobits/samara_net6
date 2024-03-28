namespace Bars.GkhCr.Migration.Version_2015031200
{
	using global::Bars.B4.Modules.Ecm7.Framework;
	using Gkh;
	using System.Data;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015031200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migration.Version_2015022600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
			Database.AddColumn("CR_OBJ_ESTIMATE_CALC", new Column("ESTIMATION_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 0));

			ViewManager.Drop(Database, "GkhCr", "DeleteViewCrObjEstCalcDet");
			ViewManager.Create(Database, "GkhCr", "CreateViewCrObjEstCalcDet");
        }

        public override void Down()
        {
			Database.RemoveColumn("CR_OBJ_ESTIMATE_CALC", "ESTIMATION_TYPE");
        }
    }
}