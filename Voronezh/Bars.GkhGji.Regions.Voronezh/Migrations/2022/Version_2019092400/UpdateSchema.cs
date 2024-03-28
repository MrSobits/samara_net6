namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2019092400
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019092400")]
    [MigrationDependsOn(typeof(Version_2019092300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
             Database.AddColumn("GJI_VR_COURT_PRACTICE_FILE", new Column("DOC_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_VR_COURT_PRACTICE_FILE", "DOC_DATE");         
        }
    }
}