namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019100700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Migration = Bars.B4.Modules.Ecm7.Framework.Migration;

    [Migration("2019100700")]
    [MigrationDependsOn(typeof(Version_2019072600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddTable(
                "GJI_TAT_DISPOSAL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("REGISTRATION_NUMBER_ERP", DbType.String),
                new Column("ERP_ID", DbType.String),
                new Column("REGISTRATION_DATE_ERP", DbType.DateTime),
                new Column("COUNT_DAYS", DbType.Int32),
                new Column("COUNT_HOURS", DbType.Int32),
                new Column("REASON_ERP_CHECKING", DbType.Int32),
                new RefColumn("PROSECUTOR_ID", "GJI_TAT_DISPOSAL_PROSECUTOR", "GJI_DICT_PROSECUTOR_OFFICE", "ID"));

            this.Database.AddForeignKey("FK_GJI_TAT_DISPOSAL_ID", "GJI_TAT_DISPOSAL", "ID", "GJI_DISPOSAL", "ID");

            this.Database.ExecuteNonQuery(@"insert into GJI_TAT_DISPOSAL (id) select id from GJI_DISPOSAL");
        }

        public override void Down()
        {
            this.Database.RemoveConstraint("GJI_TAT_DISPOSAL", "FK_GJI_TAT_DISPOSAL_ID");
            this.Database.RemoveTable("GJI_TAT_DISPOSAL");
        }
    }
}