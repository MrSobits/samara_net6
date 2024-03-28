namespace Bars.GkhCr.Migrations._2017.Version_2017031200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017031200
    /// </summary>
    [Migration("2017031200")]
    [MigrationDependsOn(typeof(Version_2017011100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применение миграции
        /// </summary>
        public override void Up()
        {
            this.Database.AddEntityTable("CR_OBJ_ADDITIONAL_PARAMS",
                new RefColumn("OBJECT_ID", "CR_OBJ_ADDITIONAL_PARAMS_OBJ_ID", "CR_OBJECT", "ID"),
                new Column("REQUEST_KTS_DATE", DbType.DateTime),
                new Column("TECH_COND_KTS_DATE", DbType.DateTime),
                new Column("TECH_COND_KTS_RECIPIENT", DbType.String),
                new Column("REQUEST_VODOKANAL_DATE", DbType.DateTime),
                new Column("TECH_COND_VODOKANAL_DATE", DbType.DateTime),
                new Column("TECH_COND_VODOKANAL_RECIPIENT", DbType.String),
                new Column("ENTRY_FOR_APPROVAL_DATE", DbType.DateTime),
                new Column("APPROVAL_KTS_DATE", DbType.DateTime),
                new Column("APPROVAL_VODOKANAL_DATE", DbType.DateTime),
                new Column("INSTALL_PERCENT", DbType.Decimal),
                new Column("CLIENT_ACCEPT", DbType.Int32),
                new Column("CLIENT_ACCEPT_CHANGE_DATE", DbType.DateTime),
                new Column("INSPECTOR_ACCEPT", DbType.Int32),
                new Column("INSPECTOR_ACCEPT_CHANGE_DATE", DbType.DateTime));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveTable("CR_OBJ_ADDITIONAL_PARAMS");
        }
    }
}