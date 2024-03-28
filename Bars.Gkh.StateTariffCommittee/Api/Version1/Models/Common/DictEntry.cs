namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Models.Common
{
    /// <summary>
    /// Запись справочника с кодом
    /// </summary>
    public class DictCodeEntry
    {
        /// <summary>
        /// Код записи справочника
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Наименование записи справочника
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Запись справочника с идентификатором
    /// </summary>
    public class DictIdEntry
    {
        /// <summary>
        /// Уникальный идентификатор записи справочника
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование записи справочника
        /// </summary>
        public string Name { get; set; }
    }
}