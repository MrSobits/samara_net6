namespace Bars.GkhDi.GroupAction
{
    public interface IDiGroupAction
    {
        /// <summary>
        /// Отображаемое имя
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// Код действия
        /// </summary>
        string Code { get; }

        /// <summary>
        /// Иконка
        /// </summary>
        string Icon { get; set; }

        /// <summary>
        ////Контроллер
        /// </summary>
        string ControllerName { get; set; }

        /// <summary>
        /// Права
        /// </summary>
        string PermissionName { get; }

        /// <summary>
        /// Тип группового действия
        /// </summary>
        TypeDiTargetAction TypeDiTargetAction { get; }
    }
}
