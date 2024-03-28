namespace Bars.GkhGji.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    public class TatarstanProtocolGjiContragentInterceptor : DocumentGjiInterceptor<TatarstanProtocolGjiContragent>
    {
        /// <summary>
        /// Инспектор Id.
        /// todo убрать
        /// </summary>
        private long InspectionId { get; set; }

        /// <inheritdoc />
        public override IDataResult BeforeCreateAction(IDomainService<TatarstanProtocolGjiContragent> service, TatarstanProtocolGjiContragent entity)
        {
            var domainServiceInspection = this.Container.Resolve<IDomainService<BaseTatProtocolGji>>();
            var domainStage = this.Container.Resolve<IDomainService<InspectionGjiStage>>();
            var domainSanction = this.Container.Resolve<IDomainService<SanctionGji>>();
            var stateDomain = this.Container.ResolveDomain<State>();
            var identityDocTypeDomain = this.Container.ResolveDomain<IdentityDocumentType>();

            using (this.Container.Using(domainStage, domainServiceInspection, 
                domainSanction, stateDomain, identityDocTypeDomain))
            {
                // Также перед созданием мы создаем Проверку Поскольку Документы ГЖИ без основания немогут быть
                // И дерево Этапов также настроено на inspectionGJI поэтому скрыто от глаз пользователей создаем основание
                // Проверки с типом постановление прокуратуры
                var newInspection = new BaseTatProtocolGji
                {
                    TypeBase = TypeBase.ProtocolGji
                };

                domainServiceInspection.Save(newInspection);

                // также перед созданием мы создаем этап 
                var newStage = new InspectionGjiStage
                {
                    Inspection = newInspection,
                    TypeStage = TypeStage.ProtocolGji,
                    Position = 0
                };

                domainStage.Save(newStage);

                entity.Inspection = newInspection;
                entity.Stage = newStage;
                entity.Sanction = domainSanction.FirstOrDefault(x => x.Code == "0");
                entity.State = stateDomain.FirstOrDefault(x => x.TypeId == "gji_document_protocol_gji_rt" && x.StartState);
                //тип документа по умолчанию - паспорт РФ
                entity.IdentityDocumentType = identityDocTypeDomain.FirstOrDefault(x => x.Code == "01");
                if (entity.DocumentDate.HasValue)
                {
                    entity.DocumentYear = entity.DocumentDate.Value.Year;
                }

                return base.BeforeCreateAction(service, entity);
            }
        }
        
        /// <inheritdoc />
        public override IDataResult AfterCreateAction(IDomainService<TatarstanProtocolGjiContragent> service, TatarstanProtocolGjiContragent entity)
        {
            //заполнение номера документа
            entity.DocumentNumber = $"{entity.Id}-{entity.Municipality?.Code}-{entity.ZonalInspection?.ShortName}";
            service.Update(entity);

            return base.AfterCreateAction(service, entity);
        }

        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<TatarstanProtocolGjiContragent> service, TatarstanProtocolGjiContragent entity)
        {
            var result = base.BeforeDeleteAction(service, entity);

            if (!result.Success)
            {
                return this.Failure(result.Message);
            }

            var annexService = this.Container.Resolve<IDomainService<TatarstanProtocolGjiAnnex>>();
            var lawService = this.Container.Resolve<IDomainService<TatarstanProtocolGjiArticleLaw>>();
            var realityObjectService = this.Container.Resolve<IDomainService<TatarstanProtocolGjiRealityObject>>();
            var violationService = this.Container.Resolve<IDomainService<TatarstanProtocolGjiViolation>>();
            var eyewitnessService = this.Container.Resolve<IDomainService<TatarstanDocumentWitness>>();
            var inspectorsService = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var gisChargeToSendDomain = this.Container.Resolve<IDomainService<GisChargeToSend>>();
            
            using (this.Container.Using(annexService, lawService, realityObjectService, violationService, 
                eyewitnessService, inspectorsService, gisChargeToSendDomain))
            {
                var refFuncs = new List<Func<long, string>>
                {
                    id => annexService.GetAll().Any(x => x.DocumentGji.Id == id) ? "Приложения" : null,
                    id => lawService.GetAll().Any(x => x.TatarstanProtocolGji.Id == id) ? "Статьи закона" : null,
                    id => realityObjectService.GetAll().Any(x => x.TatarstanProtocolGji.Id == id) ? "Адреса правонарушений" : null,
                    id => violationService.GetAll().Any(x => x.TatarstanProtocolGji.Id == id) ? "Нарушения" : null,
                    id => eyewitnessService.GetAll().Any(x => x.DocumentGji.Id == id) ? "Сведения о свидетелях и потерпевших" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                if (refs.Any())
                {
                    return this.Failure($"Существуют связанные записи в следующих таблицах: {string.Join(", ", refs)}");
                }

                //удаление инспекторов
                var inspectors = inspectorsService.GetAll()
                    .Where(x => x.DocumentGji.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();
                inspectors.ForEach(x => inspectorsService.Delete(x));

                //todo
                // Короче поскольку над удалить документ то Основание проверки данного документа также адо будет удалить
                // Следовательно запоминаем Id оснвания чтобы в AfterDelete совершить данный акт удаления
                if (entity.Inspection != null)
                {
                    this.InspectionId = entity.Inspection.Id;
                }
                
                var gisChargeList = gisChargeToSendDomain.GetAll().Where(x => x.Document.Id == entity.Id);
                
                // Удаление записей реестра отправки начислений в ГИС ГМП
                foreach (var gisCharge in gisChargeList)
                {
                    gisChargeToSendDomain.Delete(gisCharge.Id);
                }
            }

            return result;
        }

        /// <inheritdoc />
        public override IDataResult AfterDeleteAction(IDomainService<TatarstanProtocolGjiContragent> service, TatarstanProtocolGjiContragent entity)
        {
            var baseService = this.Container.Resolve<IDomainService<BaseTatProtocolGji>>();
            var documentService = this.Container.Resolve<IDomainService<DocumentGji>>();

            using (this.Container.Using(baseService, documentService))
            {
                // Сначала вызываем удаление базового потому что там произходит зачистка Stage 
                var result = base.AfterDeleteAction(service, entity);

                if (this.InspectionId > 0)
                {
                    if (!documentService.GetAll().Any(x => x.Inspection.Id == this.InspectionId))
                    {
                        // Если нет ни одного документа у основания то удаляем основание
                        baseService.Delete(this.InspectionId);
                    }
                }

                return result;
            }
        }
    }
}
