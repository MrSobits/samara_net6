namespace Bars.GkhGji.DomainService.View
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;

#warning поправить запрос
    public class ViewPrescriptionWidgetViewModel : BaseViewModel<ViewPrescriptionWidget>
    {
        public override IDataResult List(IDomainService<ViewPrescriptionWidget> domainService, BaseParams baseParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var activeOperator = userManager.GetActiveOperator();

            if (activeOperator == null)
            {
                return new ListDataResult { Success = false };
            }

            var prescriptionWidgetlData = this.Container.Resolve<IDomainService<ViewPrescriptionWidget>>()
                    .GetAll()
                    .Where(x => x.OperatorId == activeOperator.Id)
                    .AsEnumerable()
                    .Select(x => new { 
                                        x.Id, 
                                        Category = x.ContragentName,
                                        DatePrescription = x.DatePrescription.HasValue ? x.DatePrescription.Value.ToShortDateString() : string.Empty, 
                                        Header = x.Number, 
                                        LastDateViolation = x.LastDateViolation.HasValue ? x.LastDateViolation.Value.ToShortDateString() : string.Empty, 
                                        TypeDoc = "prescription" 
                    })
                    .AsEnumerable()
                    .Distinct();

            return new ListDataResult(prescriptionWidgetlData, prescriptionWidgetlData.Count());
        }
    }
}