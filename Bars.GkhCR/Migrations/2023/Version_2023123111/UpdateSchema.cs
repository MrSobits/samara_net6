using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123111
{
    [Migration("2023123111")]
    [MigrationDependsOn(typeof(_2023.Version_2023123110.UpdateSchema))]
    // Является Version_2020071600 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        private const string TableName = "CR_CTRL_DATE_MUNICIPALITY_LIMIT_DATE";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddEntityTable(UpdateSchema.TableName,
                new RefColumn("CONTROL_DATE_ID", ColumnProperty.NotNull, "CR_CTRL_DATE_MUNICIPALITY_LIMIT_DATE_CONTROL_DATE",
                    "CR_CONTROL_DATE", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "CR_CTRL_DATE_MUNICIPALITY_LIMIT_DATE_MUNICIPALITY",
                "GKH_DICT_MUNICIPALITY", "ID"),
                new Column("LIMIT_DATE", DbType.DateTime, ColumnProperty.NotNull));
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveTable(UpdateSchema.TableName);
        }
    }
}