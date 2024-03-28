namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using B4;

    using Bars.B4.Utils;

    using Castle.Windsor;
    using GkhGji.Entities;
    using System.Linq;
    using GkhGji.Enums;

    /// <summary>
    /// Отоброжение списка вида проверки в приказе 
    /// </summary>
    public class KindCheckGjiService : IKindCheckGjiService
    {
        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис вида проверки 
        /// </summary>
        public IDomainService<KindCheckGji> KindCheckGjiDomain { get; set; }

        /// <summary>
        /// Список вида проверки в приказе 
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult SpecList(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var typeBase = baseParams.Params.GetAs<TypeBase>("typeBase");

            var data = KindCheckGjiDomain.GetAll()
                .WhereIf(typeBase == TypeBase.PlanJuridicalPerson, x => x.Code == TypeCheck.PlannedExit || x.Code == TypeCheck.PlannedDocumentation)
                .WhereIf(typeBase != TypeBase.PlanJuridicalPerson, x => x.Code == TypeCheck.NotPlannedExit || x.Code == TypeCheck.NotPlannedDocumentation)
                .OrderBy(x => x.Code)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).ToList(), data.Count());
        }
    }
}
