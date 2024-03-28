namespace Bars.Gkh.RegOperator.Migrations._2017.Version_2017103000
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Исправление миграции <see cref="Version_2017102000.UpdateSchema"/>
    /// </summary>
    [Migration("2017103000")]
    [MigrationDependsOn(typeof(Version_2017102000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            var sql = @"-- Удаленние детальной информации документов ПИР отличных от претензии, ЗВСП, ИЗ
DELETE FROM CLW_DOCUMENT_ACC_DETAIL
WHERE ID IN (
    SELECT det.ID FROM CLW_DOCUMENT_ACC_DETAIL det
    JOIN CLW_DOCUMENT doc ON doc.ID = det.DOCUMENT_ID
    WHERE doc.TYPE_DOCUMENT NOT IN (20, 30, 60) -- Bars.Gkh.Modules.ClaimWork.Enums.ClaimWorkDocumentType
);
";
            this.Database.ExecuteNonQuery(sql);
        }

        /// <inheritdoc />
        public override void Down()
        {
        }
    }
}