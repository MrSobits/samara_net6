namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014061202
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061202")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014061201.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_RO_LOAN", new Column("TYPE_SOURCE_LOAN", DbType.Int16, ColumnProperty.NotNull, 10));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_RO_LOAN", "TYPE_SOURCE_LOAN");
        }
    }
}
