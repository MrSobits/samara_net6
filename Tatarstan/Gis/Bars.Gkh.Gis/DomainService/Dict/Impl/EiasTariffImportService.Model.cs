namespace Bars.Gkh.Gis.DomainService.Dict.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    using Bars.Gkh.Gis.Entities.Dict;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.Utils.Json;

    using Newtonsoft.Json;

    /// <summary>
    /// Модель получаемых данных
    /// </summary>
    public partial class EiasTariffImportService
    {
        #region ApiDescribe
        private class DescribeResponse
        {
            [JsonProperty("data")]
            public DescribeData Data { get; set; }
        }

        private class DescribeData
        {
            [JsonProperty("description")]
            public object Description { get; set; }

            [JsonProperty("cols_info")]
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public List<ColsInfo> ColsInfo { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("comments")]
            public List<Comment> Comments { get; set; }

            [JsonProperty("has_timestamp")]
            public bool HasTimestamp { get; set; }

            [JsonProperty("group_name")]
            public object GroupName { get; set; }

            [JsonProperty("is_auth_reg")]
            public bool IsAuthReg { get; set; }

            [JsonProperty("rows_count")]
            public long RowsCount { get; set; }
        }

        private class ColsInfo
        {
            [JsonProperty("DATA_PRECISION")]
            public long? DataPrecision { get; set; }

            [JsonProperty("COMMENTS")]
            public string Comments { get; set; }

            [JsonProperty("COLUMN_NAME")]
            public string ColumnName { get; set; }

            [JsonProperty("DATA_LENGTH")]
            public long DataLength { get; set; }

            [JsonProperty("IS_PK")]
            public string IsPk { get; set; }

            [JsonProperty("DATA_TYPE")]
            public string DataType { get; set; }

            [JsonProperty("NULLABLE")]
            public string Nullable { get; set; }
        }

        private class Comment
        {
            [JsonProperty("COMMENTS")]
            public object Comments { get; set; }
        }
        #endregion

        #region ApiData
        private class TariffResponse
        {
            [JsonProperty("data")]
            public List<TariffData> Data { get; set; }
        }

        private class TariffData
        {
            [JsonProperty("AUDIT_DATE")]
            public DateTime? AuditDate { get; set; }

            [JsonProperty("BP_CONTRAGENTS_NAME")]
            public string BpContragentsName { get; set; }

            [JsonProperty("CONTRAGENTS_INN")]
            public string ContragentsInn { get; set; }

            [JsonProperty("CONTRAGENTS_KPP")]
            public string ContragentsKpp { get; set; }

            [JsonProperty("CONTRAGENTS_NAME")]
            public string ContragentsName { get; set; }

            [JsonProperty("CONTRAGENTS_VDET")]
            public string ContragentsVdet { get; set; }

            [JsonProperty("DML_TYPE")]
            public string DmlType { get; set; }

            [JsonProperty("FILE_NAME")]
            public string FileName { get; set; }

            [JsonProperty("ID")]
            public string Id { get; set; }

            [JsonProperty("MOLIST_MOGUID")]
            public string MolistMoguid { get; set; }

            [JsonProperty("MOLIST_NAME")]
            public string MolistName { get; set; }

            [JsonProperty("MRLIST_MUNICIPALGUID")]
            public string MrlistMunicipalguid { get; set; }

            [JsonProperty("MRLIST_NAME")]
            public string MrlistName { get; set; }

            [JsonProperty("NAME")]
            public string Name { get; set; }

            [JsonProperty("RN")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? Rn { get; set; }

            [JsonProperty("SERVICES_NAMEID")]
            public string ServicesNameid { get; set; }

            [JsonProperty("SERVICES_SERVICEID")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? ServicesServiceid { get; set; }

            [JsonProperty("TARIFINFO_CE")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoCe { get; set; }

            [JsonProperty("TARIFINFO_CITY")]
            [JsonConverter(typeof(NullableEnumConverter<SettelmentType>))]
            public SettelmentType? TarifinfoCity { get; set; }

            [JsonProperty("TARIFINFO_CONSUMER")]
            [JsonConverter(typeof(NullableEnumConverter<ConsumerType>))]
            public ConsumerType? TarifinfoConsumer { get; set; }

            [JsonProperty("TARIFINFO_DPR")]
            public string TarifinfoDpr { get; set; }

            [JsonProperty("TARIFINFO_DPR_BP")]
            public string TarifinfoDprBp { get; set; }

            [JsonProperty("TARIFINFO_ELECTRICCOOKER")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoElectriccooker { get; set; }

            [JsonProperty("TARIFINFO_NDS")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoNds { get; set; }

            [JsonProperty("TARIFINFO_SOC")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoSoc { get; set; }

            [JsonProperty("TARIFINFO_STAGE")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoStage { get; set; }

            [JsonProperty("TARIFINFO_TARIF1VALUE")]
            public decimal? TarifinfoTarif1Value { get; set; }

            [JsonProperty("TARIFINFO_TARIF2VALUE")]
            public decimal? TarifinfoTarif2Value { get; set; }

            [JsonProperty("TARIFINFO_TARIF3VALUE")]
            public decimal? TarifinfoTarif3Value { get; set; }

            [JsonProperty("TARIFINFO_TARIFENDDATE")]
            public DateTime? TarifinfoTarifenddate { get; set; }

            [JsonProperty("TARIFINFO_TARIFKIND")]
            [JsonConverter(typeof(NullableEnumConverter<GisTariffKind>))]
            public GisTariffKind? TarifinfoTarifkind { get; set; }

            [JsonProperty("TARIFINFO_TARIFSTARTDATE")]
            public DateTime? TarifinfoTarifstartdate { get; set; }

            [JsonProperty("TARIFINFO_TYPECONSUMER")]
            [JsonConverter(typeof(NullableEnumConverter<ConsumerByElectricEnergyType>))]
            public ConsumerByElectricEnergyType? TarifinfoTypeconsumer { get; set; }

            [JsonProperty("TARIFINFO_UNITCODE")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoUnitcode { get; set; }

            [JsonProperty("TARIFINFO_ZONES")]
            [JsonConverter(typeof(ConcreteTypeConverter<int?>))]
            public int? TarifinfoZones { get; set; }
        }
        #endregion
    }
} 