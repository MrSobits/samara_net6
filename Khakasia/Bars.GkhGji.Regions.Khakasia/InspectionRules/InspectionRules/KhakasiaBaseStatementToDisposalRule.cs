namespace Bars.GkhGji.Regions.Khakasia.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// в Хакасии нельзя создать приказ пока нет домов в доменте 
    /// </summary>
    public class KhakasiaBaseStatementToDisposalRule : Bars.GkhGji.InspectionRules.BaseStatementToDisposalRule
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
