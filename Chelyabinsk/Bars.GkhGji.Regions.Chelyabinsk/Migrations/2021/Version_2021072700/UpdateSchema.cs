namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021072700
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021072700")]
    [MigrationDependsOn(typeof(Version_2021053100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_COURT_PRACTICE", new RefColumn("ADMONITION_ID", "GJI_CH_COURT_PRACTICE_ADM_ID", "GJI_APPCIT_ADMONITION", "ID"));
            Database.AddColumn("GJI_CH_COURT_PRACTICE", new Column("CM_RESULT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_COURT_PRACTICE", "ADMONITION_ID");
        }
    }
}