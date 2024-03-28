namespace Bars.Gkh.Repair.Migration.Version_2015121500
{
    using System.Data;
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2015121500")]
    [MigrationDependsOn(typeof(Migrations.Version_2014030600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.ColumnExists("RP_OBJECT", "REASON_DOCUMENT_ID"))
            {
                this.Database.AddRefColumn("RP_OBJECT", new RefColumn("REASON_DOCUMENT_ID", "RP_OBJECT_DOC", "B4_FILE_INFO", "ID"));
            }

            if (!this.Database.ColumnExists("RP_OBJECT", "COMMENT"))
            {
                this.Database.AddColumn("RP_OBJECT", new Column("COMMENT", DbType.String, 2000));
            }
        }

        public override void Down()
        {
            if (this.Database.ColumnExists("RP_OBJECT", "REASON_DOCUMENT_ID"))
            {
                this.Database.RemoveColumn("RP_OBJECT", "REASON_DOCUMENT_ID");
            }

            if (this.Database.ColumnExists("RP_OBJECT", "COMMENT"))
            {
                this.Database.RemoveColumn("RP_OBJECT", "COMMENT");
            }
        }
    }
}
