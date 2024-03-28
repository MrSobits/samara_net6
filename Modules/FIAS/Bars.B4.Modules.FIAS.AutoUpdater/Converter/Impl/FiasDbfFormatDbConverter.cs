namespace Bars.B4.Modules.FIAS.AutoUpdater.Converter.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;

    using Castle.Core;
    using Castle.Windsor;

    using SocialExplorer.IO.FastDBF;

    /// <summary>
    /// Сервис конвертации данных ФИАС из *.dbf файлов
    /// </summary>
    public class FiasDbfFormatDbConverter : IFiasDbConverter, IInitializable
    {
        public IWindsorContainer Container { get; set; }

        protected virtual Encoding DbfFileEncoding { get; set; } = Encoding.GetEncoding("cp866");

        /// <inheritdoc />
        public IEnumerable<Fias> GetFiasRecords(IFiasArchiveReader reader, bool isDelta)
        {
            using (var dbfFileWrapper = new DbfFileWrapper(reader.FiasLinkedFilesDict["addrob"], this.DbfFileEncoding))
            {
                var dbfFile = dbfFileWrapper.File;
                var record = new DbfRecord(dbfFile.Header);

                while (dbfFile.ReadNext(record))
                {
                    var addrRecord = this.ConvertDbfRecordTo<AddressObjectsObject>(record);

                    yield return this.GetFias(addrRecord);
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<FiasHouse> GetFiasHouseRecords(IFiasArchiveReader reader,  bool isDelta)
        {
            using (var dbfFileWrapper = new DbfFileWrapper(reader.FiasHouseLinkedFilesDict["house"], this.DbfFileEncoding))
            {
                var dbfFile = dbfFileWrapper.File;
                var record = new DbfRecord(dbfFile.Header);

                while (dbfFile.ReadNext(record))
                {
                    var houseRecord = this.ConvertDbfRecordTo<HousesHouse>(record);

                    yield return this.GetFiasHouse(houseRecord);
                }
            }
        }

        private Fias GetFias(AddressObjectsObject addressObjectsObject)
        {
            FiasLevelEnum aoLevel = 0;
            FiasActualStatusEnum actStatus = 0;
            FiasCenterStatusEnum centStatus = 0;
            FiasOperationStatusEnum operStatus = 0;
            int kladrCurrStatus = 0;

            Enum.TryParse(addressObjectsObject.AOLEVEL, out aoLevel);
            Enum.TryParse(addressObjectsObject.ACTSTATUS, out actStatus);
            Enum.TryParse(addressObjectsObject.CENTSTATUS, out centStatus);
            Enum.TryParse(addressObjectsObject.OPERSTATUS, out operStatus);
            int.TryParse(addressObjectsObject.CURRSTATUS, out kladrCurrStatus);

            return new Fias
            {
                TypeRecord = FiasTypeRecordEnum.Fias,
                CodeRecord = this.GetCodeRecord(addressObjectsObject),
                AOGuid = addressObjectsObject.AOGUID,
                FormalName = addressObjectsObject.FORMALNAME,
                CodeRegion = addressObjectsObject.REGIONCODE,
                CodeAuto = addressObjectsObject.AUTOCODE,
                CodeArea = addressObjectsObject.AREACODE,
                CodeCity = addressObjectsObject.CITYCODE,
                CodeCtar = addressObjectsObject.CTARCODE,
                CodePlace = addressObjectsObject.PLACECODE,
                CodeStreet = addressObjectsObject.STREETCODE,
                CodeExtr = addressObjectsObject.EXTRCODE,
                CodeSext = addressObjectsObject.SEXTCODE,
                OffName = addressObjectsObject.OFFNAME,
                PostalCode = addressObjectsObject.POSTALCODE,
                IFNSFL = addressObjectsObject.IFNSFL,
                TerrIFNSFL = addressObjectsObject.TERRIFNSFL,
                IFNSUL = addressObjectsObject.IFNSUL,
                TerrIFNSUL = addressObjectsObject.TERRIFNSUL,
                OKATO = addressObjectsObject.OKATO,
                OKTMO = addressObjectsObject.OKTMO,
                UpdateDate = addressObjectsObject.UPDATEDATE,
                ShortName = addressObjectsObject.SHORTNAME,
                AOLevel = aoLevel,
                ParentGuid = addressObjectsObject.PARENTGUID,
                AOId = addressObjectsObject.AOID,
                PrevId = addressObjectsObject.PREVID,
                NextId = addressObjectsObject.NEXTID,
                KladrCode = addressObjectsObject.CODE,
                KladrPlainCode = addressObjectsObject.PLAINCODE,
                ActStatus = actStatus,
                CentStatus = centStatus,
                OperStatus = operStatus,
                KladrCurrStatus = kladrCurrStatus,
                StartDate = addressObjectsObject.STARTDATE,
                EndDate = addressObjectsObject.ENDDATE,
                NormDoc = addressObjectsObject.NORMDOC
            };
        }

        private string GetCodeRecord(AddressObjectsObject fiasRecord)
        {
            var codeRecord = new StringBuilder();
            FiasLevelEnum aoLevel = 0;

            Enum.TryParse(fiasRecord.AOLEVEL, out aoLevel);
            if (aoLevel >= FiasLevelEnum.Region)
            {
                codeRecord.Append(fiasRecord.REGIONCODE);
            }
            if (aoLevel >= FiasLevelEnum.AutonomusRegion)
            {
                codeRecord.Append(fiasRecord.AUTOCODE);
            }
            if (aoLevel >= FiasLevelEnum.Raion)
            {
                codeRecord.Append(fiasRecord.AREACODE);
            }
            if (aoLevel >= FiasLevelEnum.City)
            {
                codeRecord.Append(fiasRecord.CITYCODE);
            }
            if (aoLevel >= FiasLevelEnum.Ctar)
            {
                codeRecord.Append(fiasRecord.CTARCODE);
            }
            if (aoLevel >= FiasLevelEnum.Place)
            {
                codeRecord.Append(fiasRecord.PLACECODE);
            }
            if (aoLevel >= FiasLevelEnum.Street)
            {
                codeRecord.Append(fiasRecord.STREETCODE);
            }
            if (aoLevel >= FiasLevelEnum.Extr)
            {
                codeRecord.Append(fiasRecord.EXTRCODE);
            }
            if (aoLevel >= FiasLevelEnum.Sext)
            {
                codeRecord.Append(fiasRecord.SEXTCODE);
            }
            return codeRecord.ToString();
        }

        private FiasHouse GetFiasHouse(HousesHouse housesHouse)
        {
            Guid houseId;
            Guid houseGuid;
            Guid aoGuid;
            FiasActualStatusEnum actStatus = 0;
            FiasStructureTypeEnum strStatus = 0;

            Enum.TryParse(housesHouse.STATSTATUS, out actStatus);
            Enum.TryParse(housesHouse.STRSTATUS, out strStatus);

            return new FiasHouse
            {
                TypeRecord = FiasTypeRecordEnum.Fias,
                HouseId = Guid.TryParse(housesHouse.HOUSEID, out houseId) ? houseId : (Guid?)null,
                HouseGuid = Guid.TryParse(housesHouse.HOUSEGUID, out houseGuid) ? houseGuid : (Guid?)null,
                AoGuid = Guid.TryParse(housesHouse.AOGUID, out aoGuid) ? aoGuid : Guid.Empty,
                PostalCode = housesHouse.POSTALCODE,
                Okato = housesHouse.OKATO,
                Oktmo = housesHouse.OKTMO,
                HouseNum = housesHouse.HOUSENUM,
                BuildNum = housesHouse.BUILDNUM,
                StrucNum = housesHouse.STRUCNUM,
                ActualStatus = actStatus,
                UpdateDate = housesHouse.UPDATEDATE,
                StartDate = housesHouse.STARTDATE,
                EndDate = housesHouse.ENDDATE,
                StructureType = strStatus
            };
        }

        private T ConvertDbfRecordTo<T>(DbfRecord record)
            where T : new()
        {
            var result = new T();
            var type = typeof(T);
            object newValue;

            foreach (var property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                var colId = record.FindColumn(property.Name);

                if (colId < 0)
                {
                    continue;
                }

                if (property.PropertyType == typeof(DateTime))
                {
                    newValue = record.GetDateValue(colId);
                }
                else

                {
                    var stringValue = record[colId];
                    switch (Type.GetTypeCode(property.PropertyType))
                    {
                        case TypeCode.SByte:
                            newValue = sbyte.Parse(stringValue);
                            break;
                        case TypeCode.Byte:
                            newValue = byte.Parse(stringValue);
                            break;
                        case TypeCode.Int16:
                            newValue = short.Parse(stringValue);
                            break;
                        case TypeCode.UInt16:
                            newValue = ushort.Parse(stringValue);
                            break;
                        case TypeCode.Int32:
                            newValue = int.Parse(stringValue);
                            break;
                        case TypeCode.UInt32:
                            newValue = uint.Parse(stringValue);
                            break;
                        default:
                            newValue = stringValue.Trim();
                            break;
                    }
                }

                property.SetValue(result, newValue);

            }

            return result;
        }

        private class DbfFileWrapper : IDisposable
        {
            private readonly DbfFile dbfFile;

            public DbfFile File => this.dbfFile;

            public DbfFileWrapper(string dbfFilePath, Encoding encoding)
            {
                this.dbfFile = new DbfFile(encoding);
                this.dbfFile.Open(dbfFilePath, FileMode.Open, FileAccess.Read);
            }

            /// <inheritdoc />
            public void Dispose()
            {
                this.dbfFile.Close();
            }
        }

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Modules.FIAS.AutoUpdater");

                var dbfEncodingString = config.GetAs("DbfEncoding", default(string), true);
                var dbfEncodingInt = config.GetAs("DbfEncoding", default(int), true);

                if (!string.IsNullOrEmpty(dbfEncodingString))
                {
                    this.DbfFileEncoding = Encoding.GetEncoding(dbfEncodingString);
                }
                else if (dbfEncodingInt != 0)
                {
                    this.DbfFileEncoding = Encoding.GetEncoding(dbfEncodingInt);
                }
            }
        }
    }
}