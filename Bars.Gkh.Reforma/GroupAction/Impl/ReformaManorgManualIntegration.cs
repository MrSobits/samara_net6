namespace Bars.Gkh.Reforma.GroupAction.Impl
{
    using Bars.GkhDi.GroupAction;

    /// <summary>
    /// Ручная интеграция по УО
    /// </summary>
    public class ReformaManorgManualIntegration : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(ReformaManorgManualIntegration);

        /// <summary>
        /// Иконка
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        ////Контроллер
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Права
        /// </summary>
        public string PermissionName => "GkhDi.Reforma.ManualIntegration.ManagingOrganization";

        /// <summary>
        /// Тип группового действия
        /// </summary>
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.ManagingOrganization;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ReformaManorgManualIntegration()
        {
            this.Name = "Провести интеграцию по УО";
            this.ControllerName = null;
            this.Icon = "icon-application-get";
        }
    }
}