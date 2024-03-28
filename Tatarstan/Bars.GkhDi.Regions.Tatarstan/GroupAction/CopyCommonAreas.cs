namespace Bars.GkhDi.Regions.Tatarstan.GroupAction
{
    using Bars.GkhDi.GroupAction;

    public class CopyCommonAreas : IDiGroupAction
    {
        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public string Code => nameof(CopyCommonAreas);

        /// <inheritdoc />
        public string Icon { get; set; }

        /// <inheritdoc />
        public string ControllerName { get; set; }

        /// <inheritdoc />
        public string PermissionName => "GkhDi.DisinfoRealObj.Actions.CopyCommonAreas";

        /// <inheritdoc />
        public TypeDiTargetAction TypeDiTargetAction => TypeDiTargetAction.RealityObject;

        public CopyCommonAreas()
        {
            this.Name = "Копирование сведений об использовании мест общего пользования из периода в период";
            this.ControllerName = "B4.controller.CopyCommonAreasController";
            this.Icon = "icon-arrow-divide";
        }
    }
}