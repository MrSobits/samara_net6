namespace Bars.GkhDi.Migrations._2017.Version_2017080800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2017080800")]
    [MigrationDependsOn(typeof(Version_2016120300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("DI_DICT_TEMPL_SERVICE", "COMMUNAL_RESOURCE", DbType.Int32, ColumnProperty.Null);
            this.Database.AddColumn("DI_DICT_TEMPL_SERVICE", "HOUSING_RESOURCE", DbType.Int32, ColumnProperty.Null);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("DI_DICT_TEMPL_SERVICE", "COMMUNAL_RESOURCE");
            this.Database.RemoveColumn("DI_DICT_TEMPL_SERVICE", "HOUSING_RESOURCE");
        }
    }
}
