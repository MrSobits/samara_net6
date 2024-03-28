namespace Bars.GkhGji.Regions.Tatarstan.DomainService.PreventiveAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="VisitSheetViolation"/>
    /// </summary>
    public class VisitSheetViolationService : IVisitSheetViolationService
    {
        #region DependencyInjection
        private readonly IWindsorContainer container;

        public VisitSheetViolationService(IWindsorContainer container)
        {
            this.container = container;
        }
        #endregion

        /// <inheritdoc />
        public IDataResult AddViolations(BaseParams baseParams)
        {
            var violationInfoId = baseParams.Params.GetAsId("violationInfoId");
            var violations = baseParams.Params.GetAs<List<ViolationDto>>("violations") ?? new List<ViolationDto>();
            var deletedViolationIds = baseParams.Params.GetAs<string>("deletedViolationId").ToLongArray();

            var visitSheetViolationDomain = this.container.ResolveDomain<VisitSheetViolation>();

            using (var transaction = this.container.Resolve<IDataTransaction>())
            using (this.container.Using(visitSheetViolationDomain))
            {
                try
                {
                    if (violations.Any())
                    {
                        var violationInfo = new VisitSheetViolationInfo { Id = violationInfoId };

                        violations.ForEach(x =>
                        {
                            var violation = x.Id > 0
                                ? visitSheetViolationDomain.Get(x.Id)
                                : new VisitSheetViolation();

                            violation.Id = x.Id;
                            violation.ViolationInfo = violationInfo;
                            violation.Violation = new ViolationGji { Id = x.ViolationId };
                            violation.IsThreatToLegalProtectedValues = x.IsThreatToLegalProtectedValues;

                            if (x.Id > 0)
                                visitSheetViolationDomain.Update(violation);
                            else
                                visitSheetViolationDomain.Save(violation);
                        });
                    }

                    if (deletedViolationIds.Any())
                    {
                        visitSheetViolationDomain.GetAll()
                            .Where(x => deletedViolationIds.Contains(x.Id))
                            .Select(x => x.Id)
                            .ToList()
                            .ForEach(x => visitSheetViolationDomain.Delete(x));
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult(false, "Не удалось сохранить нарушения");
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Dto-шка с данными для создания/обновления <see cref="VisitSheetViolation"/>
        /// </summary>
        private class ViolationDto
        {
            /// <summary>
            /// Идентификатор записи
            /// </summary>
            public long Id { get; set; }

            /// <summary>
            /// Идентификатор нарушения ГЖИ
            /// </summary>
            public long ViolationId { get; set; }

            /// <summary>
            /// Признак опасности
            /// </summary>
            public bool IsThreatToLegalProtectedValues { get; set; }
        }
    }
}