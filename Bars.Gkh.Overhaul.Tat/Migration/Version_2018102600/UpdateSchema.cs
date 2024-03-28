namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2018102600
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [Migration("2018102600")]
    [MigrationDependsOn(typeof(Version_2018082200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_PRG_VERSION", "IS_PROGRAM_PUBLISHED", DbType.Boolean, ColumnProperty.Null);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_PRG_VERSION", "IS_PROGRAM_PUBLISHED");
        }
    }
}