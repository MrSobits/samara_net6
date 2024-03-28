namespace Bars.GkhGji.Migrations._2018.Version_2018120400
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018120400")]
    [MigrationDependsOn(typeof(Bars.GkhGji.Migrations._2018.Version_2018100900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
	    public override void Up()
        {

            //-----Добавляем владельца спецсчета
            Database.AddEntityTable(
                "GJI_SPECIAL_ACCOUNT_OWNER",
                new Column("TERM_REASON", DbType.Int32, 4, ColumnProperty.None),
                new Column("DESCRIPTION", DbType.String, 5000),
                new Column("STATE_ROLE", DbType.Int32, 4, ColumnProperty.None),
                new RefColumn("CONTRAGENT_ID", ColumnProperty.None, "GJI_SPECACCOWNER_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
            new Column("ACTIVITY_DATE_END", DbType.DateTime, ColumnProperty.None));
            
            //-----Дом владельца спецсчета
            Database.AddEntityTable(
                "GJI_SPEC_ACC_OWNER_RO",
                new Column("DATE_END", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
                new Column("ACC_NUM", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("OWNER_ID", ColumnProperty.None, "GJI_SPEC_ACC_OWNER_RO_SPOWNER", "GJI_SPECIAL_ACCOUNT_OWNER", "ID"),
                new RefColumn("RO_ID", ColumnProperty.None, "GJI_SPEC_ACC_OWNER_RO_REAL_OBJ", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("BANK_ID", ColumnProperty.None, "GJI_SPEC_ACC_OWNER_RO_BANK", "OVRHL_CREDIT_ORG", "ID"));
            



        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
            Database.RemoveTable("GJI_SPEC_ACC_OWNER_RO");
            Database.RemoveTable("GJI_SPECIAL_ACCOUNT_OWNER");

        }

    }
}