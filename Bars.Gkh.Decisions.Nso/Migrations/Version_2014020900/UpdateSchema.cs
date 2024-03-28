namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2014020900
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Decisions.Nso.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("DEC_JOB_YEAR", 
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("JOB_YEARS", DbType.String, 20000, ColumnProperty.Null));
            Database.AddForeignKey("FK_DEC_JOB_YEAR_ULT", "DEC_JOB_YEAR", "ID", "DEC_ULTIMATE_DECISION", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("DEC_JOB_YEAR");
        }
    }
}