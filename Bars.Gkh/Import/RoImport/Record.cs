namespace Bars.Gkh.Import.RoImport
{
    using System.Collections.Generic;

    using Bars.B4.Modules.FIAS;

    public class Record
    {
        public bool isValidRecord { get; set; }

        public string CodeKladrStreet { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public int? FiasAddressId { get; set; }

        public FiasAddress FiasAddress { get; set; }

        public Dictionary<string, string> TechPassportData;

        public List<StructuralElementRecord> StructuralElements; 

        /// <summary>
        /// Строка файла импорта
        /// </summary>
        public int RowNumber { get; set; }

        /// <summary>
        /// Общая площадь МКД (кв.м.)
        /// </summary>
        public string AreaMkd { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка
        /// </summary>
        public string CadastreNumber { get; set; }

        /// <summary>
        /// Вид дома
        /// </summary>
        public string TypeHouse { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public string BuildYear { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public string NumberApartments { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public string NumberLiving { get; set; }

        /// <summary>
        /// Площадь жилая, всего (кв. м.)
        /// </summary>
        public string AreaLiving { get; set; }

        /// <summary>
        /// Площадь частной собственности (кв.м.)
        /// </summary>
        public string AreaOwned { get; set; }

        /// <summary>
        /// Площадь муниципальной собственности (кв.м.)
        /// </summary>
        public string AreaMunicipalOwned { get; set; }

        /// <summary>
        /// Площадь государственной собственности (кв.м.)
        /// </summary>
        public string AreaGovernmentOwned { get; set; }

        /// <summary>
        /// В т.ч. нежилых, функционального назначения (кв.м.)
        /// </summary>
        public string AreaNotLivingFunctional { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public string MaximumFloors { get; set; }

        /// <summary>
        /// Количество входов (подъездов)
        /// </summary>
        public string NumberEntrances { get; set; }

        /// <summary>
        /// Физический износ (%)
        /// </summary>
        public string PhysicalWear { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public string ConditionHouse { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        public string HeatingSystem { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public string WallMaterial { get; set; }
    }
}