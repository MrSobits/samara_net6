using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.Regions.Voronezh.Migrations._2020.Version_2020092800
{
    [Migration("2020092800")]
    [MigrationDependsOn(typeof(_2020.Version_2020061600.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {

        public override void Up()
        {
            Database.AddColumn("CLW_LAWSUIT_DEBTWORK", new Column("CB_DOC_RETURNED", DbType.Boolean));
        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_LAWSUIT_DEBTWORK", "CB_DOC_RETURNED");
        }
    }
}