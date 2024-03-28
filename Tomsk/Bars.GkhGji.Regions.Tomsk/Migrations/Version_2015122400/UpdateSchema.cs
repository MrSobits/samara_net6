namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015122400
{
    using System.Data;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015122400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2015121600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.RemoveTable("GJI_TOMSK_PROVDOC_DATE");
            Database.AddEntityTable(
                "GJI_TOMSK_PROVDOC_NUM",
                new Column("NUM_TIME_DOCUMENT", DbType.Int64),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "PROVIDE_DOC_DATE", "GJI_DISPOSAL", "ID"));


        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_TOMSK_PROVDOC_NUM");
            Database.AddEntityTable(
                "GJI_TOMSK_PROVDOC_DATE",
                new Column("DATE_TIME_DOCUMENT", DbType.DateTime),
                new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "PROVIDE_DOC_DATE", "GJI_DISPOSAL", "ID"));
        }
    }
}
