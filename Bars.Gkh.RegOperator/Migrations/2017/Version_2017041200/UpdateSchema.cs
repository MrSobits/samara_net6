namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017041200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Миграция RegOperator 2017041200
    /// </summary>
    [Migration("2017041200")]
    [MigrationDependsOn(typeof(Version_2017032200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("ACCEPT_DATE", DbType.DateTime));
            this.Database.AddColumn("REGOP_BANK_DOC_IMPORT", new Column("IMPORT_TYPE", DbType.String, 255));

            this.Database.AddColumn("REGOP_IMPORTED_PAYMENT", new Column("ACCEPT_DATE", DbType.DateTime));

            // заполним даты
            this.Database.ExecuteNonQuery(@"UPDATE REGOP_IMPORTED_PAYMENT p
                        set ACCEPT_DATE = p.OBJECT_EDIT_DATE
                        where p.ACCEPTED");

            this.Database.ExecuteNonQuery($@"UPDATE REGOP_BANK_DOC_IMPORT b
                        set ACCEPT_DATE = b.OBJECT_EDIT_DATE
                        where b.STATE = {(int)PaymentOrChargePacketState.Accepted}");
        }

        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_BANK_DOC_IMPORT", "ACCEPT_DATE");
            this.Database.RemoveColumn("REGOP_IMPORTED_PAYMENT", "ACCEPT_DATE");
        }
    }
}
