namespace Bars.B4.Modules.FIAS.Migrations.Version_2014012900
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2014012100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ChangeColumn("B4_FIAS_ADDRESS", new Column("LETTER", DbType.String, 10));
        }

        public override void Down()
        {
            Database.ChangeColumn("B4_FIAS_ADDRESS", new Column("LETTER", DbType.String, 1));
        }
    }
}