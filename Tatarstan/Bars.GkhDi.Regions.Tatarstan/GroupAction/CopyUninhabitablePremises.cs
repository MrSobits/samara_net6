namespace Bars.GkhDi.Regions.Tatarstan.GroupAction
{
    using Bars.GkhDi.GroupAction;

    /// <summary>
    /// Копирование сведений об использовании нежилых помещений из периода в период
    /// </summary>
    public class CopyUninhabitablePremises : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(CopyUninhabitablePremises);

        /// <summary>
        /// Иконка
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Контроллер
        /// </summary>
        public string ControllerName { get; set; }

        /// <summary>
        /// Права доступа
        /// </summary>
        public string PermissionName => "GkhDi.DisinfoRealObj.Actions.CopyUninhabitablePremises";

        /// <summary>
        /// Тип группового действия
        /// </summary>
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.RealityObject;

        /// <summary>
        /// Конструктор
        /// </summary>
        public CopyUninhabitablePremises()
        {
            this.Name = "Копирование сведений об использовании нежилых помещений из периода в период";
            this.ControllerName = "B4.controller.CopyUninhabitablePremisesController";
            this.Icon = "icon-arrow-divide";
        }
    }
}