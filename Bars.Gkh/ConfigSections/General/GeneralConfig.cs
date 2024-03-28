namespace Bars.Gkh.ConfigSections.General
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Enums;

    using Config;
    using Config.Attributes;
    using Enums;

    /// <summary>
    /// Общие
    /// </summary>
    [GkhConfigSection("General", DisplayName = "Общие")]
    public class GeneralConfig : IGkhConfigSection
    {
        /// <summary>
        /// Формат адреса, используемый при сохранении карточки дома для последующего отображения в Реестре жилых домов.
        /// </summary>
        [GkhConfigProperty(DisplayName = "Формат адреса, используемый при сохранении карточки дома для последующего отображения в Реестре жилых домов.")]
        [DefaultValue(ShortAddressFormat.StartsFromUrbanArea)]
        public virtual ShortAddressFormat ShortAddressFormat { get; set; }

        /// <summary>
        /// Использовать идентификацию домов по GUID из ФИАС
        /// </summary>
        [GkhConfigProperty(DisplayName = "Использовать идентификацию домов по GUID из ФИАС")]
		[DefaultValue(UseFiasHouseIdentification.NotUse)]
		public virtual UseFiasHouseIdentification UseFiasHouseIdentification { get; set; }

        /// <summary>
        /// Ограничить размер добавляемых файлов МБ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничить размер добавляемых файлов МБ")]
        [DefaultValue(60)]
        [Range(1, uint.MaxValue)]
        public virtual uint? MaxUploadFileSize { get; set; }

        /// <summary>
        /// Автоматическое проставление признака "Дом не участвует в программе КР"
        /// </summary>
        [GkhConfigProperty(DisplayName = "Автоматическое проставление признака Дом не участвует в программе КР")]
        [DefaultValue(AutoIsNotInvolved.NotUse)]
        public virtual AutoIsNotInvolved AutoIsNotInvolved { get; set; }

        /// <summary>
        /// Ограничить размер добавляемых фотографий МБ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Ограничить размер добавляемых фотографий МБ")]
        [DefaultValue(null)]
        [Range(1, uint.MaxValue)]
        public virtual uint? MaxUploadPhotoSize { get; set; }
    }
}
