namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100800
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014100800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014100701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_ACTSURVEY_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DATE_NOTIFICATION", DbType.DateTime),
                new Column("NUMBER_NOTIFICATION", DbType.String, 100));

            Database.AddForeignKey("FK_GJI_ACTSURVEY_SMOL_ID", "GJI_ACTSURVEY_SMOL", "ID", "GJI_ACTSURVEY", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_ACTSURVEY_SMOL (id)
                                     select id from GJI_ACTSURVEY");

            Database.AddColumn("GJI_ACTCHECK_SMOL", new Column("DATE_NOTIFICATION", DbType.DateTime));
            Database.AddColumn("GJI_ACTCHECK_SMOL", new Column("NUMBER_NOTIFICATION", DbType.String, 100));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_ACTSURVEY_SMOL");

            Database.RemoveColumn("GJI_ACTCHECK_SMOL", "DATE_NOTIFICATION");
            Database.RemoveColumn("GJI_ACTCHECK_SMOL", "NUMBER_NOTIFICATION");
        }
    }
}