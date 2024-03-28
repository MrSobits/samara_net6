namespace Bars.Gkh.Migrations._2023.Version_2023050106
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050106")]

    [MigrationDependsOn(typeof(Version_2023050105.UpdateSchema))]

    /// Является Version_2018041000 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            if (this.Database.TableExists("TP_TEH_PASSPORT_VALUE"))
            {
                this.Database.ExecuteNonQuery(@"update tp_teh_passport_value
                                                set value = split_part(replace(replace(value, '[', ''), ']', ''), ',', 1)
                                                where form_code = 'Form_5_11'
                                                and cell_code = '4:1'");
            }
        }
    }
}