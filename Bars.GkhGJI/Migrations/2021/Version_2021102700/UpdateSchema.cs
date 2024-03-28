namespace Bars.GkhGji.Migrations._2021.Version_2021102700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021102700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021102600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_ACTCHECK", "UNAVALIABLE_CHECK", DbType.Boolean, ColumnProperty.NotNull, false);
            Database.AddColumn("GJI_ACTCHECK", "UNAVALIABLE_REASON", DbType.String, 1500);          
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_ACTCHECK", "UNAVALIABLE_REASON");
            Database.RemoveColumn("GJI_ACTCHECK", "UNAVALIABLE_CHECK");
        }
    }
}