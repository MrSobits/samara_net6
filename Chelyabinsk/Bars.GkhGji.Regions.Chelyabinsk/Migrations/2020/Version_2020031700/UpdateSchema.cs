namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_202031700
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2020031700")]
    [MigrationDependsOn(typeof(Version_2020021600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
        public override void Up()
        {
             Database.AddColumn("GJI_CH_GIS_ERP", new Column("ERPID", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_GIS_ERP", "ERPID");         
        }
    }
}