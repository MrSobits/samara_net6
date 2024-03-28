namespace Bars.GkhGji.Regions.Voronezh.Migrations.Version_2021033101
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Collections.Generic;
    using System.Data;

    [Migration("2021033101")]
    [MigrationDependsOn(typeof(Version_2021033100.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
              "GJI_CH_SMEV_NDFL_ANSWER",
              new Column("INNUL", DbType.String, 50),
              new Column("KPP", DbType.String, 50),
              new Column("ORG_NAME", DbType.String, 500),
              new Column("RATE", DbType.Int32, ColumnProperty.Null),
              new Column("REVENUE_CODE", DbType.String, 50),
              new Column("MONTH", DbType.String, 50),
              new Column("REVENUE_SUM", DbType.Decimal, ColumnProperty.Null),
              new Column("RECOUPMENT_CODE", DbType.String, 50),
              new Column("RECOUPMENT_SUM", DbType.Decimal, ColumnProperty.Null),
              new Column("DUTY_BASE", DbType.Decimal, ColumnProperty.Null),
              new Column("DUTY_SUM", DbType.Decimal, ColumnProperty.Null),
              new Column("UNRETENTION_SUM", DbType.Decimal, ColumnProperty.Null),
              new Column("REVENUE_TOTAL_SUM", DbType.Decimal, ColumnProperty.Null),
              new RefColumn("SMEV_NDFL_ID", ColumnProperty.None, "GJI_SMEV_NDFL_ANSW_ID", "GJI_CH_SMEV_NDFL", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_CH_SMEV_NDFL_ANSWER");
        }


    }
}
