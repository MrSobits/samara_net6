namespace Bars.Gkh.Migrations._2017.Version_2017030601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017030200
    /// </summary>
    [Migration("2017030601")]
    [MigrationDependsOn(typeof(Version_2017030200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GKH_PERSON_CERTIFICATE", "IS_FROM_ANOTHER_REGION", DbType.Boolean);
            this.Database.AddColumn("GKH_PERSON_CERTIFICATE", "REGION_CODE", DbType.String, 2);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "IS_FROM_ANOTHER_REGION");
            this.Database.RemoveColumn("GKH_PERSON_CERTIFICATE", "REGION_CODE");
        }
    }
}