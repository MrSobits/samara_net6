namespace Bars.Gkh.Migrations._2016.Version_2016012100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Миграция 21.01.2016
    /// </summary>
    [Migration("2016012100")]
    [MigrationDependsOn(typeof(Version_2016011500.UpdateSchema))]
    class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("GKH_DICT_CAPITAL_GROUP","NAME",true);
        }

        public override void Down()
        {
            this.Database.ExecuteNonQuery("UPDATE GKH_DICT_CAPITAL_GROUP SET NAME = '' WHERE NAME is NULL");
            this.Database.AlterColumnSetNullable("GKH_DICT_CAPITAL_GROUP", "NAME", false);
        }
    }
}