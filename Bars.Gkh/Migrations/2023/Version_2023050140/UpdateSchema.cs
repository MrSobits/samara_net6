namespace Bars.Gkh.Migrations._2023.Version_2023050140
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    [Migration("2023050140")]

    [MigrationDependsOn(typeof(Version_2023050139.UpdateSchema))]

    /// Является Version_2021070700 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            // 1) Заменяем полную дату (ДДММГГГГ, ДММГГГГ) в поле на год (ГГГГ)
            // 2) Очищаем оставшиеся некорректные значения
            this.Database.ExecuteNonQuery(@"update tp_teh_passport_value
                                                set value = substring(value, length(value)-3, 4)
                                                where form_code = 'Form_3_1_3' 
                                                and cell_code = '34:1' 
                                                and octet_length(value) > 4;

                                                update tp_teh_passport_value
                                                set value = ''
                                                where form_code = 'Form_3_1_3' 
                                                and cell_code = '34:1' 
                                                and value !~ '^\d{4}$'");
        }
    }
}