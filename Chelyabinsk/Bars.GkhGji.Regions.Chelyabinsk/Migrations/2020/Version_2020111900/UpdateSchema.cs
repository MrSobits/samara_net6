namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2020111900
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2020111900")]
    [MigrationDependsOn(typeof(Version_2020110900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_EGRN", new Column("QUALITY_PHONE", DbType.String, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_EGRN", "QUALITY_PHONE");
        }
    }
}