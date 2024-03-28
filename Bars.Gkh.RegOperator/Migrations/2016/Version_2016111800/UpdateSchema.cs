namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016111800
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Импорт оплаты и начислений в закрытый период
    /// </summary>
    [Migration("2016111800")]
    [MigrationDependsOn(typeof(Version_2016110200.UpdateSchema))] // Логической зависимости нет. Просто, предыдущая по порядку миграция.
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            #region База
            this.Database.AddEntityTable("REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT",
                new RefColumn("TASK_ID", "REGOP_WARN_IN_CLOSED_PERIODS_IMP_TASK", "B4_TASK_ENTRY", "ID"),
                new Column("TITLE", DbType.String, 255),
                new Column("MESSAGE", DbType.String, 1024));

            this.Database.AddTable("REGOP_ACCOUNT_WARNING_IN_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("EXTERNAL_NUMBER", DbType.String, 20),
                new Column("EXTERNAL_RKC_ID", DbType.String, 20),
                new Column("NAME", DbType.String, 150),
                new Column("ADDRESS", DbType.String, 300),
                new Column("IS_PROCESSED", DbType.Int16, ColumnProperty.NotNull, defaultValue: 20), // YesNo.No
                new Column("IS_CAN_AUTO_COMPARED", DbType.Int16, ColumnProperty.NotNull, defaultValue: 20), // YesNo.No
                new Column("COMPARING_ACCOUNT_ID", DbType.Int64),
                new Column("COMPARING_INFO", DbType.String, 450));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_ACC_WARN_IN_CLOSED_PERIODS_IMP",
                "REGOP_ACCOUNT_WARNING_IN_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT", "ID");

            this.Database.AddEntityTable("REGOP_HEADER_OF_CLOSED_PERIODS_IMPORT",
                new RefColumn("TASK_ID", "REGOP_HEAD_OF_CLOSED_PERIODS_IMP_TASK", "B4_TASK_ENTRY", "ID"),                
                new RefColumn("PERIOD_ID", "REGOP_HEAD_OF_CLOSED_PERIODS_IMP_PERIOD", "REGOP_PERIOD", "ID"));
            #endregion

            #region Оплата            
            this.Database.AddTable("REGOP_ACCOUNT_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("INNER_NUMBER", DbType.String, 20),                
                new Column("INNER_RKC_ID", DbType.String, 20));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_ACC_WARN_IN_PAY_TO_CLOSED_PERIODS_IMP",
                "REGOP_ACCOUNT_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_ACCOUNT_WARNING_IN_CLOSED_PERIODS_IMPORT", "ID");

            this.Database.AddTable("REGOP_DATE_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("PAYMENT_DATE", DbType.DateTime));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_DT_WARN_IN_PAY_TO_CLOSED_PERIODS_IMP",
                "REGOP_DATE_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT", "ID");

            this.Database.AddTable("REGOP_HEADER_OF_PAYMENTS_TO_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("IS_UPDATE_SALDO_IN", DbType.Boolean),                
                new Column("EXTERNAL_RKC_ID", DbType.String, 30));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_HEAD_OF_PAY_TO_CLOSED_PERIODS_IMP",
                "REGOP_HEADER_OF_PAYMENTS_TO_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_HEADER_OF_CLOSED_PERIODS_IMPORT", "ID");
            #endregion

            #region Начисления                        
            this.Database.AddTable("REGOP_EXIST_WARNING_IN_CHARGES_TO_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("CHARGE_DESCRIPTOR_NAME", DbType.String, 100));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_EX_WARN_IN_CHG_TO_CLOSED_PERIODS_IMP",
                "REGOP_EXIST_WARNING_IN_CHARGES_TO_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT", "ID");

            this.Database.AddTable("REGOP_HEADER_OF_CHARGES_TO_CLOSED_PERIODS_IMPORT",
                new Column("ID", DbType.Int64, 22, ColumnProperty.PrimaryKey),
                new Column("IS_WITHOUT_SALDO_IN", DbType.Boolean));
            // Добавить ссылку на базовый класс. Индекс создавать не нужно, для PK он будет добавлен автоматически.
            this.Database.AddForeignKey("FK_REGOP_HEAD_OF_CHG_TO_CLOSED_PERIODS_IMP",
                "REGOP_HEADER_OF_CHARGES_TO_CLOSED_PERIODS_IMPORT", "ID",
                "REGOP_HEADER_OF_CLOSED_PERIODS_IMPORT", "ID");
            #endregion
        }

        /// <summary>
        /// Отменить миграцию
        /// </summary>
        public override void Down()
        {
            // Оплата
            this.Database.RemoveTable("REGOP_ACCOUNT_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT");
            this.Database.RemoveTable("REGOP_DATE_WARNING_IN_PAYMENTS_TO_CLOSED_PERIODS_IMPORT");
            this.Database.RemoveTable("REGOP_HEADER_OF_PAYMENTS_TO_CLOSED_PERIODS_IMPORT");
            // Начисления
            this.Database.RemoveTable("REGOP_EXIST_WARNING_IN_CHARGES_TO_CLOSED_PERIODS_IMPORT");
            this.Database.RemoveTable("REGOP_HEADER_OF_CHARGES_TO_CLOSED_PERIODS_IMPORT");            
            // База            
            this.Database.RemoveTable("REGOP_ACCOUNT_WARNING_IN_CLOSED_PERIODS_IMPORT");
            this.Database.RemoveTable("REGOP_WARNING_IN_CLOSED_PERIODS_IMPORT");
            this.Database.RemoveTable("REGOP_HEADER_OF_CLOSED_PERIODS_IMPORT");
        }
    }
}
