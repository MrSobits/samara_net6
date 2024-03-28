namespace Bars.GkhGji.Regions.Tula.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// В Сахе нельзя создавать распоряжение из основания если еще не заполнены дома
    /// </summary>
    public class TulaBaseDispHeadToDisposalRule : Bars.GkhGji.InspectionRules.BaseDispHeadToDisposalRule
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
