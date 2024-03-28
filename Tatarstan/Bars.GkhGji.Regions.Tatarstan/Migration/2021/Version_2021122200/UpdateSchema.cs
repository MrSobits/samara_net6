namespace Bars.GkhGji.Regions.Tatarstan.Migration._2021.Version_2021122200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2021122200")]
    [MigrationDependsOn(typeof(Version_2021122000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GJI_WARNING_INSPECTION", new Column("APPEAL_CITS_ID", DbType.Int64));
            this.Database.AddForeignKey("FK_GJI_WARNING_INSPECTION_APPEAL_CITS", "GJI_WARNING_INSPECTION", "APPEAL_CITS_ID", "GJI_APPEAL_CITIZENS", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_WARNING_INSPECTION", "APPEAL_CITS_ID");
        }
    }
}
