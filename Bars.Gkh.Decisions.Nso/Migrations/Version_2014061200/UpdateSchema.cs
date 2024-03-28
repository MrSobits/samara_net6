namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014061200
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_2014060800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            // Ведение счета
            Database.AddTable("DEC_ACCOUNT_MANAGE",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DECISION", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_ACCOUNT_MANAGE");
        }
    }
}
