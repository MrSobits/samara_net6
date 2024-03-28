namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2015102300
{
	using global::Bars.B4.Modules.Ecm7.Framework;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

	/// <summary>
	/// Миграция 2015102300
	/// </summary>
	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2015092900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		/// <summary>
		/// Накатить
		/// </summary>
		public override void Up()
		{
			Database.AddEntityTable("GJI_NSO_DISPOSAL_INSPFOUNDCHECK",
				new RefColumn("DISPOSAL_ID", "GJI_NSO_DISP_IFOUND_DC", "GJI_DISPOSAL", "ID"),
				new RefColumn("INSPFOUND_ID", "GJI_NSO_DISP_IFOUND_FC", "GKH_DICT_NORMATIVE_DOC", "ID"));
		}

		/// <summary>
		/// Откатить
		/// </summary>
		public override void Down()
		{
			Database.RemoveTable("GJI_NSO_DISPOSAL_INSPFOUNDCHECK");
		}
    }
}