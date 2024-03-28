namespace Bars.Gkh.Migrations._2018.Version_2018061400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018061400")]
    [MigrationDependsOn(typeof(Version_2018060601.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_END_DATE", DbType.DateTime, ColumnProperty.None));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_END_DATE");
        }
    }
}