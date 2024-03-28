using Bars.B4.DataAccess.ByCode;

namespace Bars.B4.Modules.FIAS.Map
{
    public class FiasMap : PersistentObjectMap<Fias>
    {
        public FiasMap(): base("B4_FIAS")
        {
            //Данные поля формирвеются для нужд проектов Б4
            Map(x => x.CodeRecord, "CODERECORD", false, 50);
            Map(x => x.TypeRecord, "TYPERECORD", true);//.CustomType<FiasTypeRecordEnum>();
            Map(x => x.MirrorGuid, "MIRROR_GUID", false, 36);

            //Следущие поля загружаются из ФИАС
            Map(x => x.ActStatus, "ACTSTATUS", true);//.CustomType<FiasActualStatusEnum>();
            Map(x => x.AOLevel, "AOLEVEL", true);//.CustomType<FiasLevelEnum>();
            Map(x => x.CentStatus, "CENTSTATUS", true);//.CustomType<FiasCenterStatusEnum>();
            Map(x => x.OperStatus, "OPERSTATUS", true);//.CustomType<FiasOperationStatusEnum>();
            Map(x => x.StartDate, "STARTDATE");
            Map(x => x.EndDate, "ENDDATE");
            Map(x => x.AOGuid, "AOGUID", false, 36);
            Map(x => x.AOId, "AOID", false, 36);
            Map(x => x.ParentGuid, "PARENTGUID", false, 36);
            Map(x => x.PrevId, "PREVID", false, 36);
            Map(x => x.NextId, "NEXTID", false, 36);
            Map(x => x.FormalName, "FORMALNAME", false, 120);
            Map(x => x.OffName, "OFFNAME", false, 120);
            Map(x => x.ShortName, "SHORTNAME", false, 10);
            Map(x => x.CodeRegion, "REGIONCODE", false, 5);
            Map(x => x.CodeAuto, "AUTOCODE", false, 5);
            Map(x => x.CodeArea, "AREACODE", false, 5);
            Map(x => x.CodeCity, "CITYCODE", false, 5);
            Map(x => x.CodeCtar, "CTARCODE", false, 5);
            Map(x => x.CodePlace, "PLACECODE", false, 5);
            Map(x => x.CodeStreet, "STREETCODE", false, 5);
            Map(x => x.CodeExtr, "EXTRCODE", false, 5);
            Map(x => x.CodeSext, "SEXTCODE", false, 5);
            Map(x => x.PostalCode, "POSTALCODE", false, 6);
            Map(x => x.IFNSFL, "IFNSFL", false, 4);
            Map(x => x.TerrIFNSFL, "TERRIFNSFL", false, 4);
            Map(x => x.IFNSUL, "IFNSUL", false, 4);
            Map(x => x.TerrIFNSUL, "TERRIFNSUL", false, 4);
            Map(x => x.OKATO, "OKATO", false, 11);
            Map(x => x.OKTMO, "OKTMO", false, 8);
            Map(x => x.UpdateDate, "UPDATEDATE");
            Map(x => x.NormDoc, "NORMDOC");
            Map(x => x.KladrCode, "KLADRCODE", false, 17);
            Map(x => x.KladrPlainCode, "KLADRPLAINCODE", false, 15);
            Map(x => x.KladrCurrStatus, "KLADRPCURRSTATUS");
        }
    }
}