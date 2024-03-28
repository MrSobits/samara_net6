namespace Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014073100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014071801.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        if (!this.Database.TableExists("GJI_DICT_LEGAL_REASON"))
	        {
		        Database.AddEntityTable("GJI_DICT_LEGAL_REASON",
			        new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
			        new Column("CODE", DbType.String, 300),
			        new Column("EXTERNAL_ID", DbType.String, 36));
		        Database.AddIndex("IND_GJI_LEGAL_REASON_NAME", false, "GJI_DICT_LEGAL_REASON", "NAME");
		        Database.AddIndex("IND_GJI_LEGAL_REASON_CODE", false, "GJI_DICT_LEGAL_REASON", "CODE");
	        }

	        if (!this.Database.TableExists("GJI_DICT_TYPESURV_LEGREAS"))
	        {
		        Database.AddEntityTable("GJI_DICT_TYPESURV_LEGREAS",
			        new RefColumn("TYPESURVEY_ID", ColumnProperty.NotNull, "TYPESURV_LEGREAS_TYPESURV", "GJI_DICT_TYPESURVEY", "ID"),
			        new RefColumn("LEGAL_REASON_ID", ColumnProperty.NotNull, "TYPESURV_LEGREAS_LEGREAS", "GJI_DICT_LEGAL_REASON", "ID"));
	        }

	        if (!this.Database.TableExists("GJI_DICT_TYPESURV_CONTRTYPE"))
	        {
		        Database.AddEntityTable("GJI_DICT_TYPESURV_CONTRTYPE",
			        new RefColumn("TYPESURVEY_ID", ColumnProperty.NotNull, "LEG_REAS_TYPE_SURV", "GJI_DICT_TYPESURVEY", "ID"),
			        new Column("TYPE_JUR_PERSON", DbType.Int32, 4, ColumnProperty.NotNull));
	        }
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_DICT_TYPESURV_CONTRTYPE");
            Database.RemoveTable("GJI_DICT_TYPESURV_LEGREAS");
            Database.RemoveTable("GJI_DICT_LEGAL_REASON");
        }
    }
}