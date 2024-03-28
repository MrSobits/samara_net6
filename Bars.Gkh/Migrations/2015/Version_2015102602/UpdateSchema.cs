namespace Bars.Gkh.Migration.Version_2015102602
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015102602")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015102601.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накат миграции
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("GENDER", DbType.Int32, ColumnProperty.NotNull, "0"));
            Database.AddColumn("GKH_CONTRAGENT_CONTACT", new Column("BIRTH_DATE", DbType.DateTime));
        }

        /// <summary>
        /// Откат миграции
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "BIRTH_DATE");
            Database.RemoveColumn("GKH_CONTRAGENT_CONTACT", "GENDER");
        }
    }
}