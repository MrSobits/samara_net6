namespace Bars.Gkh.Gis.Migrations._2014.Version_2014120100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2014120100")][MigrationDependsOn(typeof(global::Bars.Gkh.Gis.Migrations._2014.Version_2014112800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GIS_SUPPLIER_REGISTER",
                new Column("ID", DbType.Int64, (ColumnProperty.NotNull | ColumnProperty.Unique)),
                new Column("NAME", DbType.String, 200, ColumnProperty.NotNull),
                new Column("INN", DbType.Int64));

            this.Database.AddRefColumn("GIS_LOADED_FILE_REGISTER", new RefColumn("SUPPLIER", "GIS_LD_FL_RGSTR_SPPL", "GIS_SUPPLIER_REGISTER", "ID"));
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "SUPPLIER_NAME");
        }

        public override void Down()
        {
            this.Database.RemoveTable("GIS_SUPPLIER_REGISTER");
            this.Database.RemoveColumn("GIS_LOADED_FILE_REGISTER", "SUPPLIER");
            this.Database.AddColumn("GIS_LOADED_FILE_REGISTER", "SUPPLIER_NAME", DbType.String, ColumnProperty.NotNull);
        }
    }
}