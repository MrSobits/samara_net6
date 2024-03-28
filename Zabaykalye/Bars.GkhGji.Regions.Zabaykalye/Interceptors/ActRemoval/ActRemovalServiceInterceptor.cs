namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    using Bars.B4.Utils;

    public class ActRemovalServiceInterceptor : GkhGji.Interceptors.ActRemovalServiceInterceptor
    {
	    public override IDataResult BeforeUpdateAction(IDomainService<ActRemoval> service, ActRemoval entity)
	    {
			var documentGjiDomain = Container.Resolve<IDomainService<DocumentGjiChildren>>();
			using (Container.Using(documentGjiDomain))
			{
				var prescriptionMaxDate = documentGjiDomain.GetAll()
					   .Where(x => x.Children.Id == entity.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.Prescription)
                       .Where(x => x.Parent.DocumentDate.HasValue)
					   .Max(x => x.Parent.DocumentDate);

				if (entity.DocumentDate < prescriptionMaxDate)
				{
					return Failure("Дата акта не должна быть раньше даты предписания");
				}
			}

            return base.BeforeUpdateAction(service, entity);
	    }

        public override IDataResult BeforeCreateAction(IDomainService<ActRemoval> service, ActRemoval entity)
        {
            entity.DocumentDate = DateTime.Today;
            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ActRemoval> service, ActRemoval entity)
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
