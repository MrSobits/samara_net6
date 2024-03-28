using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2020.Version_2020052100
{
    [Migration("2020052100")]
    [MigrationDependsOn(typeof(Version_2020051400.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_OBJ_TW_STAGE", new Column("QUEUE", DbType.Int16, ColumnProperty.None, null));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("CR_OBJ_TW_STAGE", "QUEUE");
        }
    }
}