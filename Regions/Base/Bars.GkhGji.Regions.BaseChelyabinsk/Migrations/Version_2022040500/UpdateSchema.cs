namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2022040500
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
    using System.Data;

    [Migration("2022040500")]
    [MigrationDependsOn(typeof(Version_2022031700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_ERUL_LICNUMBER", new Column("REQUEST_TYPE", DbType.Int16, ColumnProperty.NotNull, (int)ERULRequestType.GetLicNumber));
          
        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_ERUL_LICNUMBER", "REQUEST_TYPE");
         

        }

    }
}