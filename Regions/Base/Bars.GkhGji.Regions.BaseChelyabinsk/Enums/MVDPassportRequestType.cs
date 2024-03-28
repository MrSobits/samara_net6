namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса документов
    /// </summary>
    public enum MVDPassportRequestType
    {

        /// <summary>
        /// Плановая проверка
        /// </summary>
        [Display("По данным физлица")]
        PersonInfo = 10,

        /// <summary>
        /// Контрольная закупка
        /// </summary>
        [Display("По данным паспорта РФ")]
        RussianPassport = 20
    }
}