namespace Bars.Gkh.Import
{
    using B4.Modules.FIAS;

    public class FundRealtyObjectRecord
    {
        public bool isValidRecord { get; set; }

        public string CodeKladrStreet { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public long? FiasAddressId { get; set; }

        public FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Федеральный номер
        /// </summary>
        public string FederalNumber { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public string NumberLiving { get; set; }

        /// <summary>
        /// Группа капитальности
        /// </summary>
        public string CapitalGroup { get; set; }

        /// <summary>
        /// Серия, тип проекта
        /// </summary>
        public string TypeProject { get; set; }

        /// <summary>
        /// Общая площадь МКД (кв.м.)
        /// </summary>
        public string AreaMkd { get; set; }

        /// <summary>
        /// Площадь жилая, всего (кв. м.)
        /// </summary>
        public string AreaLiving { get; set; }

        /// <summary>
        /// Площадь жилая находящаяся в собственности граждан, всего (кв. м.)
        /// </summary>
        public string AreaLivingOwned { get; set; }

        /// <summary>
        /// Дата сдачи в эксплуатацию
        /// </summary>
        public string DateCommissioning { get; set; }

        /// <summary>
        /// Физический износ (%)
        /// </summary>
        public string PhysicalWear { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка
        /// </summary>
        public string CadastreNumber { get; set; }

        /// <summary>
        /// Вид дома
        /// </summary>
        public string TypeHouse { get; set; }

        /// <summary>
        /// дата сноса
        /// </summary>
        public string DateDemolition { get; set; }

        /// <summary>
        /// SQV_ROOMS Общая площадь жилых и нежилых помещений в МКД (кв.м.)
        /// </summary>
        public string AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// SQV_NOT_DW В т.ч. нежилых, функционального назначения (кв.м.)
        /// </summary>
        public string AreaNotLivingFunctional { get; set; }

        /// <summary>
        /// SQV_MUN Площадь муниципальной собственности (кв.м.)
        /// </summary>
        public string AreaMunicipalOwned { get; set; }

        /// <summary>
        /// SQV_GOS Площадь государственной собственности (кв.м.)
        /// </summary>
        public string AreaGovernmentOwned { get; set; }
 
        /// <summary>
        /// "Земельный участок" -Дата постановки на учёт
        /// </summary>
        public string CadastrDate { get; set; }

        /// <summary>
        /// Форма "Земельный участок" -поле Документ
        /// </summary>
        public string CadastrDocName { get; set; }

        /// <summary>
        /// Год постройки
        /// </summary>
        public string BuildYear { get; set; }
    }
}