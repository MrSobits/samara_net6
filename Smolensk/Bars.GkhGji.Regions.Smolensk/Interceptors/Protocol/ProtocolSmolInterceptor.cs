namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.GkhGji.Regions.Smolensk.Entities.Protocol;

    public class ProtocolSmolInterceptor : ProtocolServiceInterceptor<ProtocolSmol>
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public IDomainService<ProtocolViolationDescription> DescriptionService { get; set; }

        public IDomainService<DocumentViolGroup> ViolGroupDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<ProtocolSmol> service, ProtocolSmol entity)
        {
            var description = this.DescriptionService.GetAll().FirstOrDefault(x => x.Protocol.Id == entity.Id);
            if (description != null)
            {
                this.DescriptionService.Delete(description.Id);
            }

            this.DocumentGjiPhysPersonInfoDomain.GetAll().Where(x => x.Document.Id == entity.Id).Select(x => x.Id).ForEach(x => this.DocumentGjiPhysPersonInfoDomain.Delete(x));

            this.Container.InTransaction(() =>
            {  
                // удаляем связку документа с группой нарушений
                this.ViolGroupDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(item => item.Id)
                    .ToList()
                    .ForEach(x => this.ViolGroupDomain.Delete(x));

            });

            return base.BeforeDeleteAction(service, entity);
        }

    }
}