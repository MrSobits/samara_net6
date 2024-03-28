using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023082100
{
    [Migration("2023082100")]
    [MigrationDependsOn(typeof(_2023.Version_2023081800.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("CR_OBJ_PERFOMED_WORK_ACT_PHOTO",
                new RefColumn("PERF_WORK_ACT_ID", "CR_OBJ_PERFOMED_WORK_ACT_PERFWORKACT", "CR_OBJ_PERFOMED_WORK_ACT", "ID"),
                new RefColumn("PHOTO", "CR_OBJ_PERFOMED_WORK_ACT_PHOTO_FILE", "B4_FILE_INFO", "ID"),
                new Column("NAME", DbType.String, 100),
                new Column("DISCRIPTION", DbType.String, 2000),
                new Column("PHOTO_TYPE", DbType.Int16));
        }

        public override void Down()
        {
            Database.RemoveTable("CR_OBJ_PERFOMED_WORK_ACT_PHOTO");
        }
    }
}