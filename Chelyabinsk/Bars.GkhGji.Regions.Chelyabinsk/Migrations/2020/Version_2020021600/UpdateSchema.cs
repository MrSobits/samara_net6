namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020021600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2020021600")]
    [MigrationDependsOn(typeof(Version_2020021400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
             Database.AddColumn("GJI_CH_COURT_PRACTICE_FILE", new Column("DOC_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE_FILE", "DOC_DATE");         
        }
    }
}