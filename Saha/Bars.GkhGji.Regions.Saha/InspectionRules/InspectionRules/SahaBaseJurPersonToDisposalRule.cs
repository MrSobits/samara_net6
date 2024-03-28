namespace Bars.GkhGji.Regions.Saha.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// в сахе нельзя сформироть приказ если дома не выбраны
    /// </summary>
    public class SahaBaseJurPersonToDisposalRule :Bars.GkhGji.InspectionRules.BaseJurPersonToDisposalRule
    {
        public IDomainService<InspectionGjiRealityObject> insRoDomain { get; set; }

        public override IDataResult CreateDocument(InspectionGji inspection)
        {

            if (!insRoDomain.GetAll().Any(x => x.Inspection.Id == inspection.Id))
            {
                return new BaseDataResult(false, "Необходимо добавить проверяемые дома.");
            }

            return base.CreateDocument(inspection);
        }
    }
}
