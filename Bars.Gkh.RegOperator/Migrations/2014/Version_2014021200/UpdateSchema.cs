namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014021200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014021100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("REGOP_PERS_ACC", new Column("CONTRACT_NUMBER", DbType.String));
            Database.AddColumn("REGOP_PERS_ACC", new Column("CONTRACT_NUMBER_DATE", DbType.DateTime));
            Database.AddColumn("REGOP_PERS_ACC", new RefColumn("CONTRACT_FILE_ID", "REGOP_PERSACCCONTR_FILE", "B4_FILE_INFO", "ID"));
            Database.AddColumn("REGOP_PERS_ACC", new Column("OWNERSHIP_DOC_TYPE", DbType.String));
            Database.AddColumn("REGOP_PERS_ACC", new Column("DOCUMENT_NUMBER", DbType.String));
            Database.AddColumn("REGOP_PERS_ACC", new Column("DOCUMENT_REG_DATE", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "CONTRACT_NUMBER");
            Database.RemoveColumn("REGOP_PERS_ACC", "CONTRACT_NUMBER_DATE");
            Database.RemoveColumn("REGOP_PERS_ACC", "CONTRACT_FILE_ID");
            Database.RemoveColumn("REGOP_PERS_ACC", "OWNERSHIP_DOC_TYPE");
            Database.RemoveColumn("REGOP_PERS_ACC", "DOCUMENT_NUMBER");
            Database.RemoveColumn("REGOP_PERS_ACC", "DOCUMENT_REG_DATE");
        }
    }
}
