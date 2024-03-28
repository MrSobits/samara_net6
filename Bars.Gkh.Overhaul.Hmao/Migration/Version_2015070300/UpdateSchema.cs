namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2015070300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция Overhaul.Hmao 2015070300
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015070300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Hmao.Migration.Version_2015012800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("OVRHL_ACTUALIZE_LOG",
                new RefColumn("VERSION_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_PV", "OVRHL_PRG_VERSION", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_MU", "GKH_DICT_MUNICIPALITY", "ID"),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "OVRHL_ACTUALIZE_LOG_F", "B4_FILE_INFO", "ID"),
                new Column("TYPE_ACTUALIZE", DbType.Int16, ColumnProperty.NotNull, 10),
                new Column("DATE_ACTION", DbType.DateTime, ColumnProperty.NotNull),
                new Column("USER_NAME", DbType.String, 500),
                new Column("COUNT_ACTIONS", DbType.Int32));

            this.Database.AddColumn("OVRHL_VERSION_REC", new Column("FIXED_YEAR", DbType.Boolean, ColumnProperty.NotNull, false));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_VERSION_REC", "FIXED_YEAR");
            this.Database.RemoveTable("OVRHL_ACTUALIZE_LOG");
        }
    }
}