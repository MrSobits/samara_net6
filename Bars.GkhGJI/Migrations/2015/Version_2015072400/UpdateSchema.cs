namespace Bars.GkhGji.Migrations._2015.Version_2015072400
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;
    using Gkh;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015072400")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015061102.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", "GJI_NUM_SORT", DbType.Int64, ColumnProperty.Null);
        }

        public override void Down()
        {
        }
    }
}