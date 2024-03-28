namespace Bars.Gkh.DomainService
{
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Utils;

    using Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для доп роли контрагента
    /// </summary>
    public class ContragentAdditionRoleService : IContragentAdditionRoleService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddAdditionRole(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var additionRoleDomain = this.Container.ResolveDomain<ContragentAdditionRole>();
                var contragentRoleDomain = this.Container.ResolveDomain<ContragentRole>();
                var contragentDomain = this.Container.ResolveDomain<Contragent>();

                try
                {
                    var contragentId = baseParams.Params.GetAsId("contragentId");
                    var roleIds = baseParams.Params.GetAs<long[]>("roleIds") ?? new long[0];

                    var listObjects =
                        additionRoleDomain.GetAll()
                            .WhereContainsBulked(x => x.Role.Id, roleIds)
                            .Where(x => x.Contragent.Id == contragentId)
                            .Select(x => x.Role.Id)
                            .ToHashSet();

                    var contragent = contragentDomain.Load(contragentId);

                    foreach (var id in roleIds)
                    {
                        if (listObjects.Contains(id))
                            continue;

                        var newRecord = new ContragentAdditionRole
                        {
                            Contragent = contragent,
                            Role = contragentRoleDomain.Load(id)
                        };

                        additionRoleDomain.Save(newRecord);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch
                {
                    transaction.Rollback();
                    return BaseDataResult.Error("Произошла ошибка при сохранении");
                }
                finally
                {
                    this.Container.Release(additionRoleDomain);
                    this.Container.Release(contragentRoleDomain);
                    this.Container.Release(contragentDomain);
                }
            }
        }
    }
}