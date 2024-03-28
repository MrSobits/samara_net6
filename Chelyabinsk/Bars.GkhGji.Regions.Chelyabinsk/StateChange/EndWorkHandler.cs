namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;

    /// <summary>
    /// Обработчик закрытия обращения при успешной отпавке в ОГ Челябинск
    /// </summary>
    public class EndWorkHandler : IStateChangeHandler
    {
        public IDomainService<State> StateDomain { get; set; }
        public IDomainService<AppealCitsTransferResult> AppealCitsTransferResultDomain { get; set; }

        /// <inheritdoc />
        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            var appeal = entity as AppealCits;
            if (!(appeal?.AppealUid.HasValue ?? false))
            {
                return;
            }

            var states = this.StateDomain.GetAll()
                .Where(x => x.TypeId == AppealCits.TypeId)
                .ToArray();

            var closeState = states
                .FirstOrDefault(x => x.Code == AppealCits.Closed);
            var cancelState = states
                .FirstOrDefault(x => x.Code == AppealCits.Cancelled);

            var isSuccess = false;

            if (newState.Code == AppealCits.NotAccepted)
            {
                isSuccess = this.AppealCitsTransferResultDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appeal.Id)
                    .Where(x => x.Type == AppealCitsTransferType.ExportInfoAcceptWork)
                    .Where(x => x.EndDate > DateTime.Today)
                    .Any(x => x.Status == AppealCitsTransferStatus.Success);
            }

            if (newState.Code == AppealCits.Cancelled)
            {
                isSuccess = this.AppealCitsTransferResultDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appeal.Id)
                    .Where(x => x.Type == AppealCitsTransferType.ExportInfoCitizensAppealCancel)
                    .Where(x => x.EndDate > DateTime.Today)
                    .Any(x => x.Status == AppealCitsTransferStatus.Success);
            }

            if (cancelState != null && isSuccess)
            {
                entity.State = cancelState;
            }

            if (newState.Code == AppealCits.Success || newState.Code == AppealCits.Failure)
            {
                isSuccess = this.AppealCitsTransferResultDomain.GetAll()
                    .Where(x => x.AppealCits.Id == appeal.Id)
                    .Where(x => x.Type == AppealCitsTransferType.ExportInfoCompletionOfWork)
                    .Where(x => x.EndDate > DateTime.Today)
                    .Any(x => x.Status == AppealCitsTransferStatus.Success);

                if (closeState != null && isSuccess)
                {
                    entity.State = closeState;
                }
            }
        }
    }
}