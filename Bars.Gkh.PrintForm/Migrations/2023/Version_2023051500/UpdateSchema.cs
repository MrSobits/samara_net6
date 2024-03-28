namespace Bars.Gkh.PrintForm.Migrations._2023.Version_2023051500
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2023051500")]
    [MigrationDependsOn(typeof(_2022.Version_2022112200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string TableName = "PDF_SIGN_INFO";
        private const string OldColumn = "ORIGINAL_PDF_ID";
        private const string NewColumn = "ORIGINAL_FILE_ID";
        
        public override void Up()
        {
            this.Database.RenameColumn(TableName, OldColumn, NewColumn);
        }

        public override void Down()
        {
            this.Database.RenameColumn(TableName, NewColumn, OldColumn);
        }
    }
}