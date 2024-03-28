namespace Bars.Gkh.Migrations.Version_2013072901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_OBJ_BASE_WRK",
                new RefColumn("OBJECT_ID", ColumnProperty.NotNull, "GKH_OBJ_BASE_WRK_OBJ", "GKH_REALITY_OBJECT", "ID"),
                new Column("DATE_LAST_REPAIR", DbType.Date),
                new Column("TYPE_COMPLETE_WORK", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("TYPE_REPAIR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("VOLUME_REPAIR", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DESCRIPTION", DbType.String, 1000));

            Database.AddTable("GKH_OBJ_CONSTR_EL_WRK",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJ_CONSTR_EL_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_CONSTR_EL_WRK_ELM", false, "GKH_OBJ_CONSTR_EL_WRK", "OBJ_CONSTR_EL_ID");
            Database.AddForeignKey("FK_OBJ_CONSTR_EL_WRK_ELM", "GKH_OBJ_CONSTR_EL_WRK", "OBJ_CONSTR_EL_ID", "GKH_OBJ_CONSTRUCT_ELEM", "ID");
            Database.AddForeignKey("FK_OBJ_CONSTR_EL_WRK_BASE", "GKH_OBJ_CONSTR_EL_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");

            Database.AddTable("GKH_OBJ_LIFT_WRK",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("OBJ_LIFT_ID", DbType.Int64, ColumnProperty.NotNull));

            Database.AddIndex("IND_OBJ_LIFT_WRK_ELM", false, "GKH_OBJ_LIFT_WRK", "OBJ_LIFT_ID");
            Database.AddForeignKey("FK_OBJ_LIFT_WRK_LFT", "GKH_OBJ_LIFT_WRK", "OBJ_LIFT_ID", "GKH_OBJ_LIFT", "ID");
            Database.AddForeignKey("FK_OBJ_LIFT_WRK_BASE", "GKH_OBJ_LIFT_WRK", "ID", "GKH_OBJ_BASE_WRK", "ID");
        }

        public override void Down()
        {
            Database.RemoveConstraint("GKH_OBJ_LIFT_WRK", "FK_OBJ_LIFT_WRK_LFT");
            Database.RemoveConstraint("GKH_OBJ_LIFT_WRK", "FK_OBJ_LIFT_WRK_BASE");
            Database.RemoveIndex("IND_OBJ_LIFT_WRK_ELM", "GKH_OBJ_LIFT_WRK");
            Database.RemoveTable("GKH_OBJ_LIFT_WRK");

            Database.RemoveConstraint("GKH_OBJ_CONSTR_EL_WRK", "FK_OBJ_CONSTR_EL_WRK_ELM");
            Database.RemoveConstraint("GKH_OBJ_CONSTR_EL_WRK", "FK_OBJ_CONSTR_EL_WRK_BASE");
            Database.RemoveIndex("IND_OBJ_CONSTR_EL_WRK_ELM", "GKH_OBJ_CONSTR_EL_WRK");
            Database.RemoveTable("GKH_OBJ_CONSTR_EL_WRK");

            Database.RemoveTable("GKH_OBJ_BASE_WRK");
        }
    }
}