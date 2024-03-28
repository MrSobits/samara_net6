namespace Bars.Gkh.Overhaul.Import.CommonRealtyObjectImport
{
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS;

    public class DynamicAddress : DinamicAddress
    {
        public string PlaceCode { get; set; }
    }

    public sealed class StructElementRecord
    {
        public string LastOverhaulYear { get; set; }

        public string Volume { get; set; }

        public string Wearout { get; set; }

        public bool IsValid { get; set; }
    }

    public sealed class StructElementHeader
    {
        public int LastOverhaulYear { get; set; }

        public int Volume { get; set; }

        public int Wearout { get; set; }
    }

    public sealed class RealtyObjectAddress
    {
        public string KladrCode { get; set; }

        public string AoGuid { get; set; }

        public long Id { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public long RealityObjectId { get; set; }

        public long MunicipalityId { get; set; }

        public string Liter { get; set; }
    }

    public sealed class Record
    {
        public bool isValidRecord { get; set; }

        public long RoId { get; set; }

        public Dictionary<string, StructElementRecord> StructElementDict;

        public string LocalityName { get; set; }

        public DynamicAddress Address { get; set; }

        public long MunicipalityId { get; set; }

        public string StreetName { get; set; }

        public string CodeKladrStreet { get; set; }

        public string House { get; set; }

        public string Liter { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Строка файла импорта
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Вид дома
        /// </summary>
        public string TypeHouse { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        public string DateCommissioning { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public string BuildYear { get; set; }

        /// <summary>
        /// Общая площадь МКД (кв.м.)
        /// </summary>
        public string AreaMkd { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений в МКД (кв.м.)
        /// </summary>
        public string AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// Площадь жилая, всего (кв. м.)
        /// </summary>
        public string AreaLiving { get; set; }

        /// <summary>
        /// Площадь частной собственности (кв.м.)
        /// </summary>
        public string AreaOwned { get; set; }

        /// <summary>
        /// площадь в муниципальной собственности (кв.м.)
        /// </summary>
        public string AreaMunicipalOwned { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public string MaximumFloors { get; set; }

        /// <summary>
        /// Миниимальная этажность
        /// </summary>
        public string Floors { get; set; }

        /// <summary>
        /// Количество входов (подъездов)
        /// </summary>
        public string NumberEntrances { get; set; }

        /// <summary>
        /// Группа капитальности
        /// </summary>
        public string CapitalGroup { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public string NumberLiving { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public string NumberApartments { get; set; }

        /// <summary>
        /// Физический износ
        /// </summary>
        public string PhysicalWear { get; set; }

        /// <summary>
        /// Дата приватизации первого жилого помещения
        /// </summary>
        public string PrivatizationDateFirstApartment { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public string ConditionHouse { get; set; }

        public string IsNotInvolvedCr { get; set; }

        /// <summary>
        /// Код дома
        /// </summary>
        public string GkhCode { get; set; }

        public string RealEstTypeCode { get; set; }

        /// <summary>
        /// Административный округ (для МСК) 
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Адрес строкой (информационное, для МСК)
        /// </summary>
        public string StrAddress { get; set; }
    }
}