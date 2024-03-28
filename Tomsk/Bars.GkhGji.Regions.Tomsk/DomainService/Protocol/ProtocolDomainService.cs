namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Controller;
    using Entities;
    using Enums;
    using GkhGji.DomainService;
    using GkhGji.Entities;

    public class ProtocolDomainService : ProtocolDomainService<TomskProtocol>
    {
        public IDomainService<DocumentPhysInfo> DocumentPhysInfoDomain { get; set; }

        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<TomskProtocol>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    UpdatePhysPersonRequisites(record);
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);
                }
            });

            return new BaseDataResult(values);
        }

        private void UpdatePhysPersonRequisites(SaveRecord<TomskProtocol> protocol)
        {
            var info = DocumentPhysInfoDomain.GetAll()
                .FirstOrDefault(x => x.Document.Id == protocol.Entity.Id);

            var properties = protocol.NonObjectProperties;

            if (info == null)
            {
                info = new DocumentPhysInfo
                {
                    Document = new DocumentGji {Id = protocol.Entity.Id},
                    PhysAddress = properties.Get("PhysAddress").ToStr(),
                    PhysBirthdayAndPlace = properties.Get("PhysBirthdayAndPlace").ToStr(),
                    PhysIdentityDoc = properties.Get("PhysIdentityDoc").ToStr(),
                    PhysJob = properties.Get("PhysJob").ToStr(),
                    PhysPosition = properties.Get("PhysPosition").ToStr(),
                    TypeGender = properties.GetAs<TypeGender>("TypeGender")
                };

                DocumentPhysInfoDomain.Save(info);
            }
            else
            {
                info.PhysAddress = properties.GetAs<string>("PhysAddress") ?? info.PhysAddress;
                info.PhysBirthdayAndPlace = properties.GetAs<string>("PhysBirthdayAndPlace") ?? info.PhysBirthdayAndPlace;
                info.PhysIdentityDoc = properties.GetAs<string>("PhysIdentityDoc") ?? info.PhysIdentityDoc;
                info.PhysJob = properties.GetAs<string>("PhysJob") ?? info.PhysJob;
                info.PhysPosition = properties.GetAs<string>("PhysPosition") ?? info.PhysPosition;
                info.TypeGender = properties.GetAs<TypeGender>("TypeGender");

                DocumentPhysInfoDomain.Save(info);
            }
        }
    }
}