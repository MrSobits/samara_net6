namespace Bars.Gkh.RegOperator.Migrations._2014.Version_2014062700
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014062700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.RegOperator.Migrations._2014.Version_2014062600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("REGOP_PERIOD_PAY_DOC",
                new Column("DOCUMENT_CODE", DbType.String, 300, ColumnProperty.NotNull),
                new RefColumn("FILE_ID", ColumnProperty.NotNull, "PERIOD_PAY_DOC_F", "B4_FILE_INFO", "ID"),
                new RefColumn("PERIOD_ID", ColumnProperty.NotNull, "PERIOD_PAY_DOC_P", "REGOP_PERIOD", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("REGOP_PERIOD_PAY_DOC");
        }
    }
}
