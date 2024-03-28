namespace Bars.Gkh.Migrations._2015.Version_2015100102
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100102")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015100101.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AlterColumnSetNullable("CLW_VIOL_CLAIM_WORK", "NAME", true);
            Database.ChangeColumn("CLW_VIOL_CLAIM_WORK", new Column("NAME", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE CLW_VIOL_CLAIM_WORK SET NAME = '' WHERE NAME is NULL");
            Database.AlterColumnSetNullable("CLW_VIOL_CLAIM_WORK", "NAME", false);
        }
    }
}
