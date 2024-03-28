namespace Bars.GisIntegration.UI.Service.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities.GisRole;

    using Castle.Windsor;

    /// <summary>
    /// Сервис GisRoleMethod
    /// </summary>
    public class GisRoleMethodService : IGisRoleMethodService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <summary>
        /// Добавить методы интеграции для роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult AddRoleMetods(BaseParams baseParams)
        {
            var gisRoleMethodDomain = this.Container.ResolveDomain<GisRoleMethod>();
            var gisRoleDomain = this.Container.ResolveDomain<GisRole>();

            try
            {
                var roleId = baseParams.Params.GetAsId("roleId");
                var methods = baseParams.Params.GetAs("methods", new MethodProxy[0]);

                var existingRoleMethodIds =
                    gisRoleMethodDomain.GetAll()
                        .Where(x => x.Role.Id == roleId)
                        .Select(x => x.MethodId)
                        .ToList();

                if (roleId == 0)
                {
                    return BaseDataResult.Error("Не указана роль ГИС");
                }

                var role = gisRoleDomain.Get(roleId);

                var listToSave = new List<GisRoleMethod>();

                foreach (var method in methods)
                {
                    if (!existingRoleMethodIds.Contains(method.Id))
                    {
                        listToSave.Add(new GisRoleMethod
                        {
                            MethodId = method.Id,
                            MethodName = method.Name,
                            Role = role
                        });
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave, 10000, true, true);

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(gisRoleMethodDomain);
                this.Container.Release(gisRoleDomain);
            }
        }

        /// <summary>
        /// Класс-прокси для получения данных о методах из параметров
        /// </summary>
        private class MethodProxy
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}
