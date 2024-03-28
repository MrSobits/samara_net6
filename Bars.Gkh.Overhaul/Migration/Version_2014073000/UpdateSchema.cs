namespace Bars.Gkh.Overhaul.Migration.Version_2014073000
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014052100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //if (!Database.TableExists("OVRHL_REAL_ESTATE_TYPE"))
            //{
            //    Database.AddEntityTable("OVRHL_REAL_ESTATE_TYPE",
            //        new Column("NAME", DbType.String, 300, ColumnProperty.NotNull));
            //}

            //if (!Database.ColumnExists("OVRHL_REAL_ESTATE_TYPE", "MARG_REPAIR_COST"))
            //{
            //    Database.AddColumn("OVRHL_REAL_ESTATE_TYPE", new Column("MARG_REPAIR_COST", DbType.Decimal));
            //}

            if (!Database.TableExists("REAL_EST_TYPE_MU"))
            {
                Database.AddEntityTable("REAL_EST_TYPE_MU",
                    new RefColumn("RET_ID", ColumnProperty.NotNull, "REAL_EST_TYPE_MU_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"),
                    new RefColumn("MU_ID", ColumnProperty.NotNull, "REAL_EST_TYPE_MU_MU", "GKH_DICT_MUNICIPALITY", "ID"));
            }

            //if (!Database.TableExists("OVRHL_REALESTATEREALITYO"))
            //{
            //    Database.AddEntityTable("OVRHL_REALESTATEREALITYO",
            //        new RefColumn("RO_ID", "REALESTATEREALITYO_RO", "GKH_REALITY_OBJECT", "ID"),
            //        new RefColumn("RET_ID", "REALESTATEREALITYO_RET", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            //}
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_REALESTATEREALITYO");
            Database.RemoveTable("REAL_EST_TYPE_MU");

            //if (Database.TableExists("OVRHL_REAL_ESTATE_TYPE"))
            //{
            //    Database.RemoveEntityTable("OVRHL_REAL_ESTATE_TYPE");
            //}
        }
    }
}