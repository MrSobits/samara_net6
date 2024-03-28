namespace Bars.Gkh.RegOperator.Migrations._2020.Version_2020051900
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020051900")]
   
    [MigrationDependsOn(typeof(_2020.Version_2020042800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_CODE", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_NAME", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_SERIAL", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_NUMBER", DbType.String, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_DATE", DbType.DateTime, ColumnProperty.Null));
            this.Database.AddColumn("REGOP_LAWSUIT_IND_OWNER_INFO", new Column("DOC_IND_ISSUE", DbType.String, ColumnProperty.Null));
        }

        public override void Down()
        {
            // this.Database.RemoveColumn(egrnTable, nameof(ExtractEgrn.ExtractNumber).ToLower());
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_CODE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_NAME");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_SERIAL");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_NUMBER");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_DATE");
            this.Database.RemoveColumn("REGOP_LAWSUIT_IND_OWNER_INFO", "DOC_IND_ISSUE");
        }
    }
}