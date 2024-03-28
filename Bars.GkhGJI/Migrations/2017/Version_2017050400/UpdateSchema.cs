namespace Bars.GkhGji.Migrations._2017.Version_2017050400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017050400")]
    [MigrationDependsOn(typeof(Version_2017032000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017032800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017041800.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017042000.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017042600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Query = "update GJI_DICT_KINDSTATEMENT set POSTFIX=''";

        public override void Up()
        {
            this.Database.AddColumn("GJI_DICT_KINDSTATEMENT", "POSTFIX", DbType.String, 100);

            this.Database.ExecuteNonQuery(Query);

            this.Database.ChangeColumnNotNullable("GJI_DICT_KINDSTATEMENT", "POSTFIX", true);
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DICT_KINDSTATEMENT", "POSTFIX");
        }
    }
}