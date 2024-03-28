namespace Bars.Gkh.Migrations._2023.Version_2023050122
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050122")]

    [MigrationDependsOn(typeof(Version_2023050121.UpdateSchema))]

    /// Является Version_2019100700 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddGkhDictTable("GKH_DICT_RISK_CATEGORY");

            this.Database.ExecuteNonQuery(@"insert into GKH_DICT_RISK_CATEGORY (object_version, object_create_date, object_edit_date, CODE, NAME) values
                                                (0, now(), now(), '1', 'Чрезвычайно высокий'),
                                                (0, now(), now(), '2', 'Высокий'),
                                                (0, now(), now(), '3', 'Значительный'),
                                                (0, now(), now(), '4', 'Средний'),
                                                (0, now(), now(), '5', 'Умеренный'),
                                                (0, now(), now(), '6', 'Низкий')"
            );
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_DICT_RISK_CATEGORY");
        }
    }
}