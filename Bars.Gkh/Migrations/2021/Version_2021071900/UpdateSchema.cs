namespace Bars.Gkh.Migrations._2021.Version_2021071900
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2021071900")]
    
    [MigrationDependsOn(typeof(Version_2021061700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", new RefColumn("FILE_INFO_ID", "FILE_INFO_ID_RO", "B4_FILE_INFO", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "FILE_INFO_ID");
        }
    }
}