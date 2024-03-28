namespace Bars.GkhDi.Migrations.Version_2016033000
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    /// <summary>
    /// Миграция номер [2016033000]
    /// </summary>
    [Migration("2016033000")]
    [MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations.Version_2016031500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.RenameColumn("DI_COMMUNAL_SERVICE", "CONS_NORM_LIV_HOUSE", "OLD_COLUM_CONS_NORM_LIV_HOUSE");
            this.Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("CONS_NORM_LIV_HOUSE", DbType.Decimal));
            this.Database.ExecuteNonQuery("UPDATE DI_COMMUNAL_SERVICE SET OLD_COLUM_CONS_NORM_LIV_HOUSE= REPLACE(OLD_COLUM_CONS_NORM_LIV_HOUSE, ',', '.')");
            this.Database.ExecuteNonQuery("UPDATE DI_COMMUNAL_SERVICE SET OLD_COLUM_CONS_NORM_LIV_HOUSE = NULL WHERE OLD_COLUM_CONS_NORM_LIV_HOUSE SIMILAR TO '%([А-Я]|[а-я]|[a-z]|-| )%'");
            this.Database.ExecuteNonQuery("update DI_COMMUNAL_SERVICE set CONS_NORM_LIV_HOUSE = CAST(OLD_COLUM_CONS_NORM_LIV_HOUSE AS numeric(18,5)) " 
                + "where OLD_COLUM_CONS_NORM_LIV_HOUSE <> ''");

            this.Database.RemoveColumn("DI_COMMUNAL_SERVICE", "OLD_COLUM_CONS_NORM_LIV_HOUSE");
        }

        public override void Down()
        {
            this.Database.RenameColumn("DI_COMMUNAL_SERVICE", "CONS_NORM_LIV_HOUSE", "OLD_COLUM_CONS_NORM_LIV_HOUSE");
            this.Database.AddColumn("DI_COMMUNAL_SERVICE", new Column("CONS_NORM_LIV_HOUSE", DbType.String, 300));
            this.Database.ExecuteNonQuery("update DI_COMMUNAL_SERVICE set CONS_NORM_LIV_HOUSE = CAST(OLD_COLUM_CONS_NORM_LIV_HOUSE AS character varying(300))");
            this.Database.RemoveColumn("DI_COMMUNAL_SERVICE", "OLD_COLUM_CONS_NORM_LIV_HOUSE");
        }
    }
}
