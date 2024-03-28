namespace Bars.Gkh.Decisions.Nso.Migrations.Version_2016122100
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2016122100")]
    [MigrationDependsOn(typeof(Version_2016022400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "DEC_BASE_PROTOCOL",
                new Column("PROTOCOL_NUM", DbType.String, 250),
                new Column("PROTOCOL_DATE", DbType.DateTime),
                new Column("DATE_START", DbType.DateTime),
                new Column("DESCRIPTION", DbType.String, 250),

                new RefColumn("STATE_ID", "GKH_CRFUND_D_PROT_STATE", "B4_STATE", "ID"),
                new RefColumn("RO_ID", ColumnProperty.NotNull, "GKH_CRFUND_D_PROT_RO", "GKH_REALITY_OBJECT", "ID"),
                new RefColumn("PROT_FILE_ID", ColumnProperty.Null, "GKH_CRFUND_D_PROT_FILE", "B4_FILE_INFO", "ID")
                );

            this.Database.AddTable(
                "DEC_CRFUND_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_DEC_CRFUND_PROTOCOL_ID", "DEC_CRFUND_PROTOCOL", "ID", "DEC_BASE_PROTOCOL", "ID");
            this.Database.AddIndex("IDX_DEC_CRFUND_PROTOCOL_ID", true, "DEC_CRFUND_PROTOCOL", "ID");

            this.Database.AddTable(
                "DEC_MKDORG_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_DEC_MKDORG_PROTOCOL_ID", "DEC_MKDORG_PROTOCOL", "ID", "DEC_BASE_PROTOCOL", "ID");
            this.Database.AddIndex("IDX_DEC_MKDORG_PROTOCOL_ID", true, "DEC_MKDORG_PROTOCOL", "ID");

            this.Database.AddTable(
                "DEC_MKDMANAGE_TYPE_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_DEC_MKDMANAGE_TYPE_PROTOCOL_ID", "DEC_MKDMANAGE_TYPE_PROTOCOL", "ID", "DEC_BASE_PROTOCOL", "ID");
            this.Database.AddIndex("IDX_DEC_MKDMANAGE_TYPE_PROTOCOL_ID", true, "DEC_MKDMANAGE_TYPE_PROTOCOL", "ID");

            this.Database.AddTable(
                "DEC_OOI_MANAGE_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_DEC_OOI_MANAGE_PROTOCOL_ID", "DEC_OOI_MANAGE_PROTOCOL", "ID", "DEC_BASE_PROTOCOL", "ID");
            this.Database.AddIndex("IDX_DEC_OOI_MANAGE_PROTOCOL_ID", true, "DEC_OOI_MANAGE_PROTOCOL", "ID");

            this.Database.AddTable(
                "DEC_TARIFF_APPROVAL_PROTOCOL",
                new Column("ID", DbType.Int64, 22, ColumnProperty.NotNull | ColumnProperty.Unique));

            this.Database.AddForeignKey("FK_DEC_TARIFF_APPROVAL_PROTOCOL_ID", "DEC_TARIFF_APPROVAL_PROTOCOL", "ID", "DEC_BASE_PROTOCOL", "ID");
            this.Database.AddIndex("IDX_DEC_TARIFF_APPROVAL_PROTOCOL_ID", true, "DEC_TARIFF_APPROVAL_PROTOCOL", "ID");
        }

        public override void Down()
        {
            this.Database.RemoveTable("DEC_CRFUND_PROTOCOL");
            this.Database.RemoveTable("DEC_MKDORG_PROTOCOL");
            this.Database.RemoveTable("DEC_MKDMANAGE_TYPE_PROTOCOL");
            this.Database.RemoveTable("DEC_OOI_MANAGE_PROTOCOL");
            this.Database.RemoveTable("DEC_TARIFF_APPROVAL_PROTOCOL");

            this.Database.RemoveTable("DEC_BASE_PROTOCOL");
        }
    }
}