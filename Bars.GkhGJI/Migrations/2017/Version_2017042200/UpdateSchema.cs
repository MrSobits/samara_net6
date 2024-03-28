namespace Bars.GkhGji.Migrations._2017.Version_2017042200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017042200")]
    [MigrationDependsOn(typeof(Version_2017042100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_DISPOSAL", new Column("TIME_VISIT_SART", DbType.DateTime, 25));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("TIME_VISIT_END", DbType.DateTime, 25));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_NUM", DbType.String, 100));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_NUM_LETTER", DbType.String, 100));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_DATE_LETTER", DbType.DateTime));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_OBTAINED", DbType.Int16, ColumnProperty.NotNull, 20));
            this.Database.AddColumn("GJI_DISPOSAL", new Column("NC_SENT", DbType.Int16, ColumnProperty.NotNull, 20));

            this.Database.AddColumn("GJI_ACTCHECK", new Column("DOCUMENT_PLACE", DbType.String, 1000));
            this.Database.AddColumn("GJI_ACTCHECK", new Column("DOCUMENT_TIME", DbType.DateTime, 25));

            this.Database.AddColumn("GJI_PROTOCOL", new Column("FORMAT_PLACE", DbType.String, 500, ColumnProperty.Null));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("FORMAT_HOUR", DbType.Int32, ColumnProperty.Null));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("FORMAT_MINUTE", DbType.Int32, ColumnProperty.Null));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("FORMAT_DATE", DbType.DateTime));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("NOTIF_NUM", DbType.String, 100));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("PROCEEDINGS_PLACE", DbType.String, 1000));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("REMARKS", DbType.String, 1000));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("DELIV_THROUGH_OFFICE", DbType.Boolean, ColumnProperty.NotNull, false));
            this.Database.AddColumn("GJI_PROTOCOL", new Column("PROCEEDING_COPY_NUM", DbType.Int32, ColumnProperty.Null));

            this.DisposalDataMigration();
            this.ActCheckDataMigration();
            this.ProtocolDataMigration();
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DISPOSAL", "TIME_VISIT_SART");
            this.Database.RemoveColumn("GJI_DISPOSAL", "TIME_VISIT_END");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_NUM");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_DATE");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_NUM_LETTER");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_DATE_LETTER");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_OBTAINED");
            this.Database.RemoveColumn("GJI_DISPOSAL", "NC_SENT");

            this.Database.RemoveColumn("GJI_ACTCHECK", "DOCUMENT_PLACE");
            this.Database.RemoveColumn("GJI_ACTCHECK", "DOCUMENT_TIME");

            this.Database.RemoveColumn("GJI_PROTOCOL", "FORMAT_PLACE");
            this.Database.RemoveColumn("GJI_PROTOCOL", "FORMAT_HOUR");
            this.Database.RemoveColumn("GJI_PROTOCOL", "FORMAT_MINUTE");
            this.Database.RemoveColumn("GJI_PROTOCOL", "FORMAT_DATE");
            this.Database.RemoveColumn("GJI_PROTOCOL", "NOTIF_NUM");
            this.Database.RemoveColumn("GJI_PROTOCOL", "PROCEEDINGS_PLACE");
            this.Database.RemoveColumn("GJI_PROTOCOL", "REMARKS");
            this.Database.RemoveColumn("GJI_PROTOCOL", "DELIV_THROUGH_OFFICE");
            this.Database.RemoveColumn("GJI_PROTOCOL", "PROCEEDING_COPY_NUM");
        }

        private void DisposalDataMigration()
        {
            if (this.Database.TableExists("GJI_NSO_DISPOSAL"))
            {
                const string migrationQuery = @"
                            update gji_disposal d
                            set
                                time_visit_sart = data.time_visit_sart,
                                time_visit_end  = data.time_visit_end,
                                nc_num          = data.nc_num,
                                nc_date         = data.nc_date,
                                nc_num_letter   = data.nc_num_letter,
                                nc_date_letter  = data.nc_date_letter,
                                nc_obtained     = data.nc_obtained,
                                nc_sent         = data.nc_sent
                            from (
                                    select
                                        id,
                                        time_visit_sart,
                                        time_visit_end,
                                        nc_num,
                                        nc_date,
                                        nc_num_letter,
                                        nc_date_letter,
                                        nc_obtained,
                                        nc_sent
                                    from gji_nso_disposal nd
                                    ) data
                            where d.id = data.id;";

                this.Database.ExecuteNonQuery(migrationQuery);
            }
        }

        private void ActCheckDataMigration()
        {
            if (this.Database.TableExists("GJI_NSO_ACTCHECK"))
            {
                const string migrationQuery = @"
                            update gji_actcheck a
                            set
                              document_place = data.document_place,
                              document_time  = data.document_time
                            from (
                                   select
                                     id,
                                     document_place,
                                     document_time
                                   from gji_nso_actcheck na
                                 ) data
                            where a.id = data.id;";

                this.Database.ExecuteNonQuery(migrationQuery);
            }
        }

        private void ProtocolDataMigration()
        {
            if (this.Database.TableExists("GJI_NSO_PROTOCOL"))
            {
                const string migrationQuery = @"
                            update gji_protocol p
                            set
                              format_date          = data.format_date,
                              format_place         = data.format_place,
                              format_hour          = data.format_hour,
                              format_minute        = data.format_minute,
                              notif_num            = data.notif_num,
                              proceedings_place    = data.proceedings_place,
                              remarks              = data.remarks,
                              deliv_through_office = data.deliv_through_office,
                              proceeding_copy_num  = data.proceeding_copy_num
                            from (
                                   select
                                     id,
                                     format_date,
                                     format_place,
                                     format_hour,
                                     format_minute,
                                     notif_num,
                                     proceedings_place,
                                     remarks,
                                     deliv_through_office,
                                     proceeding_copy_num
                                   from gji_nso_protocol np
                                 ) data
                            where p.id = data.id;";

                this.Database.ExecuteNonQuery(migrationQuery);
            }
        }
    }
}