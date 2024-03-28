using Bars.Gkh.Enums;

namespace Bars.Gkh.Migrations._2015.Version_2015102600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015102200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            if (Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORGANIZATION_FS"))
            {
                Database.RemoveConstraint("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORGANIZATION_FS");
            }
            if (Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "IND_GKH_MANAGING_ORGANIZATION_FS"))
            {
                Database.RemoveIndex("IND_GKH_MANAGING_ORGANIZATION_FS", "GKH_MANAGING_ORGANIZATION");
            }


            if (Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORGANIZATION_DW"))
            {
                Database.RemoveConstraint("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORGANIZATION_DW");
            }
            if (Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "IND_GKH_MANAGING_ORGANIZATION_DW"))
            {
                Database.RemoveIndex("IND_GKH_MANAGING_ORGANIZATION_DW", "GKH_MANAGING_ORGANIZATION");
            }


            if (!Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORG_FS"))
            {
                Database.AddForeignKey("FK_GKH_MANAGING_ORG_FS", "GKH_MANAGING_ORGANIZATION", "FILE_STATUTE_ID", "B4_FILE_INFO", "ID");
            }
            if (!Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "IND_GKH_MANAGING_ORG_FS"))
            {
                Database.AddIndex("IND_GKH_MANAGING_ORG_FS", false, "GKH_MANAGING_ORGANIZATION", "FILE_STATUTE_ID");
            }

            if (!Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "FK_GKH_MANAGING_ORG_DW"))
            {
                Database.AddForeignKey("FK_GKH_MANAGING_ORG_DW", "GKH_MANAGING_ORGANIZATION", "FIAS_DISPATCH_ID", "B4_FIAS_ADDRESS", "ID");
            }
            if (!Database.ConstraintExists("GKH_MANAGING_ORGANIZATION", "IND_GKH_MANAGING_ORG_DW"))
            {
                Database.AddIndex("IND_GKH_MANAGING_ORG_DW", false, "GKH_MANAGING_ORGANIZATION", "FIAS_DISPATCH_ID");
            }
        }

        public override void Down()
        {
        }
    }
}