namespace Bars.Gkh.Migrations._2019.Version_2019121600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019121600")]
    
    [MigrationDependsOn(typeof(Version_2019120500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            Database.AddColumn("CLW_PRETENSION", new Column("PRETENSION_NUMBER", DbType.String, 36));
            Database.AddColumn("CLW_LAWSUIT", new Column("PAY_DOC_NUMBER", DbType.String, 36));
            Database.AddColumn("CLW_LAWSUIT", new Column("PAY_DOC_DATE", DbType.Date));
            Database.AddColumn("CLW_LAWSUIT", new Column("DUTY_PAYED", DbType.Decimal));
            Database.AddColumn("CLW_LAWSUIT", new Column("MONEY_LESS", DbType.Boolean));




        }

        public override void Down()
        {
            Database.RemoveColumn("CLW_PRETENSION", "PRETENSION_NUMBER");
            Database.RemoveColumn("CLW_LAWSUIT", "PAY_DOC_DATE");
            Database.RemoveColumn("CLW_LAWSUIT", "PAY_DOC_NUMBER");
            Database.RemoveColumn("CLW_LAWSUIT", "DUTY_PAYED");
            Database.RemoveColumn("CLW_LAWSUIT", "MONEY_LESS");

        }
    }
}