namespace Bars.Gkh.Migrations.Version_2015021200
{
    using System.Data;

    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015021200")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2015020900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                "CLW_DOCUMENT",
                new Column("CLAIMWORK_ID", DbType.Int64, 22, ColumnProperty.NotNull),
                new Column("STATE_ID", DbType.Int64, 22),
                new Column("TYPE_DOCUMENT", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("DOCUMENT_DATE", DbType.DateTime),
                new Column("DOCUMENT_NUMBER", DbType.String, 50),
                new Column("DOCUMENT_NUM", DbType.Int32));
            Database.AddIndex("IND_CLW_DOCUMENT_C", false, "CLW_DOCUMENT", "CLAIMWORK_ID");
            Database.AddIndex("IND_CLW_DOCUMENT_STT", false, "CLW_DOCUMENT", "STATE_ID");
            Database.AddForeignKey("FK_CLW_DOCUMENT_C", "CLW_DOCUMENT", "CLAIMWORK_ID", "CLW_CLAIM_WORK", "ID");
            Database.AddForeignKey("FK_CLW_DOCUMENT_STT", "CLW_DOCUMENT", "STATE_ID", "B4_STATE", "ID");
        }

        public override void Down()
        {
            Database.RemoveTable("CLW_DOCUMENT");
        }
    }
}