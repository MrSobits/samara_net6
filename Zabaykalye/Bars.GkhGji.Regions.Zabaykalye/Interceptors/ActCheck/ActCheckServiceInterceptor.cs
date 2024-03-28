namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;

    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    using Bars.B4.Utils;

    public class ActCheckServiceInterceptor : GkhGji.Interceptors.ActCheckServiceInterceptor
    {
	    public override IDataResult BeforeUpdateAction(IDomainService<ActCheck> service, ActCheck entity)
	    {
		    var actCheckPeriodsDomain = Container.Resolve<IDomainService<ActCheckPeriod>>();
		    using (Container.Using(actCheckPeriodsDomain))
		    {
			    var lastCheckDate = actCheckPeriodsDomain.GetAll()
				    .Where(x => x.ActCheck.Id == entity.Id)
				    .Max(x => x.DateCheck);

			    if (entity.DocumentDate < lastCheckDate)
			    {
				    return Failure("Дата акта проверки не должна быть раньше даты проведения проверки");
			    }
		    }

	        var documentGjiDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            using (Container.Using(documentGjiDomain))
            {
                var maxDate = documentGjiDomain.GetAll()
                        .Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => x.Parent.DocumentDate.HasValue)
                        .Max(x => x.Parent.DocumentDate);

                if (entity.DocumentDate < maxDate)
                {
                    if (entity.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji)
                    {
                        return Failure("Дата акта не должна быть раньше даты приказа на проверку предписания");    
                    }
                    
                    return Failure("Дата акта  не должна быть  раньше даты приказа");
                    
                    
                }
            }

            return base.BeforeUpdateAction(service, entity);
	    }

        public override IDataResult BeforeCreateAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            entity.DocumentDate = DateTime.Today;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActCheck> service, ActCheck entity)
        {
            var violGroupDomain = Container.Resolve<IDomainService<DocumentViolGroup>>();

            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return result;
                }

                // удаляем связку документа с группой нарушений
                violGroupDomain.GetAll()
                               .Where(x => x.Document.Id == entity.Id)
                               .Select(x => x.Id)
                               .ForEach(x => violGroupDomain.Delete(x));


                return this.Success();
            }
            finally 
            {
                Container.Release(violGroupDomain);
            }
            
        }
    }
}
