﻿namespace Bars.GkhGji.Regions.Nnovgorod.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;

    public class ResolutionServiceInterceptor : GkhGji.Interceptors.ResolutionServiceInterceptor
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<Resolution> service, Resolution entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return result;
            }

            this.DocumentGjiPhysPersonInfoDomain.GetAll()
                .Where(x => x.Document.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => this.DocumentGjiPhysPersonInfoDomain.Delete(x));

            return this.Success();
        }
    }
}