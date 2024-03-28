namespace Bars.GkhDi.GroupAction
{
    /// <summary>
    /// Копирование сведений об услугах из периода в период
    /// </summary>
    public class DiCopyServicePeriod : IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
         public string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        public string Code => nameof(DiCopyServicePeriod);

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

         public string PermissionName { get { return "GkhDi.DisinfoRealObj.Actions.CopyServicePeriod"; } }

         public DiCopyServicePeriod()
        {
            this.Name = "Копирование сведений об услугах из периода в период";
            this.ControllerName = "B4.controller.CopyServicePeriodController";
            this.Icon = "icon-arrow-divide";
        }
    }
}
