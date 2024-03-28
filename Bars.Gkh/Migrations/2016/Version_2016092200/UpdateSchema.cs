namespace Bars.Gkh.Migrations._2016.Version_2016092200
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2016092200
    /// </summary>
    [Migration("2016092200")]
    [MigrationDependsOn(typeof(Version_2016090500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("START_DATE_PAYMENT_PERIOD", DbType.DateTime));
            this.Database.AddColumn("GKH_MORG_CONTRACT", new Column("END_DATE_PAYMENT_PERIOD", DbType.DateTime));

            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("PAYMENT_AMOUNT", DbType.Decimal));
            this.Database.AddRefColumn("GKH_MORG_CONTRACT_OWNERS", new RefColumn("PAYMENT_PROTOCOL_FILE", "GKH_MORG_CO_PAY_PROT", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GKH_MORG_CONTRACT_OWNERS", new Column("PAYMENT_PROTOCOL_DESCRIPTION", DbType.String));

            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("PAYMENT_AMOUNT", DbType.Decimal));
            this.Database.AddRefColumn("GKH_MORG_CONTRACT_JSKTSJ", new RefColumn("PAYMENT_PROTOCOL_FILE", "GKH_MORG_CJT_PAY_PROT", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GKH_MORG_CONTRACT_JSKTSJ", new Column("PAYMENT_PROTOCOL_DESCRIPTION", DbType.String));

            this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("COMP_REQIRED_PAY_AMOUNT", DbType.Decimal));
            this.Database.AddRefColumn("GKH_MORG_JSKTSJ_CONTRACT", new RefColumn("COMP_PAYMENT_PROTOCOL", "GKH_MORG_CTJ_COMP_PAY_PROT", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("COMP_PAY_PROTOCOL_DESCR", DbType.String));
            this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("REQIRED_PAY_AMOUNT", DbType.Decimal));
            this.Database.AddRefColumn("GKH_MORG_JSKTSJ_CONTRACT", new RefColumn("PAYMENT_PROTOCOL", "GKH_MORG_CTJ_PAY_PROT", "B4_FILE_INFO", "ID"));
            this.Database.AddColumn("GKH_MORG_JSKTSJ_CONTRACT", new Column("PAY_PROTOCOL_DESCR", DbType.String));

        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "START_DATE_PAYMENT_PERIOD");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT", "END_DATE_PAYMENT_PERIOD");

            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PAYMENT_AMOUNT");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PAYMENT_PROTOCOL_FILE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_OWNERS", "PAYMENT_PROTOCOL_DESCRIPTION");

            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PAYMENT_AMOUNT");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PAYMENT_PROTOCOL_FILE");
            this.Database.RemoveColumn("GKH_MORG_CONTRACT_JSKTSJ", "PAYMENT_PROTOCOL_DESCRIPTION");

            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "COMP_REQIRED_PAY_AMOUNT");
            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "COMP_PAYMENT_PROTOCOL");
            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "COMP_PAY_PROTOCOL_DESCR");
            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "REQIRED_PAY_AMOUNT");
            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "PAYMENT_PROTOCOL");
            this.Database.RemoveColumn("GKH_MORG_JSKTSJ_CONTRACT", "PAY_PROTOCOL_DESCR");
        }
    }
}
