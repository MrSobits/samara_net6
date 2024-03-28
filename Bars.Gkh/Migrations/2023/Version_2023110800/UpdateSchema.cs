namespace Bars.Gkh.Migrations._2023.Version_2023110800
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2023110800")]
    [MigrationDependsOn(typeof(Version_2023050152.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.ExecuteNonQuery(@"
                UPDATE gkh_contragent
                    SET parent_id = NULL
                WHERE id = parent_id;

                ALTER TABLE gkh_contragent ADD CONSTRAINT parent_id_check CHECK (parent_id != id);
            ");
        }
    }
}