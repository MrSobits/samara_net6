namespace Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014071800
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014071800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Khakasia.Migrations.Version_2014071700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable(
                "KHAKASIA_GJI_ACTSURVEY",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_OF", DbType.DateTime),
                new Column("TIME_END", DbType.DateTime),
                new Column("TIME_START", DbType.DateTime),
                new Column("CON_ISSUED", DbType.Int16, ColumnProperty.NotNull, 20));

            Database.AddForeignKey("FK_KHAKASIA_GJI_ACTSURVEY", "KHAKASIA_GJI_ACTSURVEY", "ID", "GJI_ACTSURVEY", "ID");

            Database.ExecuteNonQuery(@"insert into KHAKASIA_GJI_ACTSURVEY (id)
                                     select id from GJI_ACTSURVEY");

            Database.AddEntityTable(
                "GJI_ACTSURVEY_LONGDESC",
                new RefColumn("ACT_SURVEY_ID", ColumnProperty.NotNull, "GJI_ACTSURVEY_LONGDESC_A", "GJI_ACTSURVEY", "ID"),
                new Column("DESCRIPTION", DbType.Binary, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.RemoveTable("KHAKASIA_GJI_ACTSURVEY");
            Database.RemoveTable("GJI_ACTSURVEY_LONGDESC");
        }
    }
}