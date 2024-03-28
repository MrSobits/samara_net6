namespace Bars.Gkh.Migrations._2023.Version_2023050128
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050128")]

    [MigrationDependsOn(typeof(Version_2023050127.UpdateSchema))]

    /// Является Version_2020012400 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("DI_DICT_TEMPL_SERVICE", new Column("IS_CONSIDER_IN_CALC", DbType.Boolean, false));
            this.Database.AddColumn("DI_DICT_TEMPL_SERVICE", new Column("ACTUAL_START_YEAR", DbType.Int32));
            this.Database.AddColumn("DI_DICT_TEMPL_SERVICE", new Column("ACTUAL_END_YEAR", DbType.Int32));

            this.Database.ExecuteNonQuery(
                @"
                    update  DI_DICT_TEMPL_SERVICE set ACTUAL_END_YEAR = 2015, IS_MANDATORY = true where CODE = '33';
                    update  DI_DICT_TEMPL_SERVICE set ACTUAL_END_YEAR = 2016, IS_MANDATORY = true where CODE = '16';
                    update  DI_DICT_TEMPL_SERVICE set IS_MANDATORY = true where CODE::int in ( 1, 2, 6, 7, 13, 14, 27, 28, 8, 9, 10, 11, 12, 16, 18, 17, 19, 20, 22, 21);
                    update  DI_DICT_TEMPL_SERVICE set IS_CONSIDER_IN_CALC = true where CODE::int in (17, 18, 19, 20, 21, 22, 33);
                    update  DI_DICT_TEMPL_SERVICE set IS_CONSIDER_IN_CALC = true where code::int in (1, 2, 6, 8, 9, 11, 12, 13, 14, 16);");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("DI_DICT_TEMPL_SERVICE", "IS_CONSIDER_IN_CALC");
            this.Database.RemoveColumn("DI_DICT_TEMPL_SERVICE", "ACTUAL_START_YEAR");
            this.Database.RemoveColumn("DI_DICT_TEMPL_SERVICE", "ACTUAL_END_YEAR");
        }
    }
}