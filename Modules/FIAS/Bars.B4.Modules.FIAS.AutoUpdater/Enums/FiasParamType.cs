namespace Bars.B4.Modules.FIAS.AutoUpdater.Enums
{
    /// <summary>
    /// Типы ФИАС параметров
    /// </summary>
    public enum FiasParamType
    {
        /// <summary>
        /// Код ИФНС ФЛ
        /// </summary>
        IFNSFL = 1,
        
        /// <summary>
        /// Код ИФНС ЮЛ
        /// </summary>
        IFNSUL = 2,
        
        /// <summary>
        /// Код территориального участка ИФНС ФЛ
        /// </summary>
        TerritoryIFNSUL = 3,
        
        /// <summary>
        /// Код территориального участка ИФНС ЮЛ
        /// </summary>
        TerritoryIFNSFL = 4,
        
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        PostalCode = 5,
        
        /// <summary>
        /// ОКАТО
        /// </summary>
        OKATO = 6,
        
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        CadastrNumber = 8,
        
        /// <summary>
        /// Код адресного элемента
        /// </summary>
        AddressCode = 10,
        
        /// <summary>
        /// Код региона
        /// </summary>
        RegionCode = 12,
        
        /// <summary>
        /// Официальное наименование
        /// </summary>
        OfficialName = 16
    }
}