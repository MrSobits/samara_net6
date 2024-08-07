﻿namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.B4.IoC;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class DisposalServiceInterceptor : EmptyDomainInterceptor<Disposal>
    {
        public IDomainService<DisposalSurveySubject> DisposalSurveySubjectDomain { get; set; }
        public IDomainService<DisposalTypeSurvey> DisposalTypeSurveyDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<Disposal> service, Disposal entity)
        {
            var result = DisposalTypeSurveyDomain.GetAll().Count(x => x.Disposal.Id == entity.Id) != 1
                ? Failure("Не добавлен Тип обследования.")
                : Success();

			if (!result.Success)
			{
				return result;
			}

	        if (entity.TypeDisposal == TypeDisposalGji.DocumentGji)
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
						return Failure("Дата приказа не должна быть раньше даты предписания");
					}
		        }
	        }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult BeforeCreateAction(IDomainService<Disposal> service, Disposal entity)
        {
            entity.DocumentDate = DateTime.Today;
            entity.DateStart=DateTime.Today.AddDays(1);
            entity.DateEnd = DateTime.Today.AddDays(19);

            return base.BeforeCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Disposal> service, Disposal entity)
        {
            DisposalSurveySubjectDomain.GetAll()
                .Where(x => x.Disposal.Id == entity.Id)
                .Select(x => x.Id)
                .ForEach(x => DisposalSurveySubjectDomain.Delete(x));

            return Success();
        }
    }
}