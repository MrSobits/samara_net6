namespace Bars.Gkh.ConfigSections.Administration.FormatDataExport
{
    using System.ComponentModel;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Config.Attributes.UI;
    using Bars.Gkh.Dto;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Параметры экспорта
    /// </summary>
    [DisplayName(@"Параметры экспорта")]
    public class FormatDataExportParamsConfig : IGkhConfigSection
    {
        /// <summary>
        /// Контрагент ГЖИ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Контрагент ГЖИ")]
        [GkhConfigPropertyEditor("B4.ux.config.ContragentSelectField", "contragentselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Contragent> GjiContragent { get; set; }

        /// <summary>
        /// Должность руководителя
        /// </summary>
        [GkhConfigProperty(DisplayName = "Должность руководителя")]
        [GkhConfigPropertyEditor("B4.ux.config.PositionSelectField", "positionselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Position> LeaderPosition { get; set; }

        /// <summary>
        /// Должность бухгалтера
        /// </summary>
        [GkhConfigProperty(DisplayName = "Должность бухгалтера")]
        [GkhConfigPropertyEditor("B4.ux.config.PositionSelectField", "positionselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Position> AccountantPosition { get; set; }

        /// <summary>
        /// Должность председателя правления ТСЖ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Должность председателя правления ТСЖ")]
        [GkhConfigPropertyEditor("B4.ux.config.PositionSelectField", "positionselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Position> TsjChairmanPosition { get; set; }

        /// <summary>
        /// Должность члена правления ТСЖ
        /// </summary>
        [GkhConfigProperty(DisplayName = "Должность члена правления ТСЖ")]
        [GkhConfigPropertyEditor("B4.ux.config.PositionSelectField", "positionselectfield")]
        [UIExtraParam("maxWidth", FormatDataExportConfig.MaxWidth)]
        public virtual EntityDto<Position> TsjMemberPosition { get; set; }
    }
}