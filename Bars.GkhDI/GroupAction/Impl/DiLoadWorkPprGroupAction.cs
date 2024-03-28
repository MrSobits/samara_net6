namespace Bars.GkhDi.GroupAction
{
    /// <summary>
    /// Заполнение сведений по работам по текущему ремонту
    /// </summary>
    public class DiLoadWorkPprGroupAction : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
         public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(DiLoadWorkPprGroupAction);

        /// <summary>
         /// Иконка
         /// </summary>
         public string Icon { get; set; }

        /// <summary>
        /// Контроллер
        /// </summary>
         public string ControllerName { get; set; }

        /// <summary>
        /// Тип группового действия
        /// </summary>
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.RealityObject;

        public string PermissionName { get { return "GkhDi.DisinfoRealObj.Actions.LoadWorkPprGroupAction";}}

        public DiLoadWorkPprGroupAction()
        {
            this.Name = "Заполнение сведений по работам по текущему ремонту";
            this.ControllerName = "B4.controller.LoadWorkController";
            this.Icon = "icon-application-form-edit";
        }
    }
}
