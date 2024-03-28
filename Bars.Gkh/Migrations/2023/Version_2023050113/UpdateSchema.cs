namespace Bars.Gkh.Migrations._2023.Version_2023050113
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2023050113")]

    [MigrationDependsOn(typeof(Version_2023050112.UpdateSchema))]

    /// Является Version_2018091600 из ядра
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                UPDATE gkh_dict_type_information_npa SET code = regexp_replace(code, '(\d+)\.(\d+)', E'\\1.\\2.', 'g');");
        }
    }
}