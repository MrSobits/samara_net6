namespace Bars.GkhGji.Migrations._2022.Version_2022050500
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022050500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022042000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {           

            Database.AddColumn("GJI_DICISION", new Column("PROSECUTOR_DEC_NUM", DbType.String, 300));
            Database.AddColumn("GJI_DICISION", new Column("PROSECUTOR_DEC_DATE", DbType.DateTime));
            Database.AddColumn("GJI_DICISION", new Column("PERIOD_CORRECT", DbType.String, 50));
            Database.AddColumn("GJI_DICISION", new Column("DATE_STATEMENT", DbType.DateTime));
            Database.AddColumn("GJI_DICISION", new Column("TIME_STATEMENT", DbType.DateTime, 25));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_DICISION", "TIME_STATEMENT");
            Database.RemoveColumn("GJI_DICISION", "DATE_STATEMENT");
            Database.RemoveColumn("GJI_DICISION", "PERIOD_CORRECT");
            Database.RemoveColumn("GJI_DICISION", "PROSECUTOR_DEC_DATE");
            Database.RemoveColumn("GJI_DICISION", "PROSECUTOR_DEC_NUM");
        }
    }
}