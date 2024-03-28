namespace Bars.Gkh.Report.TechPassportSections
{
    using System.Linq;
    using Gkh.PassportProvider;

    public class Section1_1: BaseTechPassportSectionReport
    {
        private RealtyObjectTypeWorkCr[] _realtyObjectTypeWorkCrs; 

        protected override void PrepareComponentIds(){}

        protected override void GetData()
        {
            if (!this.Container.Kernel.HasComponent(typeof(IRealtyObjTypeWorkProvider)))
            {
                return;
            }

            var objTypeWorkProvider = this.Container.Resolve<IRealtyObjTypeWorkProvider>();

            _realtyObjectTypeWorkCrs = objTypeWorkProvider.GetWorks(RealtyObjectId).ToArray();


        }

        protected override void PlaceData()
        {
            var section = ReportParams.ComplexReportParams.ДобавитьСекцию("sectionForm_1_1");

            if (_realtyObjectTypeWorkCrs == null)
            {
                _realtyObjectTypeWorkCrs = new RealtyObjectTypeWorkCr[0];
            }

            foreach (var row in _realtyObjectTypeWorkCrs.OrderBy(x => x.PeriodName))
            {
                section.ДобавитьСтроку();
                section["Период"] = row.PeriodName;
                section["ВидРаботы"] = row.WorkName;
            }
        }
    }
}