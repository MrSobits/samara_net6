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
    /// Сервис RisContragentRole
    /// </summary>
    public class RisContragentRoleService : IRisContragentRoleService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Добавить контрагенту роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public IDataResult AddContragentRoles(BaseParams baseParams)
        {
            var gisOperatorDomain = this.Container.ResolveDomain<GisOperator>();
            var gisRoleDomain = this.Container.ResolveDomain<GisRole>();
            var risContragentRoleDomain = this.Container.ResolveDomain<RisContragentRole>();

            try
            {
                var operatorId = baseParams.Params.GetAsId("operatorId");
                var roleIds = baseParams.Params.GetAs("roleIds", new long[0]);

                var existingContragentRoles =
                    risContragentRoleDomain.GetAll()
                        .Where(x => x.GisOperator.Id == operatorId)
                        .Select(x => x.Role.Id)
                        .ToList();

                if (operatorId == 0)
                {
                    return BaseDataResult.Error("Не указан контрагент");
                }

                var gisOperator = gisOperatorDomain.Get(operatorId);

                var listToSave = new List<RisContragentRole>();

                foreach (var roleId in roleIds)
                {
                    if (!existingContragentRoles.Contains(roleId))
                    {
                        listToSave.Add(new RisContragentRole
                        {
                            GisOperator = gisOperator,
                            Role = gisRoleDomain.Get(roleId)
                        });
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, listToSave, 10000, true, true);

                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(gisOperatorDomain);
                this.Container.Release(gisRoleDomain);
                this.Container.Release(risContragentRoleDomain);
            }
        }
    }
}
