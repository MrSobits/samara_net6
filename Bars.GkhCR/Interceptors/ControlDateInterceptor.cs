namespace Bars.GkhCr.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhCr.Entities;

    public class ControlDateInterceptor : EmptyDomainInterceptor<ControlDate>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ControlDate> service, ControlDate entity)
        {
            if (this.Container.Resolve<IDomainService<ControlDate>>().GetAll()
                         .Any(x => x.ProgramCr.Id == entity.ProgramCr.Id && x.Work.Id == entity.Work.Id))
            {
                return this.Failure("Данный вид работ уже есть в программе.");
            }

            return this.Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<ControlDate> service, ControlDate entity)
        {
            var stageWorkDomain = this.Container.Resolve<IDomainService<ControlDateStageWork>>();
            var limitDateDomain = this.Container.Resolve<IDomainService<ControlDateMunicipalityLimitDate>>();

            using (this.Container.Using(stageWorkDomain, limitDateDomain))
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => stageWorkDomain.GetAll().Any(x => x.ControlDate.Id == entity.Id) ? "Этапы работ контрольного срока" : null,
                    id => limitDateDomain.GetAll().Any(x => x.ControlDate.Id == entity.Id) ? "Сроки по муниципальным образованиям" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Any())
                {
                    message = refs.Aggregate(message, (current, str) => current + $" {str}; ");
                    message = $"Существуют связанные записи в следующих таблицах: {message}";
                    return this.Failure(message);
                }
            }

            return this.Success();
        }
    }
}