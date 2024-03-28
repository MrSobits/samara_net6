namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014013000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014013000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014012901.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "GJI_TOMSK_TYPESURV_ISSUE",
                new Column("NAME", DbType.String, 300, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, 100),
                new RefColumn("TYPE_SURVEY_GJI_ID", ColumnProperty.NotNull, "TYPESURVEY_ISSUE", "GJI_DICT_TYPESURVEY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_TOMSK_TYPESURV_ISSUE");
        }
    }
}
