namespace Bars.Gkh.Migrations.Version_2013072400
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            // Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "IS_TRANSFER_MANAGEMENT");
            // Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "MAN_ORG_TRANSFERRED_MANAG_ID");

            Database.AddEntityTable(
              "GKH_MORG_CONTR_REL",
               new RefColumn("PARENT_CONTRACT_ID", "GKH_MORG_CREL_PAR", "GKH_MORG_CONTRACT", "ID"),
               new RefColumn("CHILDREN_CONTRACT_ID", "GKH_MORG_CREL_CHILD", "GKH_MORG_CONTRACT", "ID"),
               new Column("PARENT_TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
               new Column("CHILDREN_TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MORG_CONTR_REL");
        }
    }
}