namespace Bars.GkhGji.Regions.Tatarstan.Migration._2020.Version_2020041700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2020041700")]
    [MigrationDependsOn(typeof(Version_2020041400.UpdateSchema))]

    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "DOCUMENT_GJI_ID", "DOCUMENT_ID");
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "EYEWITNESS_TYPE", "WITNESS_TYPE");
            this.Database.RenameTable("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "GJI_DOCUMENT_WITNESS");

            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW", "ARTICLE_LOW_ID", "ARTICLE_LAW_ID");
            this.Database.RenameTable("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW", "GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LAW");


            this.Database.ExecuteNonQuery(@"INSERT INTO GJI_DOCUMENT_INSPECTOR (object_version, object_create_date, object_edit_date, document_id, inspector_id, inspector_order)
                                        select object_version, object_create_date, object_edit_date, tatarstan_protocol_gji_id, inspector_id, (row_number() over (partition by tatarstan_protocol_gji_id))-1
                                        from GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR;");

            this.Database.RemoveTable("GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR");

  
        }

        public override void Down()
        {
            this.Database.RenameTable("GJI_DOCUMENT_WITNESS", "GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS");
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "DOCUMENT_ID", "DOCUMENT_GJI_ID");
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_EYEWITNESS", "WITNESS_TYPE", "EYEWITNESS_TYPE");
          

            this.Database.RenameTable("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LAW", "GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW");
            this.Database.RenameColumn("GJI_TATARSTAN_PROTOCOL_GJI_ARTICLE_LOW", "ARTICLE_LAW_ID", "ARTICLE_LOW_ID");

            this.Database.AddEntityTable("GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR",
                new RefColumn("TATARSTAN_PROTOCOL_GJI_ID",
                    "GJI_TATARSTAN_PROTOCOL_GJI_INPECTOR_PROTOCOL",
                    "GJI_TATARSTAN_PROTOCOL_GJI",
                    "ID"),
                new RefColumn("INSPECTOR_ID", "GJI_TATARSTAN_PROTOCOL_GJI_INPECTOR_INSPECTOR", "GKH_DICT_INSPECTOR", "ID")
            );

            this.Database.ExecuteNonQuery(@"INSERT INTO GJI_TATARSTAN_PROTOCOL_GJI_INSPECTOR (object_version, object_create_date, object_edit_date, document_id, inspector_id)
                                        select object_version, object_create_date, object_edit_date, document_id, inspector_id
                                        from GJI_DOCUMENT_INSPECTOR i 
                                        join GJI_TATARSTAN_PROTOCOL_GJI p on p.id = document_id;

                                        DELETE FROM GJI_DOCUMENT_INSPECTOR i WHERE EXISTS (SELECT 1 FROM GJI_TATARSTAN_PROTOCOL_GJI p WHERE p.id = i.document_id); ");

        }
    }
}
