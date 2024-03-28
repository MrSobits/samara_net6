namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022110800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2022110800")]
    [MigrationDependsOn(typeof(Version_2022103100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ExecuteNonQuery("UPDATE b4_state SET type_id = 'gji_tat_protocol' where type_id = 'gji_document_prot'");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ExecuteNonQuery("UPDATE b4_state SET type_id = 'gji_document_prot' where type_id = 'gji_tat_protocol'");
        }
    }
}