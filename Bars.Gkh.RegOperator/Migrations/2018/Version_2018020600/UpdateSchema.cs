namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018020600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2018020600")]
   
    [MigrationDependsOn(typeof(Version_2018020200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.ChangeColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_NUM", DbType.Int32, ColumnProperty.NotNull, 0));
            this.Database.ChangeColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_DEN", DbType.Int32, ColumnProperty.NotNull, 1));
        }
        public override void Down()
        {
            this.Database.ChangeColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_NUM", DbType.Byte, ColumnProperty.NotNull, 0));
            this.Database.ChangeColumn("REGOP_LAWSUIT_OWNER_INFO", new Column("AREA_SHARE_DEN", DbType.Byte, ColumnProperty.NotNull, 1));
        }
    }
}