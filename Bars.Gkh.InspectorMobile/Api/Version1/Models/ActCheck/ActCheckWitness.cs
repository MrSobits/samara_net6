namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck
{
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Модель получения "Лицо, присутствующие при проверке (или свидетель)"
    /// </summary>
    public class ActCheckWitnessGet : BaseActCheckWitness
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        public long Id { get; set; }
    }

    /// <summary>
    /// Модель создания "Лицо, присутствующие при проверке (или свидетель)"
    /// </summary>
    public class ActCheckWitnessCreate : BaseActCheckWitness
    {
    }

    /// <summary>
    /// Модель обновления "Лицо, присутствующие при проверке (или свидетель)"
    /// </summary>
    public class ActCheckWitnessUpdate : BaseActCheckWitness, INestedEntityId
    {
        /// <inheritdoc />
        [OnlyExistsEntity(typeof(ActCheckWitness))]
        public long? Id { get; set; }
    }

    /// <summary>
    /// Базовая модель "Лицо, присутствующие при проверке (или свидетель)"
    /// </summary>
    public class BaseActCheckWitness
    {
        /// <summary>
        /// ФИО лица, присутствующего при проверке
        /// </summary>
        [Required]
        public string FullName { get; set; }

        /// <summary>
        /// Должность лица, присутствующего при проверке
        /// </summary>
        public string Position { get; set; }

        /// <summary>
        /// Признак ознакомления лица, присутствующего при проверке с составленным актом проверки
        /// </summary>
        public bool SignAcquainted { get; set; }
    }
}