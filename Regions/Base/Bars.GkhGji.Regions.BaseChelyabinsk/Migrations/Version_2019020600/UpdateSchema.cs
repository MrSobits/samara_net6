namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_20190206000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2019020600")]
    [MigrationDependsOn(typeof(Version_20180417000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
               "GJI_DISP_ADDITIONAL_DOC",
               new Column("DOC_NAME", DbType.String, 300),
               new RefColumn("DISPOSAL_ID", ColumnProperty.NotNull, "FK_GJI_DISP_ADDIT_DOC", "GJI_DISPOSAL", "ID"));


        }
        public override void Down()
        {
            this.Database.RemoveTable("GJI_DISP_ADDITIONAL_DOC");         

        }
    }
}