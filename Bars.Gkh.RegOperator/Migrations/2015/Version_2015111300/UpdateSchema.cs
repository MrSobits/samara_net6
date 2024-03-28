namespace Bars.Gkh.RegOperator.Migrations._2015.Version_2015111300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015.11.13.00
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2015.Version_2015111100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("RF_TRANSFER_CTR", new Column("DOCUMENT_NUM_PP", DbType.Int32, 0));
            this.Database.AddColumn("RF_TRANSFER_CTR", new Column("DATE_FROM_PP", DbType.DateTime));
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("RF_TRANSFER_CTR", "DOCUMENT_NUM_PP");
            this.Database.RemoveColumn("RF_TRANSFER_CTR", "DATE_FROM_PP");
        }
    }
}
