namespace Bars.Gkh.Ris.Migrations.Version_2016102601
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016102601")]
    [MigrationDependsOn(typeof(Version_2016102000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //this.Database.AddRisTable( "RIS_LIFT",
            //    new RefColumn("APARTAMENT_HOUSE", "RIS_LIFT_HOUSE", "RIS_HOUSE", "ID"),
            //    new Column("ENTRANCE_NUM", DbType.String),
            //    new Column("FACTORY_NUM", DbType.String),
            //    new Column("OPERATING_LIMIT", DbType.String),
            //    new Column("TERMINATION_DATE", DbType.DateTime),
            //    new Column("OGF_DATA_CODE", DbType.String),
            //    new Column("OGF_DATA_VALUE", DbType.String),
            //    new Column("TYPE_CODE", DbType.String),
            //    new Column("TYPE_GUID", DbType.String));

            //this.Database.AddColumn("RIS_HOUSE", new Column("CONDITIONAL_NUMBER", DbType.String));
            //this.Database.AddColumn("RIS_HOUSE", new Column("VALUE", DbType.String));
            //this.Database.AddColumn("RIS_HOUSE", new Column("CODE", DbType.String));
            //this.Database.AddColumn("RIS_HOUSE", new Column("TYPE", DbType.String));
            //this.Database.AddColumn("RIS_HOUSE", new Column("REG_NUMBER", DbType.String));
            //this.Database.AddColumn("RIS_HOUSE", new Column("REG_DATE", DbType.DateTime));

            
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //this.Database.RemoveTable("RIS_LIFT");
            //this.Database.RemoveColumn("RIS_HOUSE", "CONDITIONAL_NUMBER");
            //this.Database.RemoveColumn("RIS_HOUSE", "VALUE");
            //this.Database.RemoveColumn("RIS_HOUSE", "CODE");
            //this.Database.RemoveColumn("RIS_HOUSE", "TYPE");
            //this.Database.RemoveColumn("RIS_HOUSE", "REG_NUMBER");
            //this.Database.RemoveColumn("RIS_HOUSE", "REG_DATE");

        }
    }
}