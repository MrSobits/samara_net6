namespace Bars.GkhGji.Regions.Tatarstan.Migration.Version_2019112900
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Migrations;
    using System.Data;

    [Migration("2019112900")]
    [MigrationDependsOn(typeof(Version_2019112700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //справочник "Конфигурация справочной информации ТОР КНД"
            this.Database.AddEntityTable("GJI_DICT_CONFIG_REF_INFORMATION_KND_TOR",
                new GuidColumn("TOR_ID", ColumnProperty.Null),
                new Column("VALUE", DbType.String, ColumnProperty.NotNull),
                new Column("TYPE", DbType.Int32, ColumnProperty.NotNull));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_CONFIG_REF_INFORMATION_KND_TOR");
        }
    }
}
