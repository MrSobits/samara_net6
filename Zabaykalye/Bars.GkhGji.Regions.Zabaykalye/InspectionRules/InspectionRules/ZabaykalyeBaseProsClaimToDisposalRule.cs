﻿namespace Bars.GkhGji.Regions.Zabaykalye.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// в сахе нельзя создать приказ пока не добавлены дома в основание
    /// </summary>
    public class ZabaykalyeBaseProsClaimToDisposalRule : Bars.GkhGji.InspectionRules.BaseProsClaimToDisposalRule
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
