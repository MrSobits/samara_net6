namespace Bars.GkhGji.Migrations.Version_2013041000
{
    using Bars.Gkh;
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013041000")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2013040700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            
            Database.AddRefColumn("GJI_BUISNES_NOTIF", new RefColumn("STATE_ID", "GJI_BUIS_NOTIF_ST", "B4_FILE_INFO", "ID"));

            if (!Database.ColumnExists("GJI_DICT_EXPERT", "EXTERNAL_ID"))
            {
                Database.AddColumn("GJI_DICT_EXPERT", new Column("EXTERNAL_ID", DbType.String, 36));
            }

            Database.ChangeColumn("GJI_ACT_TSJ_ARTICLE", new Column("PARAGRAPH", DbType.String, 1000));

            Database.ChangeColumn("gji_document", new Column("document_number", DbType.String, 300));
            Database.ChangeColumn("GJI_DOCUMENT", new Column("STATE_ID", DbType.Int64, 22));

            Database.ChangeColumn("gji_disposal", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_disposal_annex", new Column("NAME", DbType.String, 300));
            Database.ChangeColumn("gji_disposal_annex", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("gji_prescription", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_prescription_annex", new Column("NAME", DbType.String, 300));
            Database.ChangeColumn("gji_prescription_annex", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("GJI_PRESCRIPTION_ARTLAW", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_prescription_cancel", new Column("reason", DbType.String, 2000));
            Database.ChangeColumn("gji_prescription_cancel", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("gji_protocol", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_protocol_annex", new Column("NAME", DbType.String, 300));
            Database.ChangeColumn("gji_protocol_annex", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("GJI_PROTOCOL_ARTLAW", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_protocol_definition", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_protocol_definition", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("gji_resolution_payfine", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("GJI_ACTCHECK", new Column("FLAT", DbType.String, 250));
            Database.ChangeColumn("gji_actcheck_annex", new Column("NAME", DbType.String, 300));
            Database.ChangeColumn("gji_actcheck_annex", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_actcheck_definition", new Column("description", DbType.String, 2000));
            Database.ChangeColumn("gji_actcheck_definition", new Column("document_num", DbType.String, 300));

            Database.ChangeColumn("GJI_ACTREMOVAL", new Column("FLAT", DbType.String, 250));
            Database.ChangeColumn("GJI_ACTREMOVAL", new Column("description", DbType.String, 2000));

            Database.ChangeColumn("GJI_RESOLUTION", new Column("PHYSICAL_PERSON", DbType.String, 500));
            Database.ChangeColumn("GJI_RESOLUTION", new Column("SECTOR_NUMBER", DbType.String, 250));
            Database.ChangeColumn("gji_resolution_annex", new Column("NAME", DbType.String, 300));

            Database.ChangeColumn("GJI_PRESENTATION", new Column("PHYSICAL_PERSON", DbType.String, 500));
            Database.ChangeColumn("gji_presentation_annex", new Column("NAME", DbType.String, 300));
            
            Database.ChangeColumn("gji_actsurvey_annex", new Column("NAME", DbType.String, 300));

            Database.ChangeColumn("gji_resolpros_annex", new Column("NAME", DbType.String, 300));
        }

        public override void Down()
        {           
            Database.RemoveColumn("GJI_BUISNES_NOTIF", "STATE_ID");
        }
    }
}