namespace Bars.GkhGji.Regions.Nso.Migrations.Version_2014121604
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121604")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nso.Migrations.Version_2014121603.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //-----НСО Предписание
            Database.AddTable(
                "GJI_NSO_PRESCRIPTION",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DOCUMENT_PLACE", DbType.String, 1000),
                new Column("DOCUMENT_TIME", DbType.DateTime, 25));
            Database.AddForeignKey("FK_GJI_NSO_PRESCRIPRION_D", "GJI_NSO_PRESCRIPTION", "ID", "GJI_PRESCRIPTION", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_NSO_PRESCRIPTION (id)
                                     select id from GJI_PRESCRIPTION");

            Database.AddEntityTable(
                "GJI_PRESCR_ACTIV_DIRECT",
                new RefColumn("ACTIVEDIRECT_ID", ColumnProperty.NotNull, "GJI_PRESCR_ACTIV_DIRECT_A", "GJI_ACTIVITY_DIRECTION", "ID"),
                new RefColumn("PRESCRIPTION_ID", ColumnProperty.NotNull, "GJI_PRESCR_ACTIV_DIRECT_P", "GJI_PRESCRIPTION", "ID"));

            Database.AddEntityTable(
                "GJI_PRESCR_BASE_DOC",
                new RefColumn("KIND_BASE_DOC_ID", ColumnProperty.NotNull, "GJI_PRESCR_BASE_DOC_D", "GJI_KIND_BASE_DOC", "ID"),
                new RefColumn("PRESCRIPTION_ID", ColumnProperty.NotNull, "GJI_PRESCR_BASE_DOC_P", "GJI_PRESCRIPTION", "ID"));

        }

        public override void Down()
        {
            Database.RemoveTable("GJI_PRESCR_BASE_DOC");
            Database.RemoveTable("GJI_PRESCR_ACTIV_DIRECT");

            Database.RemoveConstraint("GJI_NSO_PRESCRIPTION", "FK_GJI_NSO_PRESCRIPRION_D");
            Database.RemoveTable("GJI_NSO_PRESCRIPTION");
        }
    }
}