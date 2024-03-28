namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;

    /// <summary>
    /// Вложение документа "Предписание". Модель выборки
    /// </summary>
    public class PrescriptionAnnexGet : FileInfoGet
    {
        /// <inheritdoc />
        public override long? FileId { get; set; }
    }
    
    /// <summary>
    /// Вложение документа "Предписание". Модель создания
    /// </summary>
    public class PrescriptionAnnexCreate : FileInfoCreate
    {
        /// <inheritdoc />
        [Required]
        public override DateTime? FileDate { get; set; }

        /// <inheritdoc />
        [Required]
        public override string FileDescription { get; set; }

        /// <inheritdoc />
        [Required]
        public override string FileName { get; set; }
    }
}