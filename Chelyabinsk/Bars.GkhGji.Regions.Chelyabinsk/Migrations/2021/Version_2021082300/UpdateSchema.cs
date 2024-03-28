namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2021082300
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2021082300")]
    [MigrationDependsOn(typeof(Version_2021080500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_MVD", new Column("ANSWER_INFO", DbType.String, 5000));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_MVD", "ANSWER_INFO");
        }
    }
}