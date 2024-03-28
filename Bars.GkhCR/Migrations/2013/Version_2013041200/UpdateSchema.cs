namespace Bars.GkhCr.Migrations.Version_2013041200
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2013041000.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_DICT_QUAL_MEMBER", new RefColumn("ROLE_ID", "CR_QUAL_MEM_ROLE", "B4_ROLE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_DICT_QUAL_MEMBER", "ROLE_ID");
        }
    }
}