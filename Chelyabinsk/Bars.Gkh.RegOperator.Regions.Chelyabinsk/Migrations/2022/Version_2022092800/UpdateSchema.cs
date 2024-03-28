using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2022.Version_2022092800
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022092800")]
    [MigrationDependsOn(typeof(_2022.Version_2022071300.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("USE_CUSTOM_DATE", DbType.Boolean));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("CUSTOM_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("START_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("END_DATE", DbType.DateTime, ColumnProperty.None));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("BT_DEBT", DbType.Decimal, ColumnProperty.None));
            Database.AddColumn("AGENT_PIR_DEBTOR", new Column("PENALTY_DEBT", DbType.Decimal, ColumnProperty.None));

            this.Database.AddEntityTable("AGENT_PIR_DEBTOR_REFERENCE_CALCULATION",
              new Column("ACC_NUM", DbType.String, ColumnProperty.NotNull),
              new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull),
              new Column("BASE_TARIF", DbType.Decimal, ColumnProperty.NotNull),
              new Column("PERIOD_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("ACCOUNT_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("ROOM_AREA", DbType.Decimal, ColumnProperty.NotNull),
              new Column("TARIF_DEBT", DbType.Decimal, ColumnProperty.NotNull),
              new Column("TARIF_CHARGED", DbType.Decimal, ColumnProperty.NotNull),
              new Column("TARIF_PAYMENTS", DbType.Decimal, ColumnProperty.NotNull),
              new Column("PAYMENT_DATE", DbType.String),
              new Column("PENALTIES", DbType.Decimal, ColumnProperty.None),
              new Column("DESCRIPTION", DbType.String,500),
              new Column("ACCRUAL_PENALTIES", DbType.String, 1500, ColumnProperty.None),
              new Column("PENALTY_PAYMENT", DbType.Decimal, ColumnProperty.None),
              new Column("PENALTY_PAYMENT_DATE", DbType.String, ColumnProperty.None),
              new Column("ACCRUAL_PENALTIES_FORM", DbType.String, ColumnProperty.None),
              new RefColumn("DEBTOR_ID", ColumnProperty.NotNull, "AGENT_PIR_REFCALCULATION_DEBTOR", "AGENT_PIR_DEBTOR", "ID")
          );

        }

        public override void Down()
        {
            this.Database.RemoveTable("AGENT_PIR_DEBTOR_REFERENCE_CALCULATION");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "PENALTY_DEBT");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "END_DATE");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "START_DATE");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "BT_DEBT");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "USE_CUSTOM_DATE");
            this.Database.RemoveColumn("AGENT_PIR_DEBTOR", "CUSTOM_DATE");
        }
    }
}