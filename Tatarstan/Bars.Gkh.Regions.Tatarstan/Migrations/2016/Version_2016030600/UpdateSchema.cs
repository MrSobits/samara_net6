namespace Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using System;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016030600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Regions.Tatarstan.Migrations._2016.Version_2016030501.UpdateSchema))]

    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_CONSTRUCT_OBJ_CONTRACT",
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_CONSTRUCT_OBJ_CONTRACT_OBJ", "GKH_CONSTRUCTION_OBJECT", "ID"),
                new RefColumn("STATE_ID", "GKH_CONSTRUCT_OBJ_CONTRACT_S", "B4_STATE", "ID"),
                new Column("TYPE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("NAME", DbType.String,ColumnProperty.NotNull, 500),
                new Column("DATE", DbType.Date),
                new Column("NUMBER", DbType.Int32),
                new RefColumn("FILE_ID", ColumnProperty.Null, "GKH_CONSTRUCT_OBJ_CONTRACT_OBJ_F", "B4_FILE_INFO", "ID"),
                new Column("SUM", DbType.Decimal.WithSize(10, 2), ColumnProperty.Null),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_END", DbType.DateTime, ColumnProperty.Null),
                new RefColumn("CONTRAGENT_ID", "GKH_CONSTRUCT_OBJ_CONTRACT_C", "GKH_CONTRAGENT", "ID"),
                new Column("DATE_START_WORK", DbType.DateTime, ColumnProperty.Null),
                new Column("DATE_END_WORK", DbType.DateTime, ColumnProperty.Null));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_CONSTRUCT_OBJ_CONTRACT");
        }
    }
}
