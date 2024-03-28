namespace Bars.Gkh.Migrations.Version_2013072600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013072600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013072503.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.RemoveColumn("GKH_MORG_CONTR_REL", "PARENT_TYPE_CONTRACT");
            Database.RemoveColumn("GKH_MORG_CONTR_REL", "CHILDREN_TYPE_CONTRACT");

            Database.AddColumn("GKH_MORG_CONTR_REL", new Column("TYPE_CONTRACT_RELATION", DbType.Int32, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_MORG_CONTR_REL", "TYPE_CONTRACT_RELATION");

            Database.AddColumn("GKH_MORG_CONTR_REL", new Column("PARENT_TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
            Database.AddColumn("GKH_MORG_CONTR_REL", new Column("CHILDREN_TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10));
        }
    }
}