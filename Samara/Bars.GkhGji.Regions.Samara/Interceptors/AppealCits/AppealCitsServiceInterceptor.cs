namespace Bars.GkhGji.Regions.Samara.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Samara.Entities;

    // Пустышка на случай если от этог окласса наследовались
    public class AppealCitsServiceInterceptor : Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor
    {
        public override IDataResult BeforeDeleteAction(IDomainService<AppealCits> service, AppealCits entity)
        {
            var servAppCitsTester = Container.Resolve<IDomainService<AppealCitsTester>>();

            try
            {
                var testerIds = servAppCitsTester.GetAll()
                                            .Where(x => x.AppealCits.Id == entity.Id)
                                            .Select(x => x.Id)
                                            .AsEnumerable();
                foreach (var id in testerIds)
                {
                    servAppCitsTester.Delete(id);
                }

                return base.BeforeDeleteAction(service, entity);
            }
            finally 
            {
                Container.Release(servAppCitsTester);
            }
        }
    }
}