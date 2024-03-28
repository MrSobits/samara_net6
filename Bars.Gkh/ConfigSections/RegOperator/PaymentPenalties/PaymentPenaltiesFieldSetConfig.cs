namespace Bars.Gkh.ConfigSections.RegOperator.PaymentPenalties
{
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Attributes;

    /// <summary>
    /// Группа для грида параметров начисления пени
    /// </summary>
    public class PaymentPenaltiesFieldSetConfig : IGkhConfigSection
    {
        /// <summary>
        /// Свойство для грида параметров начисления пени.
        /// Само по себе оно ничего не хранит, указанный едитор привязывается к котроллеру, который и управляет сущностями настраиваемых параметров
        /// Для них сделаны отдельные таблицы в БД и кастомное поведение на клиенте
        /// </summary>
        [GkhConfigProperty(DisplayName = "Статусы ЛС")]
        [GkhConfigPropertyEditor("B4.ux.config.PaymentPenaltiesEditor", "paymentpenaltieseditor")]
        public virtual int PaymentPenaltiesEditor { get; set; }
    }
}
