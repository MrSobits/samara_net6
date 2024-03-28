namespace Bars.GkhGji.Migrations._2015.Version_2015102300
{
	using System.Data;
	using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

	/// <summary>
	/// Миграция 2015102300
	/// </summary>
	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015101300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
		/// <summary>
		/// Накатить
		/// </summary>
        public override void Up()
        {
	        Database.AddEntityTable(
		        "GJI_DICT_LEGFOUND_INSPECTCHECK",
		        new Column("CODE", DbType.String, 100),
		        new RefColumn("NORM_DOC_ID", ColumnProperty.Null, "TYPE_SUR_LEGFCHECK_NDOC", "GKH_DICT_NORMATIVE_DOC", "ID"),
		        new RefColumn("TYPE_SURVEY_GJI_ID", "GJI_LEGF_INSCHECK_TS", "GJI_DICT_TYPESURVEY", "ID"));
        }

		/// <summary>
		/// Откатить
		/// </summary>
        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_LEGFOUND_INSPECTCHECK");
        }
    }
}