namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021020900
{
    using B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2021020900")]
    [MigrationDependsOn(typeof(Version_2020111900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_FNS_LIC", new Column("DES_TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 1));
            UpdateQuery();
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_FNS_LIC", "DES_TYPE");
        }

        private void UpdateQuery()
        {
            this.Database.ExecuteQuery("UPDATE GJI_CH_SMEV_FNS_LIC SET DES_TYPE = 1");
        }
    }
}