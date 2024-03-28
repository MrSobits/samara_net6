namespace Bars.GkhGji.Regions.Zabaykalye.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Протокол'
    /// </summary>
    public class ZabaykalyeProtocolToResolutionRule : ProtocolToResolutionRule
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGJIPhysPersonInfoDomain { get; set; }

        // Копирование Реквизитов физ. лица, которому выдан документ ГЖИ
        public override void CopyingPhysicalPersons(long protocolId, ref Resolution resolution)
        {
            var protocolPhysPersonInfo = DocumentGJIPhysPersonInfoDomain.GetAll()
                .Where(x => x.Document.Id == protocolId)
                .Select(x => new
                {
                    x.ObjectCreateDate,
                    x.PhysPersonAddress,
                    x.PhysPersonBirthdayAndPlace,
                    x.PhysPersonDocument,
                    x.PhysPersonJob,
                    x.PhysPersonMaritalStatus,
                    x.PhysPersonPosition,
                    x.PhysPersonSalary
                })
                .OrderByDescending(x => x.ObjectCreateDate)
                .FirstOrDefault();

            if (protocolPhysPersonInfo != null)
            {
                DocumentGJIPhysPersonInfoDomain.Save(
                    new DocumentGJIPhysPersonInfo
                    {
                        Document = resolution,
                        PhysPersonAddress = protocolPhysPersonInfo.PhysPersonAddress,
                        PhysPersonBirthdayAndPlace = protocolPhysPersonInfo.PhysPersonBirthdayAndPlace,
                        PhysPersonDocument = protocolPhysPersonInfo.PhysPersonDocument,
                        PhysPersonJob = protocolPhysPersonInfo.PhysPersonJob,
                        PhysPersonMaritalStatus = protocolPhysPersonInfo.PhysPersonMaritalStatus,
                        PhysPersonPosition = protocolPhysPersonInfo.PhysPersonPosition,
                        PhysPersonSalary = protocolPhysPersonInfo.PhysPersonSalary
                    });
            }
        }
    }
}
