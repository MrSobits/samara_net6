namespace Bars.Gkh.Overhaul.Tat.Migration.Version_2013121600
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013121600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Tat.Migration.Version_2013121200.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
           // Database.AddEntityTable("OVRHL_FUND_FORMAT_CONTR",
           //     new RefColumn("LONG_TERM_OBJ_ID", ColumnProperty.NotNull, "OV_FND_CTR_LONG_OBJ", "OVRHL_LONGTERM_PR_OBJECT", "ID"),
           //     new RefColumn("REG_OPER_ID", ColumnProperty.NotNull, "OV_FND_CTR_REG_OP", "OVRHL_REG_OPERATOR", "ID"),
           //     new Column("TYPE_CONTRACT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
           //     new Column("CONTRACT_NUMBER", DbType.String, 100),
           //     new Column("DATE_START", DbType.DateTime, ColumnProperty.NotNull),
           //     new Column("DATE_END", DbType.DateTime),
           //     new Column("CONTRACT_DATE", DbType.DateTime),
           //     new RefColumn("FILE_ID", "OV_FND_CTR_FILE", "B4_FILE_INFO", "ID")
           //);
        }

        public override void Down()
        {
            //Database.RemoveEntityTable("OVRHL_FUND_FORMAT_CONTR");
        }
    }
}