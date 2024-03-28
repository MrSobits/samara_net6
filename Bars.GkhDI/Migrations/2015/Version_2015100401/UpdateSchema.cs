namespace Bars.GkhDi.Migrations.Version_2015100401
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015100401")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhDi.Migrations._2015.Version_2015100400.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("DI_DISINFO_DOC_PROT", new Column("DOC_NUM", DbType.String, 250));
            Database.AddColumn("DI_DISINFO_DOC_PROT", new Column("DOC_DATE", DbType.DateTime));

            Database.AddColumn("DI_DISINFO_DOC_RO", new Column("HAS_GEN_MEET_OWNERS", DbType.Int32, ColumnProperty.NotNull, 30));
        }

        public override void Down()
        {
            Database.RemoveColumn("DI_DISINFO_DOC_RO", "HAS_GEN_MEET_OWNERS");
            Database.RemoveColumn("DI_DISINFO_DOC_PROT", "DOC_NUM");
            Database.RemoveColumn("DI_DISINFO_DOC_PROT", "DOC_DATE");
        }
    }
}