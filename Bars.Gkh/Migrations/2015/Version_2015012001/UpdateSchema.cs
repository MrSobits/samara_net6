using System.Data;
using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

namespace Bars.Gkh.Migrations.Version_2015012001
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015012001")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015012000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                    "CLW_VIOL_CLAIM_WORK",
                    new Column("NAME", DbType.String, ColumnProperty.NotNull, 1000),
                    new Column("CODE", DbType.String, 1000));
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_VIOL_CLAIM_WORK");
        }
    }
}