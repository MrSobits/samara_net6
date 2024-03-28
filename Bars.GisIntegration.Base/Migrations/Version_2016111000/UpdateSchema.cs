namespace Bars.GisIntegration.Base.Migrations.Version_2016111000
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GisIntegration.Base.Extensions;

    [Migration("2016111000")]
    [MigrationDependsOn(typeof(Version_2016110901.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRisEntityTable(
                "INSURANCE_PRODUCT",
                new RefColumn("ATTACHMENT_ID", "RIS_ATTACHMENT_ID", "GI_ATTACHMENT", "ID"),
                new Column("NAME", DbType.String),
                new Column("DECRIPTION", DbType.String),
                new Column("ATTACHMENT_HASH", DbType.String),
                new Column("INSURANCE_ORGANIZATION", DbType.String),
                new Column("CLOSE_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            this.Database.RemoveTable("INSURANCE_PRODUCT");
        }
    }
}