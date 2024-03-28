namespace Bars.Gkh.ConfigSections.ClaimWork.Debtor
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Настройки реструктуризации
    /// </summary>
    public class RestructDebtConfig : IGkhConfigSection
    {
        /// <summary>
        /// Печать документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Печать документа")]
        [DefaultValue(PrintType.Print)]
        public virtual PrintType PrintType { get; set; }

        /// <summary>
        /// Карточка документа
        /// </summary>
        [GkhConfigProperty(DisplayName = "Карточка документа")]
        [DefaultValue(DocumentFormationKind.Form)]
        public virtual DocumentFormationKind RestructDebtFormKind { get; set; }

        /// <summary>
        /// Допустимый срок просрочки оплаты
        /// </summary>
        [GkhConfigProperty(DisplayName = "Допустимый срок просрочки оплаты")]
        [DefaultValue(0)]
        [Range(0, 31)]
        public virtual int AllowDelayPaymentDays { get; set; }
    }
}