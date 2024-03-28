namespace Bars.GkhDiCr.GroupAction
{
    using Bars.GkhDi.GroupAction;

    /// <summary>
    /// Загрузка сведений по капитальному ремонту из модуля КР
    /// </summary>
    public class DiCopyCrServiceGroupAction : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(DiCopyCrServiceGroupAction);

        /// <summary>
        /// Иконка
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Тип группового действия
        /// </summary>
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.RealityObject;

        /// <summary>
        /// Контроллер
        /// </summary>
        public string ControllerName { get; set; }

        public string PermissionName
        {
            get { return "GkhDi.DisinfoRealObj.Actions.CopyCrServiceGroupAction"; }
        }

        public DiCopyCrServiceGroupAction()
        {
            this.Name = "Загрузка сведений по капитальному ремонту из модуля КР";
            this.ControllerName = "B4.controller.CopyCr";
            this.Icon = "icon-application-get";
        }
    }
}
