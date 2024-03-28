namespace Bars.GkhGji.Regions.Tomsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Report;

    public class MonthlyProsecutorsOfficeServiceData : Bars.GkhGji.Report.MonthlyProsecutorsOfficeServiceData
    {
        protected override void FillValuesInRows3(MonthlyProsecutorsOfficeData data, List<DisposalProxy> disposalsYear, List<DisposalProxy> disposalsMonth)
        {
            data.param3_1 = disposalsYear.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
            data.param3_2 = disposalsMonth.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
        }

        protected override void FillValuesInRows6(MonthlyProsecutorsOfficeData data, List<PrescriptionProxy> prescriptionsYear, List<PrescriptionProxy> prescriptionsMonth)
        {
            data.param6_1 = prescriptionsYear.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
            data.param6_2 = prescriptionsMonth.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
        }

        protected override void FillValuesInRows10(MonthlyProsecutorsOfficeData data, List<ResolutionProxy> resolutionsYear, List<ResolutionProxy> resolutionsMonth)
        {
            data.param10_1 = resolutionsYear.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
            data.param10_2 = resolutionsMonth.Where(x => x.TypeEntrepreneurship == TypeEntrepreneurship.Average || x.TypeEntrepreneurship == TypeEntrepreneurship.Small).Select(x => x.Id).Distinct().Count();
        } 
    }
}
