namespace Bars.B4.Modules.Analytics.Migrations.Version_2014073000
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014073000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.B4.Modules.Analytics.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("AL_DATA_SOURCE", "DATA_SOURCE_TYPE", DbType.Int32, ColumnProperty.NotNull, 1);
        }

        public override void Down()
        {
            Database.RemoveColumn("AL_DATA_SOURCE", "DATA_SOURCE_TYPE");
        }
    }
}
