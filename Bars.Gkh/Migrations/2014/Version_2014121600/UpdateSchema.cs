namespace Bars.Gkh.Migrations.Version_2014121600
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations.Version_2014121200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_DICT_NORMATIVE_DOC", new Column("FULLNAME", DbType.String, 1000));

            Database.ExecuteNonQuery(@"update GKH_DICT_NORMATIVE_DOC set FULLNAME = NAME");
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_DICT_NORMATIVE_DOC", "FULLNAME");
        }
    }
}