namespace Bars.GkhGji.Regions.Samara.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;

    public class SamaraActCheckToProtocolRule : ActCheckToProtocolRule
    {
        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        public override IDataResult ValidationRule(DocumentGji document)
        {
            if (ActCheckDomain.GetAll().Any(x => x.Id == document.Id && x.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji))
            {
                return new BaseDataResult(false, "В Самаре для Акта проверки предписания нельзя формировать протокол");
            }

            return new BaseDataResult();
        }
    }
}
