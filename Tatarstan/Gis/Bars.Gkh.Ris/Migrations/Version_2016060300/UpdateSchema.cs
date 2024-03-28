namespace Bars.Gkh.Ris.Migrations.Version_2016060300
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016060300")]
    [MigrationDependsOn(typeof(Version_2016053100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Метод миграции на версию вперед
        /// </summary>
        public override void Up()
        {
            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "INN"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("INN", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_SURNAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_ENTPR_SURNAME", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_FIRSTNAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_ENTPR_FIRSTNAME", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_PATRONYMIC"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_ENTPR_PATRONYMIC", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_KPP"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_LEGAL_KPP", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_NAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("RECIPIENT_LEGAL_NAME", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_ID"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("PAYMENT_DOCUMENT_ID", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_NUMBER"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("PAYMENT_DOCUMENT_NUMBER", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "YEAR"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("YEAR", DbType.Int16));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "MONTH"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("MONTH", DbType.Int32));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "UNIFIED_ACCOUNT_NUMBER"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("UNIFIED_ACCOUNT_NUMBER", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "FIAS_HOUSE_GUID"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("FIAS_HOUSE_GUID", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "APARTMENT"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("APARTMENT", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PLACEMENT"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("PLACEMENT", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_SURNAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("CONSUMER_SURNAME", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_FIRST_NAME"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("CONSUMER_FIRST_NAME", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_PATRONYMIC"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("CONSUMER_PATRONYMIC", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_INN"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("CONSUMER_INN", DbType.String));
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "SERVICE_ID"))
            //{
            //    this.Database.AddColumn("RIS_NOTIFORDEREXECUT", new Column("SERVICE_ID", DbType.String));
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PAYMDOC_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "PAYMDOC_ID");
            //}

            //if (!this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RIS_PAYM_DOC_ID"))
            //{
            //    this.Database.AddRefColumn("RIS_NOTIFORDEREXECUT", new RefColumn("RIS_PAYM_DOC_ID", "RIS_NOTIFORDEREX_PAYM_DOC", "RIS_PAYMENT_DOC", "ID")); 
            //}

        }

        /// <summary>
        /// Метод миграции на версию назад
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "INN"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "INN");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_SURNAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_SURNAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_FIRSTNAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_FIRSTNAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_PATRONYMIC"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_ENTPR_PATRONYMIC");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_KPP"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_KPP");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_NAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RECIPIENT_LEGAL_NAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_ID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_NUMBER"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "PAYMENT_DOCUMENT_NUMBER");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "YEAR"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "YEAR");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "MONTH"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "MONTH");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "UNIFIED_ACCOUNT_NUMBER"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "UNIFIED_ACCOUNT_NUMBER");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "FIAS_HOUSE_GUID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "FIAS_HOUSE_GUID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "APARTMENT"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "APARTMENT");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "PLACEMENT"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "PLACEMENT");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_SURNAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "CONSUMER_SURNAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_FIRST_NAME"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "CONSUMER_FIRST_NAME");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_PATRONYMIC"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "CONSUMER_PATRONYMIC");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "CONSUMER_INN"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "CONSUMER_INN");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "SERVICE_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "SERVICE_ID");
            //}

            //if (this.Database.ColumnExists("RIS_NOTIFORDEREXECUT", "RIS_PAYM_DOC_ID"))
            //{
            //    this.Database.RemoveColumn("RIS_NOTIFORDEREXECUT", "RIS_PAYM_DOC_ID");
            //}
        }
    }
}