namespace Bars.Gkh.Reforma.GroupAction.Impl
{
    using Bars.GkhDi.GroupAction;

    /// <summary>
    /// Ручная интеграция по УО
    /// </summary>
    public class ReformaRealityObjManualIntegration : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(ReformaRealityObjManualIntegration);

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
        public string PermissionName => "GkhDi.Reforma.ManualIntegration.RealityObj";

        /// <summary>
        /// Тип группового действия
        /// </summary>
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.RealityObject;

        /// <summary>
        /// Конструктор
        /// </summary>
        public ReformaRealityObjManualIntegration()
        {
            this.Name = "Провести интеграцию по домам";
            this.ControllerName = "B4.controller.manualintegration.RealityObjectIntegration";
            this.Icon = "icon-application-get";
        }
    }
}