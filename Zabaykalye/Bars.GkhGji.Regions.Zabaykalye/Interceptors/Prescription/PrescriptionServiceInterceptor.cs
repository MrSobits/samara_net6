namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;

    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class PrescriptionInterceptor : GkhGji.Interceptors.PrescriptionInterceptor
    {
	    public override IDataResult BeforeUpdateAction(IDomainService<Prescription> service, Prescription entity)
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
					return Failure("Дата предписания не должна быть раньше даты акта проверки");
				}

			    var actRemovalMaxDate = documentGjiDomain.GetAll()
				    .Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                    .Where(x => x.Parent.DocumentDate.HasValue)
				    .Max(x => x.Parent.DocumentDate);

				if (entity.DocumentDate < actRemovalMaxDate)
				{
					return Failure("Дата предписания не должна быть раньше даты акта проверки предписания");
				}
		    }

            return base.BeforeUpdateAction(service, entity);
	    }

        public override IDataResult BeforeCreateAction(IDomainService<Prescription> service, Prescription entity)
        {
            entity.DocumentDate = DateTime.Today;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Prescription> service, Prescription entity)
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
