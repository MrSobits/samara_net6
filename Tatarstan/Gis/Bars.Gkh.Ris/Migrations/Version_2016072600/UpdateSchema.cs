namespace Bars.Gkh.Ris.Migrations.Version_2016072600
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016072600")]
    [MigrationDependsOn(typeof(Version_2016070800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisTable("RIS_BLOCK",
            //    new RefColumn("HOUSE_ID", "RIS_BLOCK_HOUSE_ID", "RIS_HOUSE", "ID"),
            //    new Column("CADASTRALNUMBER", DbType.String),
            //    new Column("BLOCKNUM", DbType.String),
            //    new Column("PREMISESCHARACTERISTICCODE", DbType.String),
            //    new Column("PREMISESCHARACTERISTICGUID", DbType.String),
            //    new Column("TOTALAREA", DbType.Decimal),
            //    new Column("GROSSAREA", DbType.Decimal),
            //    new Column("TERMINATIONDATE", DbType.Date));

            //this.Database.AddRefColumn("RIS_LIVINGROOM", new RefColumn("BLOCK_ID", "RIS_LIVINGROOM_BLOCK", "RIS_BLOCK", "ID"));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_LIVINGROOM", "BLOCK_ID");
            //this.Database.RemoveTable("RIS_BLOCK");
        }
    }
}