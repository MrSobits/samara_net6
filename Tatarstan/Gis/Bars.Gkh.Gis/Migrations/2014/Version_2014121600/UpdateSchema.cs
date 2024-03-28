namespace Bars.Gkh.Gis.Migrations._2014.Version_2014121600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014121600")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014121201.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_UIC_PERSONAL_ACCOUNT", new[]
            {
                new Column("UIC", DbType.String, ColumnProperty.NotNull),
                new Column("PERSONAL_ACC_ID", DbType.Int64, ColumnProperty.NotNull),
                new Column("ACCOUNT_NUMBER", DbType.Int64, ColumnProperty.NotNull),
                new Column("FLAT_NUMBER", DbType.String, ColumnProperty.NotNull),

                new RefColumn("HOUSE_GIS_ID", "GIS_UICPERSACC_HREG_ID", "GIS_HOUSE_REGISTER", "ID"),
            });
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_UIC_PERSONAL_ACCOUNT");
        }
    }
}