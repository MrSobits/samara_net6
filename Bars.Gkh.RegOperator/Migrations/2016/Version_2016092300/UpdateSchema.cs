namespace Bars.Gkh.RegOperator.Migrations._2016.Version_2016092300
{
    using System.Data;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Bars.B4.Modules.Ecm7.Framework;
    using B4.Utils;

    /// <summary>
    /// Миграция RegOperator 2016092300
    /// </summary>
    [Migration("2016092300")]
    [MigrationDependsOn(typeof(Version_2016091001.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Вверх
        /// </summary>
        public override void Up()
        {
            this.Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("DATE_DOCUMENT_ISSUANCE", DbType.DateTime));
            this.Database.AddColumn("REGOP_INDIVIDUAL_ACC_OWN", new Column("GENDER", DbType.Int32));
            this.Database.ExecuteNonQuery("update REGOP_INDIVIDUAL_ACC_OWN set GENDER =  0");
        }
        /// <summary>
        /// Вниз
        /// </summary>
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "DATE_DOCUMENT_ISSUANCE");
            this.Database.RemoveColumn("REGOP_INDIVIDUAL_ACC_OWN", "GENDER");
        }
    }
}
