namespace Bars.GkhDi.Migrations._2019.Version_2019071900
{
    using Bars.B4.Modules.Ecm7.Framework;

    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2019071900")]
    [MigrationDependsOn(typeof(_2019.Version_2019070500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddRefColumn("DI_DISINFO_DOC_PROT",
                new RefColumn("DISINFO_RO_ID", "FK_DI_DISINFO_DOC_PROT_DISINFO_RO_ID", "DI_DISINFO_REALOBJ", "ID"));

            this.Database.ExecuteNonQuery(@"
            drop table if exists tmp_doc;
            create temp table tmp_doc as
                select doc.id, dro.id real_obj_id, year, d.date_start, min(dro.id) over(partition by doc.id)
                from DI_DISINFO_DOC_PROT doc
                join DI_DISINFO_REALOBJ dro on dro.reality_obj_id = doc.reality_obj_id
                join DI_DICT_PERIOD d on d.id = period_di_id
                where doc.year between extract(year from d.date_start)-1 and extract(year from d.date_end);

            update DI_DISINFO_DOC_PROT set disinfo_ro_id = t.min from
            tmp_doc t where t.id = DI_DISINFO_DOC_PROT.id and real_obj_id = min; 

            insert into DI_DISINFO_DOC_PROT(object_version, object_create_date, object_edit_date, reality_obj_id, year, doc_num, doc_date, disinfo_ro_id)
            select 19200, now()::timestamp(0),now()::timestamp(0), reality_obj_id, d.year, doc_num, doc_date, real_obj_id
                from tmp_doc t join DI_DISINFO_DOC_PROT d on d.id = t.id
                where real_obj_id <> min;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("DI_DISINFO_DOC_PROT", "DISINFO_RO_ID");
            this.Database.ExecuteNonQuery(@"delete from DI_DISINFO_DOC_PROT where object_version = 19200;");
        }
    }
}