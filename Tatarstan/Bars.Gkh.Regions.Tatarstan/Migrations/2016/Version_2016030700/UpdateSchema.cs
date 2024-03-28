namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016030700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030600.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "GKH_CONSTRUCT_OBJ_MONITORING_SMR",
                new Column("EXTERNAL_ID", DbType.String, 36),
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_CONSTRUCT_OBJ_SMR_OBJ", "GKH_CONSTRUCTION_OBJECT", "ID"),
                new RefColumn("STATE_ID", "GKH_CONSTRUCT_OBJ_SMR_S", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONSTRUCT_OBJ_MONITORING_SMR");
        }
    }
}
