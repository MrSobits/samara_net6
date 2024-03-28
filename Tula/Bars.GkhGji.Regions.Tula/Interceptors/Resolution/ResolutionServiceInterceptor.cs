using Bars.B4.IoC;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Tula.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ResolutionServiceInterceptor : GkhGji.Interceptors.ResolutionServiceInterceptor
    {
        public IDomainService<DocumentGJIPhysPersonInfo> DocumentGjiPhysPersonInfoDomain { get; set; }

	    public override IDataResult BeforeUpdateAction(IDomainService<Resolution> service, Resolution entity)
	    {
			var documentGjiDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
			using (Container.Using(documentGjiDomain))
			{
				var protocolMaxDate = documentGjiDomain.GetAll()
					.Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Protocol)
                    .Where(x => x.Parent.DocumentDate.HasValue)
					.Max(x => x.Parent.DocumentDate);

				if (entity.DocumentDate < protocolMaxDate)
				{
					return Failure("Дата постановления не должна быть раньше даты протокола");
				}
			}

            return base.BeforeUpdateAction(service, entity);
	    }

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

            return base.BeforeDeleteAction(service, entity);
        }
    }
}