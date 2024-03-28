namespace Bars.Gkh.Migrations._2018.Version_2018040700
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018040700")]
    [MigrationDependsOn(typeof(Version_2018032300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_START_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_START_DATE");
        }
    }
}