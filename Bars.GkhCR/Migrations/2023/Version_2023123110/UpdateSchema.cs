using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123110
{
    [Migration("2023123110")]
    [MigrationDependsOn(typeof(_2023.Version_2023123109.UpdateSchema))]
    // Является Version_2019051500 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Schema = "PUBLIC", Name = "GKH_DICT_WORK" };
        private readonly Column column = new Column("IS_ACTUAL", DbType.Boolean, ColumnProperty.NotNull, true);

        public override void Up()
        {
            this.Database.AddColumn(this.table, this.column);
        }

        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.column.Name);
        }
    }
}