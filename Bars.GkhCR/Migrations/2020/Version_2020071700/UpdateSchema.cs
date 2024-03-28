using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020071700
{
    [Migration("2020071700")]
    [MigrationDependsOn(typeof(Version_2020070700.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("CR_OBJ_PERFOMED_WORK_ACT", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("DATE_FROM_TRANSFER", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("CR_OBJ_PERFOMED_WORK_ACT", new Column("OVER_LIMITS", DbType.Boolean, ColumnProperty.NotNull, false));

        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "OVER_LIMITS");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "DATE_FROM_TRANSFER");
            Database.RemoveColumn("CR_OBJ_PERFOMED_WORK_ACT", "CR_OBJ_PERFOMED_WORK_ACT");
        }
    }
}