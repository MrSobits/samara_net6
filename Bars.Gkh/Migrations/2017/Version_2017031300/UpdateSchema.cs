namespace Bars.Gkh.Migrations._2017.Version_2017031300
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Gkh.2017031300
    /// </summary>
    [Migration("2017031300")]
    [MigrationDependsOn(typeof(Version_2017022000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.RemoveColumn("GKH_TECHNICAL_MISTAKE", "PERSON_ID");
            this.Database.AddRefColumn("GKH_TECHNICAL_MISTAKE", new RefColumn("INSPECTOR_ID", "GKH_DICT_INSPECTOR_ID", "GKH_DICT_INSPECTOR", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_TECHNICAL_MISTAKE", "INSPECTOR_ID");         
        }
    }
}

