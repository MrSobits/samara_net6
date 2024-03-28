namespace Bars.Gkh.RegOperator.Migrations._2023.Version_2023101800
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    [Migration("2023101800")]

    [MigrationDependsOn(typeof(Version_2023100900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC", new Column("DIGITAL_RECEIPT", DbType.Int16, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("REGOP_PERS_ACC_DTO", new Column("DIGITAL_RECEIPT", DbType.Int16, ColumnProperty.NotNull, 20));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_DTO", "DIGITAL_RECEIPT");
            this.Database.RemoveColumn("REGOP_PERS_ACC", "DIGITAL_RECEIPT");
        }      
    }
}
