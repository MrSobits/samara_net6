using System;
using System.Text;

namespace Bars.B4.Modules.FIAS.AutoUpdater.Helpers
{
    /// <summary>
    /// TODO: Вынести в автомаппер
    /// </summary>
    internal static class ConvertHelper
    {
        internal static FiasHouse GetFiasHouse(HousesHouse housesHouse)
        {
            Guid houseId;
            Guid houseGuid;
            Guid aoGuid;
            FiasActualStatusEnum actStatus = 0;
            FiasStructureTypeEnum strStatus = 0;
            Enums.FiasEstimateStatusEnum estimateStatus = 0;

            Enum.TryParse(housesHouse.STATSTATUS, out actStatus);
            Enum.TryParse(housesHouse.STRSTATUS, out strStatus);
            Enum.TryParse(housesHouse.ESTSTATUS, out estimateStatus);

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
                StructureType = strStatus,
                EstimateStatus = estimateStatus
            };
        }

        internal static void CopyToFiasHouse(HousesHouse housesHouse, FiasHouse fiasHouse)
        {
            Guid houseId;
            Guid houseGuid;
            Guid aoGuid;
            FiasActualStatusEnum actStatus = 0;
            FiasStructureTypeEnum strStatus = 0;
            Enums.FiasEstimateStatusEnum estimateStatus = 0;

            Enum.TryParse(housesHouse.STATSTATUS, out actStatus);
            Enum.TryParse(housesHouse.STRSTATUS, out strStatus);
            Enum.TryParse(housesHouse.ESTSTATUS, out estimateStatus);


            fiasHouse.TypeRecord = FiasTypeRecordEnum.Fias;
            fiasHouse.HouseId = Guid.TryParse(housesHouse.HOUSEID, out houseId) ? houseId : (Guid?)null;
            fiasHouse.HouseGuid = Guid.TryParse(housesHouse.HOUSEGUID, out houseGuid) ? houseGuid : (Guid?)null;
            fiasHouse.AoGuid = Guid.TryParse(housesHouse.AOGUID, out aoGuid) ? aoGuid : Guid.Empty;
            fiasHouse.PostalCode = housesHouse.POSTALCODE;
            fiasHouse.Okato = housesHouse.OKATO;
            fiasHouse.Oktmo = housesHouse.OKTMO;
            fiasHouse.HouseNum = housesHouse.HOUSENUM;
            fiasHouse.BuildNum = housesHouse.BUILDNUM;
            fiasHouse.StrucNum = housesHouse.STRUCNUM;
            fiasHouse.ActualStatus = actStatus;
            fiasHouse.UpdateDate = housesHouse.UPDATEDATE;
            fiasHouse.StartDate = housesHouse.STARTDATE;
            fiasHouse.EndDate = housesHouse.ENDDATE;
            fiasHouse.StructureType = strStatus;
            fiasHouse.EstimateStatus = estimateStatus;
        }

        internal static Fias GetFias(AddressObjectsObject addressObjectsObject)
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
                CodeRecord = GetCodeRecord(addressObjectsObject),
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

        internal static void CopyToFias(AddressObjectsObject addressObjectsObject, Fias fiasRecord)
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

            fiasRecord.TypeRecord = FiasTypeRecordEnum.Fias;
            fiasRecord.CodeRecord = GetCodeRecord(addressObjectsObject);
            fiasRecord.AOGuid = addressObjectsObject.AOGUID;
            fiasRecord.FormalName = addressObjectsObject.FORMALNAME;
            fiasRecord.CodeRegion = addressObjectsObject.REGIONCODE;
            fiasRecord.CodeAuto = addressObjectsObject.AUTOCODE;
            fiasRecord.CodeArea = addressObjectsObject.AREACODE;
            fiasRecord.CodeCity = addressObjectsObject.CITYCODE;
            fiasRecord.CodeCtar = addressObjectsObject.CTARCODE;
            fiasRecord.CodePlace = addressObjectsObject.PLACECODE;
            fiasRecord.CodeStreet = addressObjectsObject.STREETCODE;
            fiasRecord.CodeExtr = addressObjectsObject.EXTRCODE;
            fiasRecord.CodeSext = addressObjectsObject.SEXTCODE;
            fiasRecord.OffName = addressObjectsObject.OFFNAME;
            fiasRecord.PostalCode = addressObjectsObject.POSTALCODE;
            fiasRecord.IFNSFL = addressObjectsObject.IFNSFL;
            fiasRecord.TerrIFNSFL = addressObjectsObject.TERRIFNSFL;
            fiasRecord.IFNSUL = addressObjectsObject.IFNSUL;
            fiasRecord.TerrIFNSUL = addressObjectsObject.TERRIFNSUL;
            fiasRecord.OKATO = addressObjectsObject.OKATO;
            fiasRecord.OKTMO = addressObjectsObject.OKTMO;
            fiasRecord.UpdateDate = addressObjectsObject.UPDATEDATE;
            fiasRecord.ShortName = addressObjectsObject.SHORTNAME;
            fiasRecord.AOLevel = aoLevel;
            fiasRecord.ParentGuid = addressObjectsObject.PARENTGUID;
            fiasRecord.AOId = addressObjectsObject.AOID;
            fiasRecord.PrevId = addressObjectsObject.PREVID;
            fiasRecord.NextId = addressObjectsObject.NEXTID;
            fiasRecord.KladrCode = addressObjectsObject.CODE;
            fiasRecord.KladrPlainCode = addressObjectsObject.PLAINCODE;
            fiasRecord.ActStatus = actStatus;
            fiasRecord.CentStatus = centStatus;
            fiasRecord.OperStatus = operStatus;
            fiasRecord.KladrCurrStatus = kladrCurrStatus;
            fiasRecord.StartDate = addressObjectsObject.STARTDATE;
            fiasRecord.EndDate = addressObjectsObject.ENDDATE;
            fiasRecord.NormDoc = addressObjectsObject.NORMDOC;
        }

        private static string GetCodeRecord(AddressObjectsObject fiasRecord)
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
    }
}
