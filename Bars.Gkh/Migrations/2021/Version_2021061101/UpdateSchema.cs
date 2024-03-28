namespace Bars.Gkh.Migrations._2021.Version_2021061101
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021061101")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021061100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_HOUSEKEEPER", new Column("PHONE_NUMBER", DbType.String, 50));
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_HOUSEKEEPER", "PHONE_NUMBER");
        }
    }
}