namespace Bars.Gkh.Migrations.Version_2014121100
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2014120400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (!Database.TableExists("OVRHL_REAL_ESTATE_TYPE"))
            {
                Database.AddEntityTable("OVRHL_REAL_ESTATE_TYPE",
                    new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
            }

            if (!Database.ColumnExists("OVRHL_REAL_ESTATE_TYPE", "MARG_REPAIR_COST"))
            {
                Database.AddColumn("OVRHL_REAL_ESTATE_TYPE", new Column("MARG_REPAIR_COST", DbType.Decimal));
            }

            Database.AddColumn("OVRHL_REAL_ESTATE_TYPE", new Column("CODE", DbType.String, 100));

            Database.AddRefColumn("GKH_REALITY_OBJECT", new RefColumn("REAL_EST_TYPE_ID", "GKH_RO_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "REAL_EST_TYPE_ID");

            if (Database.TableExists("OVRHL_REAL_ESTATE_TYPE"))
            {
                Database.RemoveTable("OVRHL_REAL_ESTATE_TYPE");
            }
        }
    }
}