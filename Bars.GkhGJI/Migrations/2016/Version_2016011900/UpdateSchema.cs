namespace Bars.GkhGji.Migrations.Version_2016011900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2016011900
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2016011900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015122400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {

            Database.AddColumn("GJI_APPCIT_ANSWER", new Column("YEAR", DbType.Int32));
            Database.ExecuteNonQuery("update GJI_APPCIT_ANSWER set YEAR = Extract(YEAR from DOCUMENT_DATE)");
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_APPCIT_ANSWER", "YEAR");
        }
    }
}