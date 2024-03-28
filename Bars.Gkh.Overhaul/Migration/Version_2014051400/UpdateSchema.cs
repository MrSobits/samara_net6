using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Overhaul.Migration.Version_2014051400
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014051400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2014043000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_RO_STR_EL_HISTORY", 
                new RefColumn("RO_ID", ColumnProperty.NotNull, "OV_RO_STR_EL_HIS_RO", "GKH_REALITY_OBJECT", "ID"),
                new Column("RO_SE_ID", DbType.Int64, ColumnProperty.NotNull));


            Database.ExecuteNonQuery(@"insert into OVRHL_RO_STR_EL_HISTORY 
                                     select 
                                       id,
                                       0,
                                       object_create_date, 
                                       object_create_date, 
                                       ro_id,
                                       id
                                     from OVRHL_RO_STRUCT_EL");
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_RO_STR_EL_HISTORY");
        }
    }
}