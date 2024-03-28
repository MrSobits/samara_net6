namespace Bars.GkhGji.Regions.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    // Пустышка на случай если от этог окласса наследовались
    public class AppealCitsServiceInterceptor : Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var servAppCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();

            try
            {

                var executantIds = servAppCitsExecutant.GetAll()
                                        .Where(x => x.AppealCits.Id == entity.Id)
                                        .Select(x => x.Id)
                                        .AsEnumerable();

                foreach (var id in executantIds)
                {
                    servAppCitsExecutant.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);

            }
            finally
            {
                Container.Release(servAppCitsExecutant);
            }
        }
    }
}