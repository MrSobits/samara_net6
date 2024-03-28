namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017052200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using ForeignKeyConstraint = Bars.B4.Modules.Ecm7.Framework.ForeignKeyConstraint;

    /// <summary>
    /// Миграция RegOperator 2017052200
    /// </summary>
    [Migration("2017052200")]
    [MigrationDependsOn(typeof(Version_2017042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("REGOP_PERS_ACC_OWNER_INFO", new Column("FILE_ID", DbType.Int64, 22));
            Database.AddIndex("IND_REGOP_PERS_ACC_OWNER_INFO_FILE", false, "REGOP_PERS_ACC_OWNER_INFO", "FILE_ID");
            Database.AddForeignKey("FK_REGOP_PERS_ACC_OWNER_INFO_FILE", "REGOP_PERS_ACC_OWNER_INFO", "FILE_ID", "B4_FILE_INFO", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveColumn("REGOP_PERS_ACC_OWNER_INFO", "FILE_ID");
        }
    }
}