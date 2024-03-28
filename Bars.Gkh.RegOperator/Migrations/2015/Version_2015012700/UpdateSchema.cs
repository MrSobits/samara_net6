namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015012700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014122900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        #region Overrides of Migration

        public override void Up()
        {
            Database.AddColumn("REGOP_FS_IMPORT_MAP_ITEM",
                new Column("REQUIRED", DbType.Boolean, ColumnProperty.NotNull, "'f'"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_FS_IMPORT_MAP_ITEM", "REQUIRED");
        }

        #endregion
    }
}
