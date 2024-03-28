namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;

    using NHibernate.Linq;

    public class WarningDocInterceptor : DocumentGjiInterceptor<WarningDoc>
    {
        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<WarningDoc> service, WarningDoc entity)
        {
            var baseWarningDict = new Dictionary<TypeBase, string>()
            {
                {TypeBase.PlanJuridicalPerson, "Плановая проверка юр. лиц"},
                {TypeBase.ProsecutorsClaim, "Проверка по требованию прокуратуры"},
                {TypeBase.DisposalHead, "Проверка по поручению руководителя"},
                {TypeBase.CitizenStatement, "Проверка по обращению граждан"},
                {TypeBase.InspectionActionIsolated, "Проверка по КНМ без взаимодействия"},
                {TypeBase.GjiWarning, "Обращение гражданина"},
                {TypeBase.ActionIsolated, "Мероприятие без взаимодействия"},
            };

            entity.BaseWarning = entity.Inspection is WarningInspection warningInspection
                ? warningInspection.InspectionBasis.GetDisplayName()
                : baseWarningDict.Get(entity.Inspection.TypeBase);
            entity.TypeDocumentGji = TypeDocumentGji.WarningDoc;
            entity.NcInRecived = YesNo.No;
            entity.NcOutSent = YesNo.No;
            entity.ObjectionReceived = YesNo.No;

            return base.BeforeCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeUpdateAction(IDomainService<WarningDoc> service, WarningDoc entity)
        {
            if (entity.CompilationPlace != null && entity.CompilationPlace.Id == 0)
            {
                Utils.SaveFiasAddress(this.Container, entity.CompilationPlace);
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<WarningDoc> service, WarningDoc entity)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var warningsBasisDomain = this.Container.ResolveDomain<WarningDocBasis>();
            var violationDomain = this.Container.ResolveDomain<WarningDocViolations>();
            var annexDomain = this.Container.ResolveDomain<WarningDocAnnex>();
            var warningDocRoDomain = this.Container.ResolveDomain<WarningDocRealObj>();
            var fiasAddressDomain = this.Container.ResolveDomain<FiasAddress>();

            using (this.Container.Using(fileManager, warningsBasisDomain, violationDomain, annexDomain, warningDocRoDomain))
            {
                this.Container.InTransaction(() =>
                {
                    warningsBasisDomain.GetAll()
                        .Where(x => x.WarningDoc.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList()
                        .ForEach(x =>
                        {
                            warningsBasisDomain.Delete(x);
                        });

                    violationDomain.GetAll()
                        .Where(x => x.WarningDoc.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList()
                        .ForEach(x =>
                        {
                            violationDomain.Delete(x);
                        });

                    annexDomain.GetAll()
                        .Where(x => x.WarningDoc.Id == entity.Id)
                        .Fetch(x => x.File)
                        .Select(x => new
                        {
                            x.Id,
                            x.File
                        })
                        .ToList()
                        .ForEach(x =>
                        {
                            annexDomain.Delete(x.Id);
                        });

                    warningDocRoDomain.GetAll()
                        .Where(x => x.WarningDoc.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToList()
                        .ForEach(x =>
                        {
                            warningDocRoDomain.Delete(x);
                        });

                    if (entity.CompilationPlace != null)
                    {
                        fiasAddressDomain.Delete(entity.CompilationPlace.Id);
                    }
                });
            }
            return base.BeforeDeleteAction(service, entity);
        }
    }
}