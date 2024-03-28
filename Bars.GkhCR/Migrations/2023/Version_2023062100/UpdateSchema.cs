using Bars.B4.Modules.Ecm7.Framework;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023062100
{
    [Migration("2023062100")]
    [MigrationDependsOn(typeof(_2023.Version_2023060500.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_CMP_BUILD_CONTR_FILE", new Column("VIDEOLINK", DbType.String, 1500));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_CMP_BUILD_CONTR_FILE", "VIDEOLINK");        
        }
    }
}