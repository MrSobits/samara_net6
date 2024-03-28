namespace Bars.Gkh.Migrations._2018.Version_2018060601
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018060601")]
    [MigrationDependsOn(typeof(Version_2018060600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {

            Database.AddColumn("CLW_LAWSUIT", new Column("DEBT_CALC_METHOD", DbType.Int16, ColumnProperty.None));
        }

        public override void Down()
        {

            Database.RemoveColumn("CLW_LAWSUIT", "DEBT_CALC_METHOD");
        }
    }
}