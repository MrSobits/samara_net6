using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123104
{
    [Migration("2023123104")]
    [MigrationDependsOn(typeof(_2023.Version_2023123103.UpdateSchema))]
    // Является Version_2018091900 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("REPRESENTATIVE_SURNAME", DbType.String));
            this.Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("REPRESENTATIVE_PATRONYMIC", DbType.String));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_SURNAME");
            this.Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "REPRESENTATIVE_PATRONYMIC");
        }
    }
}