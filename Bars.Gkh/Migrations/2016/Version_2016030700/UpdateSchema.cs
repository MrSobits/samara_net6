namespace Bars.Gkh.Migrations._2016.Version_2016030700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016030700")]
    [MigrationDependsOn(typeof(Version_1.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_DICT_WORK", "IS_CONSTRUCTION_WORK", DbType.Boolean, ColumnProperty.NotNull, false);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_WORK", "IS_CONSTRUCTION_WORK");
        }
    }
}