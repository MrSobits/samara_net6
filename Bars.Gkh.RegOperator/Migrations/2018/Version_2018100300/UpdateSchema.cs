namespace Bars.Gkh.RegOperator.Migrations._2018.Version_2018100300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System;
    using System.Data;

    [Migration("2018100300")]
   
    [MigrationDependsOn(typeof(Version_2018072600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("REGOP_DICT_CLW_TARIF_BY_PERIOD",
              new Column("NAME", DbType.String),
              new Column("VALUE", DbType.Decimal, ColumnProperty.NotNull),
              new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "FK_REGOP_DICT_CLW_TBP_PERIOD", "REGOP_PERIOD", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.None, "FK_REGOP_DICT_CLW_TBP_MUNICIPALITY", "GKH_DICT_MUNICIPALITY", "ID")
          );
        }
        public override void Down()
        {
            this.Database.RemoveTable("REGOP_DICT_CLW_TARIF_BY_PERIOD");
        }


    }
}