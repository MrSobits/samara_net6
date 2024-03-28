namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022071300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.GkhGji.Enums;

    [MigrationDependsOn(typeof(Version_2022070800.UpdateSchema))]
    [Migration("2022071300")]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery($@"
                UPDATE GJI_DOCUMENT
                SET    DOCUMENT_NUM = (regexp_matches(DOCUMENT_NUMBER, '[0-9]+\.?[0-9]*'))[1]::integer
                WHERE  DOCUMENT_NUMBER ~ '\w+-\d+.*' 
                       AND type_document = {(int)TypeDocumentGji.WarningDoc}
                       AND DOCUMENT_NUM ISNULL
            ");
        }
    }
}