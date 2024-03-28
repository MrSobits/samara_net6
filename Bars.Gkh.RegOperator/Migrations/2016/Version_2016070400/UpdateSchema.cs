namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016070400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016070400")]
    [MigrationDependsOn(typeof(Version_2016052600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
	        this.Database.AddEntityTable(
				"REGOP_PERS_ACC_OWNER_INFO",
		        new Column("DOCUMENT_NUMBER", DbType.Decimal, ColumnProperty.NotNull, 0m),
		        new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull, 0m),
		        new Column("START_DATE", DbType.DateTime, ColumnProperty.NotNull),
		        new Column("END_DATE", DbType.DateTime, ColumnProperty.NotNull),
		        new RefColumn("ACCOUNT_ID", ColumnProperty.NotNull, "REGOP_PERS_ACC_OWNER_INFO_PA", "REGOP_PERS_ACC", "ID"),
		        new RefColumn("ACCOUNT_OWNER_ID", ColumnProperty.NotNull, "REGOP_PERS_ACC_OWNER_INFO_PAO", "REGOP_PERS_ACC_OWNER", "ID")
		        );
        }

        public override void Down()
        {
			this.Database.RemoveTable("REGOP_PERS_ACC_OWNER_INFO");
        }
    }
}
