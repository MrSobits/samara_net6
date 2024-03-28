namespace Bars.GkhGji.Migrations._2015.Version_2015121400
{
	using System.Data;
	using B4.Modules.Ecm7.Framework;
	using B4.Modules.NH.Migrations.DatabaseExtensions;

	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015111300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
	        if (!this.Database.TableExists("GJI_DICT_LEGAL_REASON"))
	        {
		        this.Database.AddEntityTable("GJI_DICT_LEGAL_REASON",
			        new Column("NAME", DbType.String, 2000),
			        new Column("CODE", DbType.String, 300),
			        new Column("EXTERNAL_ID", DbType.String, 36));
		        this.Database.AddIndex("IND_GJI_LEGAL_REASON_NAME", false, "GJI_DICT_LEGAL_REASON", "NAME");
		        this.Database.AddIndex("IND_GJI_LEGAL_REASON_CODE", false, "GJI_DICT_LEGAL_REASON", "CODE");
	        }

	        if (!this.Database.TableExists("GJI_DICT_TYPESURV_LEGREAS"))
	        {
		        this.Database.AddEntityTable("GJI_DICT_TYPESURV_LEGREAS",
			        new RefColumn("TYPESURVEY_ID", ColumnProperty.NotNull, "TYPESURV_LEGREAS_TYPESURV", "GJI_DICT_TYPESURVEY", "ID"),
			        new RefColumn("LEGAL_REASON_ID", ColumnProperty.NotNull, "TYPESURV_LEGREAS_LEGREAS", "GJI_DICT_LEGAL_REASON", "ID"));
	        }

	        if (!this.Database.TableExists("GJI_DICT_TYPESURV_CONTRTYPE"))
	        {
		        this.Database.AddEntityTable("GJI_DICT_TYPESURV_CONTRTYPE",
			        new RefColumn("TYPESURVEY_ID", ColumnProperty.NotNull, "LEG_REAS_TYPE_SURV", "GJI_DICT_TYPESURVEY", "ID"),
			        new Column("TYPE_JUR_PERSON", DbType.Int32, 4, ColumnProperty.NotNull));
	        }
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_TYPESURV_CONTRTYPE");
            this.Database.RemoveTable("GJI_DICT_TYPESURV_LEGREAS");
            this.Database.RemoveTable("GJI_DICT_LEGAL_REASON");
        }
    }
}