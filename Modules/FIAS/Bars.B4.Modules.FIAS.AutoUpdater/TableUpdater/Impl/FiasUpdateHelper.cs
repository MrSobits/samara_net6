namespace Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater.Impl
{
    using Bars.B4.Modules.FIAS.AutoUpdater.Helpers;

    using Npgsql;

    /// <summary>
    /// Сервис обновления <see cref="Fias"/>
    /// </summary>
    internal class FiasUpdateHelper : BaseTableUpdateHelper<Fias>
    {
        /// <inheritdoc />
        protected override string TableColumns { get; } = @"
AOGUID,FORMALNAME,REGIONCODE,AUTOCODE,AREACODE,CITYCODE,CTARCODE,PLACECODE,STREETCODE,
EXTRCODE,SEXTCODE,OFFNAME,POSTALCODE,IFNSFL,TERRIFNSFL,IFNSUL,TERRIFNSUL,OKATO,OKTMO,
UPDATEDATE,SHORTNAME,AOLEVEL,PARENTGUID,AOID,PREVID,NEXTID,KLADRCODE,KLADRPLAINCODE,
ACTSTATUS,CENTSTATUS,OPERSTATUS,KLADRPCURRSTATUS,STARTDATE,ENDDATE,NORMDOC,CODERECORD,TYPERECORD";

        /// <inheritdoc />
        protected override void WriteRecord(NpgsqlBinaryImporter writer, Fias record)
        {
            writer.StartRow();
            
            writer.WriteWithTypeCheck(record.AOGuid);
            writer.WriteWithTypeCheck(record.FormalName);
            writer.WriteWithTypeCheck(record.CodeRegion);
            writer.WriteWithTypeCheck(record.CodeAuto);
            writer.WriteWithTypeCheck(record.CodeArea);
            writer.WriteWithTypeCheck(record.CodeCity);
            writer.WriteWithTypeCheck(record.CodeCtar);
            writer.WriteWithTypeCheck(record.CodePlace);
            writer.WriteWithTypeCheck(record.CodeStreet);
            writer.WriteWithTypeCheck(record.CodeExtr);
            writer.WriteWithTypeCheck(record.CodeSext);
            writer.WriteWithTypeCheck(record.OffName);
            writer.WriteWithTypeCheck(record.PostalCode);
            writer.WriteWithTypeCheck(record.IFNSFL);
            writer.WriteWithTypeCheck(record.TerrIFNSFL);
            writer.WriteWithTypeCheck(record.IFNSUL);
            writer.WriteWithTypeCheck(record.TerrIFNSUL);
            writer.WriteWithTypeCheck(record.OKATO);
            writer.WriteWithTypeCheck(record.OKTMO);
            writer.WriteWithTypeCheck(record.UpdateDate);
            writer.WriteWithTypeCheck(record.ShortName);
            writer.WriteWithTypeCheck(record.AOLevel);
            writer.WriteWithTypeCheck(record.ParentGuid);
            writer.WriteWithTypeCheck(record.AOId);
            writer.WriteWithTypeCheck(record.PrevId);
            writer.WriteWithTypeCheck(record.NextId);
            writer.WriteWithTypeCheck(record.KladrCode);
            writer.WriteWithTypeCheck(record.KladrPlainCode);
            writer.WriteWithTypeCheck(record.ActStatus);
            writer.WriteWithTypeCheck(record.CentStatus);
            writer.WriteWithTypeCheck(record.OperStatus);
            writer.WriteWithTypeCheck(record.KladrCurrStatus);
            writer.WriteWithTypeCheck(record.StartDate);
            writer.WriteWithTypeCheck(record.EndDate);
            writer.WriteWithTypeCheck(record.NormDoc);
            writer.WriteWithTypeCheck(record.CodeRecord);
            writer.WriteWithTypeCheck(record.TypeRecord);
        }

        /// <inheritdoc />
        public override string UpdateFromTempTableCommand => $"UPDATE {this.TableName} AS t1 " +
            "SET AOGUID = t2.AOGUID, " +
            "FORMALNAME = t2.FORMALNAME, " +
            "REGIONCODE = t2.REGIONCODE, " +
            "AUTOCODE = t2.AUTOCODE, " +
            "AREACODE = t2.AREACODE, " +
            "CITYCODE = t2.CITYCODE, " +
            "CTARCODE = t2.CTARCODE, " +
            "PLACECODE = t2.PLACECODE, " +
            "STREETCODE = t2.STREETCODE, " +
            "EXTRCODE = t2.EXTRCODE, " +
            "SEXTCODE = t2.SEXTCODE, " +
            "OFFNAME = t2.OFFNAME, " +
            "POSTALCODE = t2.POSTALCODE, " +
            "IFNSFL = t2.IFNSFL, " +
            "TERRIFNSFL = t2.TERRIFNSFL, " +
            "IFNSUL = t2.IFNSUL, " +
            "TERRIFNSUL = t2.TERRIFNSUL, " +
            "OKATO = t2.OKATO, " +
            "OKTMO = t2.OKTMO, " +
            "UPDATEDATE = t2.UPDATEDATE, " +
            "SHORTNAME = t2.SHORTNAME, " +
            "AOLEVEL = t2.AOLEVEL, " +
            "PARENTGUID = t2.PARENTGUID, " +
            "AOID = t2.AOID, " +
            "PREVID = t2.PREVID, " +
            "NEXTID = t2.NEXTID, " +
            "KLADRCODE = t2.KLADRCODE, " +
            "KLADRPLAINCODE = t2.KLADRPLAINCODE, " +
            "ACTSTATUS = t2.ACTSTATUS, " +
            "CENTSTATUS = t2.CENTSTATUS, " +
            "OPERSTATUS = t2.OPERSTATUS, " +
            "KLADRPCURRSTATUS = t2.KLADRPCURRSTATUS, " +
            "STARTDATE = t2.STARTDATE, " +
            "ENDDATE = t2.ENDDATE, " +
            "NORMDOC = t2.NORMDOC, " +
            "CODERECORD = t2.CODERECORD, " +
            "TYPERECORD = t2.TYPERECORD " +
            $"FROM {this.TempTableName} AS t2 " +
            $"WHERE t1.TYPERECORD = {(int) FiasTypeRecordEnum.Fias} AND t1.AOGUID = t2.AOGUID AND t1.UPDATEDATE < t2.UPDATEDATE;\n" +

            $"INSERT INTO {this.TableName} ({this.TableColumns}) (SELECT {this.TableColumns} " +
            $"FROM {this.TempTableName} WHERE AOGUID NOT IN (SELECT AOGUID FROM {this.TableName}));\n" +

            $"DROP TABLE {this.TempTableName};";

        /// <inheritdoc />
        public override string DropFiasTypeRecordsCommand => $@"
            DELETE FROM {this.TableName} 
            WHERE TYPERECORD = {(int) FiasTypeRecordEnum.Fias} 
                AND REGIONCODE = '{RegionCodeHelper.GetRegionCode()}';";
    }
}