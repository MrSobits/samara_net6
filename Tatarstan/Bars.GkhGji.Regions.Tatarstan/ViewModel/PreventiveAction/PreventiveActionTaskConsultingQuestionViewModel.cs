﻿namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    public class PreventiveActionTaskConsultingQuestionViewModel : BaseViewModel<PreventiveActionTaskConsultingQuestion>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PreventiveActionTaskConsultingQuestion> domainService, BaseParams baseParams)
        {
            var taskId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .Where(x => x.Task.Id == taskId)
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}