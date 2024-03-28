namespace Bars.GisIntegration.RegOp.Migrations.Version_2016100600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция
    /// </summary>
    [Migration("2016100600")]
    [MigrationDependsOn(typeof(Version_2016100500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Применить миграцию
        /// </summary>
        public override void Up()
        {
            //if (this.Database.ColumnExists("RIS_IND", "SURNAME"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("SURNAME", DbType.String, 100));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "FIRSTNAME"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("FIRSTNAME", DbType.String, 100));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "PATRONYMIC"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("PATRONYMIC", DbType.String, 100));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "IDSERIES"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("IDSERIES", DbType.String, 200));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "IDNUMBER"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("IDNUMBER", DbType.String, 200));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "PLACEBIRTH"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("PLACEBIRTH", DbType.String, 300));
            //}
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
        public override void Down()
        {
            //if (this.Database.ColumnExists("RIS_IND", "SURNAME"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("SURNAME", DbType.String, 50));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "FIRSTNAME"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("FIRSTNAME", DbType.String, 50));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "PATRONYMIC"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("PATRONYMIC", DbType.String, 50));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "IDSERIES"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("IDSERIES", DbType.String, 50));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "IDNUMBER"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("IDNUMBER", DbType.String, 50));
            //}
            //if (this.Database.ColumnExists("RIS_IND", "PLACEBIRTH"))
            //{
            //    this.Database.ChangeColumn("RIS_IND", new Column("PLACEBIRTH", DbType.String, 50));
            //}
        }
    }
}