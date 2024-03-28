namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014102901
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014102901")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014102900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERSACC_SERVTYPE",
                new Column("CODE", DbType.String),
                new Column("NAME", DbType.String));

            Database.AddRefColumn("REGOP_PERS_ACC", new RefColumn("SERV_TYPE_ID", "PERS_ACC_SERV_TYPE", "REGOP_PERSACC_SERVTYPE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("REGOP_PERS_ACC", "SERV_TYPE_ID");

            Database.RemoveTable("REGOP_PERSACC_SERVTYPE");
        }
    }
}
