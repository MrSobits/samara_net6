namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities;

    public class TatDisposalInterceptor : TatDisposalBaseInterceptor<TatarstanDisposal>
    {
    }

    public class TatDisposalBaseInterceptor<T> : DisposalServiceInterceptor<T>
        where T : TatarstanDisposal
    {
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var result = base.BeforeCreateAction(service, entity);

            if (!result.Success)
            {
                return result;
            }
            
            if (entity.Inspection.TypeBase == TypeBase.CitizenStatement)
            {
                var appealCitsRealityObjectDomain = this.Container.ResolveDomain<AppealCitsRealityObject>();
                var appealCitsStatSubjectDomain = this.Container.ResolveDomain<AppealCitsStatSubject>();
                var inspectionAppealCitsDomain = this.Container.ResolveDomain<InspectionAppealCits>();

                using (this.Container.Using(appealCitsRealityObjectDomain,
                    appealCitsStatSubjectDomain,
                    inspectionAppealCitsDomain))
                {
                    var appCits = inspectionAppealCitsDomain.GetAll()
                        .Where(x => x.Inspection.Id == entity.Inspection.Id)
                        .Select(x => new
                        {
                            HasRo = appealCitsRealityObjectDomain.GetAll().Any(y => y.AppealCits.Id == x.AppealCits.Id),
                            HasStatSubject = appealCitsStatSubjectDomain.GetAll().Where(y => y.Subject != null)
                                .Any(y => y.AppealCits.Id == x.AppealCits.Id),
                            x.AppealCits.DocumentNumber,
                            HasRequiredField = !string.IsNullOrEmpty(x.AppealCits.NumberGji) && x.AppealCits.ZonalInspection != null,
                        })
                        .ToList();

                    if (appCits.Any(x => !x.HasRo || !x.HasStatSubject || !x.HasRequiredField))
                    {
                        var failureMessage = new StringBuilder();
                        var tmpMsg = string.Empty;
                        failureMessage.AppendLine("Невозможно сформировать проверку, так как у обращений не заполнены поля:");

                        if (appCits.Any(x => !x.HasRequiredField))
                        {
                            failureMessage.Append("Обязательные поля: ");
                            failureMessage.AppendLine(appCits.Where(x => !x.HasRequiredField).Select(x => x.DocumentNumber)
                                .Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                        }

                        if (appCits.Any(x => !x.HasRo))
                        {
                            failureMessage.Append("Адрес дома: ");
                            failureMessage.AppendLine(appCits.Where(x => !x.HasRo).Select(x => x.DocumentNumber)
                                .Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                        }

                        if (appCits.Any(x => !x.HasStatSubject))
                        {
                            failureMessage.Append("Тематики: ");
                            failureMessage.AppendLine(appCits.Where(x => !x.HasStatSubject).Select(x => x.DocumentNumber)
                                .Aggregate(tmpMsg, (current, str) => current + string.Format(" {0}; ", str)));
                        }

                        return this.Failure(failureMessage.ToString());
                    }
                }
            }

            return this.Success();
        }

        /// <inheritdoc />
        protected override IDataResult ActionWithDependencies(T entity)
        {
            var tatarstanDisposalService = this.Container.Resolve<ITatarstanDisposalService>();

            using (this.Container.Using(tatarstanDisposalService))
            {
                //удаление связанных объектов
                var dependencies = tatarstanDisposalService.GetDependenciesItems(entity.Id);
                this.Container.InTransaction(() =>
                {
                    foreach (var dependency in dependencies)
                    {
                        var domainServiceType = typeof(IDomainService<>).MakeGenericType(dependency.Type);
                        var domain = (IDomainService)this.Container.Resolve(domainServiceType);
                        try
                        {
                            dependency.IdsList.ForEach(x => domain.Delete(x));
                        }
                        finally
                        {
                            this.Container.Release(domain);
                        }
                    }
                });
            }

            return new BaseDataResult();
        }
    }
}