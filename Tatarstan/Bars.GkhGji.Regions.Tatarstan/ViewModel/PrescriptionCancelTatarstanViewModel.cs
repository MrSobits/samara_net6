namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.ViewModel;

    public class PrescriptionCancelTatarstanViewModel : PrescriptionCancelViewModel<PrescriptionCancelTatarstan>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PrescriptionCancelTatarstan> prescriptionCancelTatarstan, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var loadParams = this.GetLoadParam(baseParams);

            return prescriptionCancelTatarstan.GetAll()
                .Where(w => w.Prescription.Id == documentId)
                .Select(s => new
                {
                    s.Id,
                    s.DocumentDate,
                    s.DocumentNum,
                    s.DateCancel,
                    s.IsCourt,
                    s.Reason,
                    IssuedCancel = s.IssuedCancel.Fio,
                    s.DateDecisionCourt,
                    DecisionMakingAuthority = s.DecisionMakingAuthority.Name,
                    s.TypeCancel,
                    s.Date,
                    s.DocumentNumber,
                    s.OutMailDate,
                    s.OutMailNumber,
                    s.NotificationTransmission,
                    s.NotificationReceive,
                    s.NotificationType,
                    s.ProlongationDate
                })
                .ToListDataResult(loadParams);
        }
    }
}