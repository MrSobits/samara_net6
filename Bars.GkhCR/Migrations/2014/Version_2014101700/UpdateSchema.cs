namespace Bars.GkhCr.Migrations.Version_2014101700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014101700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhCr.Migrations.Version_2014100702.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("CR_DICT_PROG_CHANGE_JOUR", "DESCRIPTION", DbType.String, 1000, ColumnProperty.Null);
        }

        public override void Down()
        {
            Database.RemoveColumn("CR_DICT_PROG_CHANGE_JOUR", "DESCRIPTION");
        }
    }
}