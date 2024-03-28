namespace Bars.Gkh.Migrations._2015.Version_2015060100
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015060100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations._2015.Version_2015052900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /*
         * колонки с именем "NUMBER" и "DATE" не допустимы в бд ORACLE
         * попутно изменена миграция Bars.Gkh.Migration.Version_2015050800 
         * в которой создавались колонки
         */

        public override void Up()
        {
            if (Database.ColumnExists("CLW_RESTR_DEBT_AMIC_AGR", "NUMBER"))
                Database.RenameColumn("CLW_RESTR_DEBT_AMIC_AGR", "NUMBER", "DOC_NUM");
            if (Database.ColumnExists("CLW_RESTR_DEBT_AMIC_AGR", "DATE"))
                Database.RenameColumn("CLW_RESTR_DEBT_AMIC_AGR", "DATE", "DOC_DATE");
        }

        public override void Down()
        {
            if (Database.ColumnExists("CLW_RESTR_DEBT_AMIC_AGR", "DOC_NUM"))
                Database.RenameColumn("CLW_RESTR_DEBT_AMIC_AGR", "DOC_NUM", "NUMBER");
            if (Database.ColumnExists("CLW_RESTR_DEBT_AMIC_AGR", "DOC_DATE"))
                Database.RenameColumn("CLW_RESTR_DEBT_AMIC_AGR", "DOC_DATE", "DATE");
        }
    }
}