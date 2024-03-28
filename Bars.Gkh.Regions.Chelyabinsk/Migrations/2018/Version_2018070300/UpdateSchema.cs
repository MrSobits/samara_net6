namespace Bars.Gkh.Regions.Chelyabinsk.Migrations._2018.Version_2018070300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2018070300")]
    [MigrationDependsOn(typeof(Version_2018060800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        SchemaQualifiedObjectName tableBig = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTBIG", Schema = "IMPORT" };
        SchemaQualifiedObjectName tableOwner = new SchemaQualifiedObjectName { Name = "ROSREGEXTRACTBIGOWNER", Schema = "IMPORT" };
        public override void Up()
        {
            Database.AddColumn(tableBig, new Column("RoomArea", DbType.String));
            Database.AddTable(tableOwner,
                    new Column("Id", DbType.Int64, ColumnProperty.PrimaryKeyWithIdentity),
                    new Column("ExtractId", DbType.Int32),
                    new Column("OwnerName", DbType.String),
                    new Column("AreaShareNum", DbType.Int32),
                    new Column("AreaShareDen", DbType.Int32),
                    new Column("RightNumber", DbType.Int32));
            Database.AddIndex("ExtractIdInd", false, tableOwner, "ExtractId");
            AddOwnerGenFunc();
        }

        public override void Down()
        {
            Database.RemoveColumn(tableBig,"RoomArea");
            RemoveOwnerGenFunc();
            Database.RemoveTable(tableOwner);
        }

        public void AddOwnerGenFunc()
        {
            Database.ExecuteNonQuery(@"
                create or replace function import.rosregextractbigownersgenerate()
                returns void as
                $$
                begin
                    delete from import.rosregextractbigowner;
                    with tmp as (
                    SELECT
                        id,
                        xml,
                        unnest(xpath('//base:Person/base:Content/text()', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::TEXT owner_name,
                        unnest(xpath('//base:Right/@RightNumber', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER right_number,
                        unnest(xpath('//base:Registration/base:Share/@Numerator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER num,
                        unnest(xpath('//base:Registration/base:Share/@Denominator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER den
                        from import.rosregextractbig rr
                    UNION ALL
                    SELECT
                        id,
                        xml,
                        unnest(xpath('//base:Right/base:Owner/base:Governance/base:Name/text()', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::TEXT owner_name,
                        unnest(xpath('//base:Right/@RightNumber', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER right_number,
                        unnest(xpath('//base:Registration/base:Share/@Numerator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER num,
                        unnest(xpath('//base:Registration/base:Share/@Denominator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER den
                        from import.rosregextractbig rr
                    UNION ALL
                        SELECT
                        id,
                        xml,
                        unnest(xpath('//base:Right/base:Owner/base:Organization/base:Name/text()', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::TEXT owner_name,
                        unnest(xpath('//base:Right/@RightNumber', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER right_number,
                        unnest(xpath('//base:Registration/base:Share/@Numerator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER num,
                        unnest(xpath('//base:Registration/base:Share/@Denominator', xml, ARRAY[ARRAY['base', 'urn://x-artefacts-rosreestr-ru/outgoing/kpoks/4.0.1']]))::VARCHAR::INTEGER den
                        from import.rosregextractbig rr
                    )

                    INSERT INTO import.rosregextractbigowner(id, extractid, ownername, areasharenum, areashareden, rightnumber) 
                           SELECT row_number() over() id,tmp.id extract_id, owner_name, num, den, right_number from tmp where owner_name is not NULL order by id;
                end
                $$
                language 'plpgsql'; ");
        }
        public void RemoveOwnerGenFunc()
        {
            Database.ExecuteNonQuery(@"Drop function if exists import.rosregextractbigownersgenerate();");
        }
    }
}