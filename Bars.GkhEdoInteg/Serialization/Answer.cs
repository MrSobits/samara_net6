namespace Bars.GkhEdoInteg.Serialization
{
    /// <summary>
    /// Ответ оп обращению
    /// </summary>
    public sealed class Answer
    {
        public long Id { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// От
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// Исполнитель (id и ФИО из справочника инспекторов)
        /// </summary>
        public long InspectorId { get; set; }

        public string Inspector { get; set; }

        public string ContentAnswer { get; set; }

        public string Description { get; set; }
    }
}
