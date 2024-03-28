namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Задание (необходимо для того что бы у основания предостережения вместо распоряжения отображалось Задание)
    /// </summary>
    public class TaskDisposal : GkhGji.Entities.Disposal
    {
        public TaskDisposal()
        {
            base.TypeDisposal = TypeDisposalGji.Base;
            base.TypeDocumentGji = TypeDocumentGji.TaskDisposal;
        }
    }
}