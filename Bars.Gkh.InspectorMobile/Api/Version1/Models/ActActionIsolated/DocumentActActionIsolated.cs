namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActActionIsolated
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Enums;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Newtonsoft.Json;

    /// <summary>
    /// Модель получения документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public class DocumentActActionIsolatedGet : DocumentActCheckGet
    {
        #region Ignored propeties

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckProvidedDocumentGet> ProvidedDocuments { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckInspectedPartGet> InspectedParts { get; set; }
        
        /// <inheritdoc />
        [JsonIgnore]
        public override ActCheckChildrenDocumentType[] ChildrenDocumentTypes { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckWitnessGet> Witnesses { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override AcquaintState? AcquaintedStatus { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override long? ExecutiveId { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? AcquaintedDate { set; get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialFullName { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialPosition { get; set; }

        #endregion
        
        /// <summary>
        /// Признак того, что у документа "Акт КНМ" есть дочерние документы в НЕ конечном статусе
        /// </summary>
        /// <remarks>
        /// override ради summary
        /// </remarks>
        public override bool RelatedDocuments { get; set; }

        /// <summary>
        /// Результаты проведения мероприятия
        /// </summary>
        /// <remarks>
        /// override ради summary
        /// </remarks>
        [Required]
        public override IEnumerable<ActCheckEventResultGet> EventResults { get; set; }

    }

    /// <summary>
    /// Модель создания документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public class DocumentActActionIsolatedCreate : DocumentActCheckCreate
    {
        /// <inheritdoc />
        [RequiredExistsEntity(typeof(TaskActionIsolated))]
        public override long? ParentDocumentId { get; set; }

        #region Ignored propeties

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckProvidedDocumentCreate> ProvidedDocuments { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckInspectedPartCreate> InspectedParts { get; set; }
        
        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckWitnessCreate> Witnesses { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override AcquaintState? AcquaintedStatus { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? AcquaintedDate { set; get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialFullName { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialPosition { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override long? ExecutiveId { get; set; }

        #endregion
        
        /// <summary>
        /// Результаты проведения мероприятия
        /// </summary>
        /// <remarks>
        /// override ради summary
        /// </remarks>
        [Required]
        public override IEnumerable<ActCheckEventResultCreate> EventResults { get; set; }
    }

    /// <summary>
    /// Модель обновления документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    public class DocumentActActionIsolatedUpdate : DocumentActCheckUpdate
    {
        #region Ignored propeties
        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckProvidedDocumentUpdate> ProvidedDocuments { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckInspectedPartUpdate> InspectedParts { get; set; }
        
        /// <inheritdoc />
        [JsonIgnore]
        public override IEnumerable<ActCheckWitnessUpdate> Witnesses { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override AcquaintState? AcquaintedStatus { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? AcquaintedDate { set; get; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialFullName { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override string OfficialPosition { get; set; }

        /// <inheritdoc />
        [JsonIgnore]
        public override long? ExecutiveId { get; set; }

        #endregion
        
        /// <summary>
        /// Результаты проведения мероприятия
        /// </summary>
        /// <remarks>
        /// override ради summary
        /// </remarks>
        [Required]
        public override IEnumerable<ActCheckEventResultUpdate> EventResults { get; set; }
    }
}