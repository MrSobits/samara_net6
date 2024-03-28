namespace Bars.Gkh.Overhaul.Migration.Version_2014080400
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014080400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014073000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //if (!Database.TableExists("REAL_EST_TYPE_COMM_PARAM"))
            //{
            //    Database.AddEntityTable("REAL_EST_TYPE_COMM_PARAM",
            //        new Column("MIN", DbType.String, 500, ColumnProperty.NotNull),
            //        new Column("MAX", DbType.String, 500, ColumnProperty.NotNull),
            //        new Column("COMMON_PARAM_CODE", DbType.String, 500, ColumnProperty.Null),
            //        new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "COMM_PAR_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"));
            //}

            //if (!Database.TableExists("REAL_EST_TYPE_STRUCT_EL"))
            //{
            //    Database.AddEntityTable("REAL_EST_TYPE_STRUCT_EL",
            //        new RefColumn("REAL_EST_TYPE_ID", ColumnProperty.NotNull, "STR_EL_REAL_EST_TYPE", "OVRHL_REAL_ESTATE_TYPE", "ID"),
            //        new RefColumn("STRUCT_EL_ID", ColumnProperty.NotNull, "EST_TYPE_STRUCT_EL", "OVRHL_STRUCT_EL", "ID"));
            //}

            //if (!Database.ColumnExists("REAL_EST_TYPE_STRUCT_EL", "IS_EXISTS"))
            //{
            //    Database.AddColumn("REAL_EST_TYPE_STRUCT_EL", new Column("IS_EXISTS", DbType.Boolean, true));
            //}
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("REAL_EST_TYPE_COMM_PARAM");
            //Database.RemoveEntityTable("REAL_EST_TYPE_STRUCT_EL");
        }
    }
}