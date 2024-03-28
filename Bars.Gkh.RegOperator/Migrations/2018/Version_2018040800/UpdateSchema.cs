namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018040800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2018040800")]
   
    [MigrationDependsOn(typeof(Version_2018032300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc/>
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_LAWSUIT_REFERENCE_CALCULATION",
              new Column("ACC_NUM", DbType.String, ColumnProperty.NotNull),
              new Column("AREA_SHARE", DbType.Decimal, ColumnProperty.NotNull),
              new Column("BASE_TARIF", DbType.Decimal, ColumnProperty.NotNull),
              new Column("PERIOD_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("ACCOUNT_ID", DbType.Int64, ColumnProperty.NotNull),
              new Column("ROOM_AREA", DbType.Decimal, ColumnProperty.NotNull),
              new Column("TARIF_DEBT", DbType.Decimal, ColumnProperty.None),
              new Column("TARIF_CHARGED", DbType.Decimal, ColumnProperty.None),
              new Column("TARIF_PAYMENTS", DbType.Decimal, ColumnProperty.None),
              new RefColumn("LAWSUIT_ID", ColumnProperty.NotNull, "LAWSUIT_REFERENCE_CALCULATION_LAWSUIT", "CLW_LAWSUIT", "ID")
          );
        }
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_LAWSUIT_REFERENCE_CALCULATION");
        }
    }
}