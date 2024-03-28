namespace Bars.GkhGji.Migrations._2020.Version_2020070200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020070200")]
    [MigrationDependsOn(typeof(Version_2020062300.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_RESOLUTION", new Column("INLAW_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLUTION", new Column("DUE_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("GJI_RESOLUTION", new Column("PAYMENT_DATE", DbType.DateTime, ColumnProperty.None));

            Database.ExecuteNonQuery(@"update GJI_RESOLUTION set DUE_DATE = DELIVERY_DATE::date + integer '60' where PENALTY_AMOUNT>0 and DELIVERY_DATE is not null;
                update GJI_RESOLUTION set INLAW_DATE = DELIVERY_DATE::date + integer '10' where DELIVERY_DATE is not null;
                drop table if exists res_pay;
                create temp table res_pay as (
                select resolution_id, max(document_date) docdate from GJI_RESOLUTION_PAYFINE group by 1);
                UPDATE GJI_RESOLUTION SET PAYMENT_DATE = docdate
                FROM res_pay WHERE id = resolution_id;");
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_RESOLUTION", "PAYMENT_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "DUE_DATE");
            Database.RemoveColumn("GJI_RESOLUTION", "INLAW_DATE");
        }
    }
}