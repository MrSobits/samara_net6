namespace Bars.Gkh.Migrations._2015.Version_2015093003
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Utils;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015093003")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015093002.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AlterColumnSetNullable("REAL_EST_TYPE_COMM_PARAM", "MAX", true);
        }

        public override void Down()
        {
            Database.ExecuteNonQuery("UPDATE REAL_EST_TYPE_COMM_PARAM SET MAX = '' WHERE MAX is NULL");
            this.Database.AlterColumnSetNullable("REAL_EST_TYPE_COMM_PARAM", "MAX", false);
        }
    }
}