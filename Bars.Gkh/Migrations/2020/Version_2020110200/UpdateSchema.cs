namespace Bars.Gkh.Migrations._2020.Version_2020110200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2020110200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2020102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CS_FORMULA",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("FORMULA", DbType.String, 250),
                new Column("FORMULA_PARAMS", DbType.Binary, ColumnProperty.Null));

            Database.AddColumn("GKH_CS_CALCULATION", new RefColumn("FORMULA_ID", "GKH_CS_CALCULATION_FORMULA", "GKH_CS_FORMULA", "ID"));

        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_CS_CALCULATION", "FORMULA_ID");
            Database.RemoveTable("GKH_CS_FORMULA");
         
        }
    }
}