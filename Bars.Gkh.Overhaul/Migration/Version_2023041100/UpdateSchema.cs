namespace Bars.Gkh.Overhaul.Migration.Version_2023041100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2023041100")]
    [MigrationDependsOn(typeof(Version_2022051600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddRefColumn("OVRHL_RO_STRUCT_EL", new RefColumn("STRUCT_EL_FILE_ID", "OVRHL_RO_STRUCT_EL_FILE", "B4_FILE_INFO", "ID"));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "STRUCT_EL_FILE_ID");
        }
    }
}