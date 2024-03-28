using Bars.B4.IoC;
using Bars.GkhGji.Enums;
using Stimulsoft.Report;

namespace Bars.GkhGji.Regions.Tula.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tula.Entities;

    using Bars.B4.Utils;

    public class ProtocolServiceInterceptor : GkhGji.Interceptors.ProtocolServiceInterceptor
    {
	    public override IDataResult BeforeUpdateAction(IDomainService<Protocol> service, Protocol entity)
	    {
			var documentGjiDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
		    using (Container.Using(documentGjiDomain))
		    {
			    var actCheckMaxDate = documentGjiDomain.GetAll()
				    .Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    .Where(x => x.Parent.DocumentDate.HasValue)
				    .Max(x => x.Parent.DocumentDate);

			    if (entity.DocumentDate < actCheckMaxDate)
			    {
				    return Failure("Дата протокола не должна быть раньше даты акта проверки");
			    }

			    var actRemovalMaxDate = documentGjiDomain.GetAll()
				    .Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .Where(x => x.Parent.DocumentDate.HasValue)
				    .Max(x => x.Parent.DocumentDate);

			    if (entity.DocumentDate < actRemovalMaxDate)
			    {
				    return Failure("Дата протокола не должна быть раньше даты акта проверки предписания");
			    }
		    }

            return base.BeforeUpdateAction(service, entity);
	    }

	    public override IDataResult BeforeDeleteAction(IDomainService<Protocol> service, Protocol entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();
            var physPersonInfoDomain = Container.Resolve<IDomainService<DocumentGJIPhysPersonInfo>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return result;
                }

                // удаляем инфомацию по физ лицу
                physPersonInfoDomain.GetAll()
                    .Where(x => x.Document.Id == entity.Id)
                    .Select(x => x.Id)
                    .ForEach(x => physPersonInfoDomain.Delete(x));

                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                               .Where(x => x.Document.Id == entity.Id)
                               .Select(item => item.Id)
                               .ToList()
                               .ForEach(
                                   x =>
                                   {
                                       try
                                       {
                                           violGroupDomain.Delete(x);
                                       }
                                       catch(Exception exception)
                                       {
                                       }
                                       
                                   });

                return Success();
            }
            finally 
            {
                Container.Release(violGroupDomain);
                Container.Release(physPersonInfoDomain);
            }
            
        }
    }
}
