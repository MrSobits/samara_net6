namespace Bars.B4.Modules.FIAS.Migrations.Version_2014012100
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014012100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2013052000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("B4_FIAS_ADDRESS", new Column("LETTER", DbType.String, 1));

            Database.AddIndex("B4_FIAS_ADDRESS_LETTER", false, "B4_FIAS_ADDRESS", "LETTER");
        }

        public override void Down()
        {
            Database.RemoveColumn("B4_FIAS_ADDRESS", "LETTER");
        }
    }
}