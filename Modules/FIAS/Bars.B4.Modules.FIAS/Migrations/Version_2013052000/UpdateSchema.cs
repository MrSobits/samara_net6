namespace Bars.B4.Modules.FIAS.Migrations.Version_2013052000
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013052000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.FIAS.Migrations.Version_2012111400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("B4_FIAS", new Column("MIRROR_GUID", DbType.String, 36));

            Database.AddIndex("B4_FIAS_MRGUID", false, "B4_FIAS", "MIRROR_GUID");
        }

        public override void Down()
        {
            Database.RemoveColumn("B4_FIAS", "MIRROR_GUID");
        }
    }
}