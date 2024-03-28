using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Entities;

namespace Bars.Gkh.Migrations._2015.Version_2015121700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015121700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof (global::Bars.Gkh.Migrations._2015.Version_2015120701.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_INSTR_GROUP",
                new Column("DISPLAY_NAME", DbType.String, 255));

            Database.AddEntityTable("GKH_INSTR_GROUP_ROLE",
                new RefColumn("GROUP_ID", "GKH_INSTRGR_ROLE_GID", "GKH_INSTR_GROUP", "ID"),
                new RefColumn("ROLE_ID", "GKH_INSTRGR_ROLE_RID", "B4_ROLE", "ID"));

            Database.AddRefColumn("GKH_INSTRUCTIONS", 
                new RefColumn("GROUP_ID", "GKH_INSTR_INSTRGID", "GKH_INSTR_GROUP", "ID"));

            Database.Commit();

            var container = ApplicationContext.Current.Container;
            var instructionDomain = container.ResolveDomain<Instruction>();
            var instructionGroupDomain = container.ResolveDomain<InstructionGroup>();
            try
            {
                var group = new InstructionGroup { DisplayName = "Инструкции" };
                instructionGroupDomain.Save(group);
                instructionDomain.GetAll().ForEach(x =>
                {
                    x.InstructionGroup = group;
                    instructionDomain.Update(x);
                });
            }
            finally
            {
                container.Release(instructionDomain);
                container.Release(instructionGroupDomain);
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_INSTRUCTIONS", "GROUP_ID");

            Database.RemoveTable("GKH_INSTR_GROUP_ROLE");
            Database.RemoveTable("GKH_INSTR_GROUP");
        }
    }
}
