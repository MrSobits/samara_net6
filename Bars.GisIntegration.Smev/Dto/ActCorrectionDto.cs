namespace Bars.GisIntegration.Smev.Dto
{
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;

    /// <summary>
    /// DTO для создания информации об актах в теле запроса
    /// </summary>
    internal class ActCorrectionDto
    {
        /// <summary>
        /// Акт проверки (не размещенный в ЕРКНМ)
        /// </summary>
        public ISubjectActInsert SubjectActInsert { get; set; }

        /// <summary>
        /// Акт проверки (размещенный в ЕРКНМ)
        /// </summary>
        public ISubjectActUpdate SubjectActUpdate { get; set; }

        /// <summary>
        /// Должностное лицо, подписавшее акт проверки
        /// </summary>
        public IActTitleSignerUpdate ActTitleSignerUpdate { get; set; }

        /// <summary>
        /// Инспекторы из акта проверки
        /// </summary>
        public IActKnoInspectorsInsert[] ActKnoInspectorsInsert { get; set; }

        /// <summary>
        /// Документ со вкладки "Приложения" связанного акта
        /// </summary>
        public IActDocumentInsert ActDocumentInsert { get; set; }
    }
}