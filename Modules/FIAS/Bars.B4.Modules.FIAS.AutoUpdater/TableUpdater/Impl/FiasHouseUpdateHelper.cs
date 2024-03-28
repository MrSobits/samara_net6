namespace Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater.Impl
{
    using Bars.B4.Modules.FIAS.AutoUpdater.Helpers;

    using Npgsql;

    using NpgsqlTypes;

    /// <summary>
    /// Сервис обновления <see cref="FiasHouse"/>
    /// </summary>
    internal class FiasHouseUpdateHelper : BaseTableUpdateHelper<FiasHouse>
    {
        /// <inheritdoc />
        protected override string TableColumns { get; } = @"
HOUSE_ID,HOUSE_GUID,AO_GUID,POSTAL_CODE,OKATO,OKTMO,HOUSE_NUM,BUILD_NUM,
STRUC_NUM,STAT_STATUS,UPDATE_DATE,TYPE_RECORD,START_DATE,END_DATE,STRUCTURE_TYPE";

        /// <inheritdoc />
        protected override void WriteRecord(NpgsqlBinaryImporter writer, FiasHouse record)
        {
            writer.StartRow();
            
            writer.WriteWithTypeCheck(record.HouseId);
            writer.WriteWithTypeCheck(record.HouseGuid);
            writer.WriteWithTypeCheck(record.AoGuid);
            writer.WriteWithTypeCheck(record.PostalCode);
            writer.WriteWithTypeCheck(record.Okato);
            writer.WriteWithTypeCheck(record.Oktmo);
            writer.WriteWithTypeCheck(record.HouseNum);
            writer.WriteWithTypeCheck(record.BuildNum);
            writer.WriteWithTypeCheck(record.StrucNum);
            writer.WriteWithTypeCheck(record.ActualStatus);
            writer.WriteWithTypeCheck(record.UpdateDate);
            writer.WriteWithTypeCheck(record.TypeRecord);
            writer.WriteWithTypeCheck(record.StartDate);
            writer.WriteWithTypeCheck(record.EndDate);
            writer.WriteWithTypeCheck(record.StructureType, true);
        }

        /// <inheritdoc />
        public override string UpdateFromTempTableCommand => $"UPDATE {this.TableName} AS t1 " +
            "SET HOUSE_ID = t2.HOUSE_ID, " +
            "HOUSE_GUID = t2.HOUSE_GUID, " +
            "AO_GUID = t2.AO_GUID, " +
            "POSTAL_CODE = t2.POSTAL_CODE, " +
            "OKATO = t2.OKATO, " +
            "OKTMO = t2.OKTMO, " +
            "HOUSE_NUM = t2.HOUSE_NUM, " +
            "BUILD_NUM = t2.BUILD_NUM, " +
            "STRUC_NUM = t2.STRUC_NUM, " +
            "STAT_STATUS = t2.STAT_STATUS, " +
            "UPDATE_DATE = t2.UPDATE_DATE, " +
            "TYPE_RECORD = t2.TYPE_RECORD, " +
            "START_DATE = t2.START_DATE, " +
            "END_DATE = t2.END_DATE, " +
            "STRUCTURE_TYPE = t2.STRUCTURE_TYPE " +
            $"FROM {this.TempTableName} AS t2 " +
            $"WHERE t1.TYPE_RECORD = {(int)FiasTypeRecordEnum.Fias} AND t1.HOUSE_GUID = t2.HOUSE_GUID AND t1.UPDATE_DATE < t2.UPDATE_DATE;\n" +

            $"INSERT INTO {this.TableName} ({this.TableColumns}) (SELECT {this.TableColumns} " +
            $"FROM {this.TempTableName} WHERE HOUSE_GUID NOT IN (SELECT HOUSE_GUID FROM {this.TableName}));\n" +

            $"DROP TABLE {this.TempTableName};";

        /// <inheritdoc />
        public override string DropFiasTypeRecordsCommand => $@"
            DELETE FROM {this.TableName} h 
            WHERE TYPE_RECORD = {(int)FiasTypeRecordEnum.Fias} 
              AND EXISTS(SELECT FROM B4_FIAS f WHERE CAST(f.AOGUID AS UUID) = h.AO_GUID AND f.REGIONCODE = '{RegionCodeHelper.GetRegionCode()}');";
    }
}