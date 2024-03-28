namespace Bars.Gkh.Migrations._2015.Version_2015093002
{
    using System.Data;
    using Bars.Gkh.Utils;
    using global::Bars.B4.Modules.Ecm7.Framework;


    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015093001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("CLW_DICT_PETITION_TYPE", "CODE", false);

        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE CLW_DICT_PETITION_TYPE SET CODE = '' WHERE CODE is NULL");
            this.Database.AlterColumnSetNullable("CLW_DICT_PETITION_TYPE", "CODE", true);
        }
    }
}
