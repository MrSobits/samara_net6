namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020050800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2020050800")]
    [MigrationDependsOn(typeof(Version_2020042300.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            var query = @"drop table if exists tmp_doc_type;
                        create temp table tmp_doc_type as
                          select id from GKH_DICT_IDENTITY_DOC_TYPE where code = '01';
                        
                        update gji_tatarstan_protocol_gji set identity_document_id = (select id from tmp_doc_type)
                        where citizenship_type = 10 and identity_document_id is null;

                        update gji_tatarstan_resolution_gji set identity_document_id = (select id from tmp_doc_type)
                        where citizenship_type = 10 and identity_document_id is null;";

            this.Database.ExecuteNonQuery(query);
        }
    }
}