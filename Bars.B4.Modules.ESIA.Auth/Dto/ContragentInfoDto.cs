namespace Bars.B4.Modules.ESIA.Auth.Dto
{
    /// <summary>
    /// Информация о контрагенте
    /// </summary>
    public class ContragentInfoDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }
    }
}