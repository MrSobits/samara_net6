using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2020.Version_2020061600
{
    [Migration("2020061600")]
    [MigrationDependsOn(typeof(_2019.Version_2019122400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("DEBT_WORK_TYPE", DbType.Int16,10));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "DEBT_WORK_TYPE");
        }
    }
}