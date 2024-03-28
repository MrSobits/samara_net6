namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using System;
    using Bars.Gkh.Decisions.Nso.Entities.Decisions;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Отложенный перевод статуса счета в "Неактивно".
    /// Необходимость вознкла в связи с добавлением "Протокола решений госорганов" 
    /// <see cref="Bars.Gkh.Decisions.Nso.Entities.Decisions.GovDecision"/>
    /// </summary>
    public class DefferedUnactivation : BaseImportableEntity
    {
        /// <summary>
        /// Дата, когда счета дома будут переведены в статус неактивно
        /// </summary>
        public virtual DateTime UnactivationDate { get; set; }

        /// <summary>
        /// Дом, счета которого будут переведены в статус некативно
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Дом был обработан
        /// </summary>
        public virtual bool Processed { get; set; }

        /// <summary>
        /// Решение, по которому лицевые счета будут переведены в статус неактивно
        /// </summary>
        public virtual GovDecision GovDecision { get; set; }
    }
}
