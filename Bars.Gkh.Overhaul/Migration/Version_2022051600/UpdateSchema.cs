namespace Bars.Gkh.Overhaul.Migration.Version_2022051600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2022051600")]
    [MigrationDependsOn(typeof(Version_2019082700.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("WEAROUT_ACUAL", DbType.Decimal, ColumnProperty.NotNull, 0));
            this.Database.ExecuteQuery("UPDATE OVRHL_RO_STRUCT_EL SET WEAROUT_ACUAL = WEAROUT");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "WEAROUT_ACUAL");
        }
    }
}