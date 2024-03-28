namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    using NHibernate.Util;

    public class InspectionGjiProvider : IInspectionGjiProvider
    {
        public IWindsorContainer Container { get; set; }

        public virtual string CodeRegion 
        {
            get { return "Tat"; }
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипуДокументаГЖИ
        /// </summary>
        public virtual IDataResult GetRules(BaseParams baseParams)
        {
            var documentGjiDomain = this.Container.Resolve<IDomainService<DocumentGji>>();
            var inspectionGjiDomain = this.Container.Resolve<IDomainService<InspectionGji>>();

            try
            {
                var typeBase = baseParams.Params.GetAs("typeBase", 0);
                var typeDocument = baseParams.Params.GetAs("typeDocument", 0);
                var parentId = baseParams.Params.GetAs("parentId", 0);

                if (parentId == 0)
                {
                    return new BaseDataResult(false, "Для получения списка правил необходимо передать Id сущности инициатора");
                }

                if (typeDocument > 0)
                {
                    var document = documentGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (document == null)
                    {
                        return new BaseDataResult(false, $"По Id {parentId} не найден документ ГЖИ");
                    }
                    else if (typeDocument != (int) document.TypeDocumentGji)
                    {
                        return new BaseDataResult(
                            false,
                            $"Запрашиваемый тип {typeDocument} не соответсвует типу документа {(int) document.TypeDocumentGji}");
                    }
                    else
                    {
                        var listRules = this.GetRules(document);
                        var dataResult = listRules.Select(x => new { x.Id, Name = x.ResultName, x.ActionUrl }).ToList();
                        return new BaseDataResult(dataResult);
                    }
                }
                else if (typeBase > 0)
                {
                    var inspection = inspectionGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (inspection == null)
                    {
                        return new BaseDataResult(false, $"По Id {parentId} не найдена проверка ГЖИ");
                    }
                    else if (typeBase != (int)inspection.TypeBase)
                    {
                        return new BaseDataResult(
                            false,
                            $"Запрашиваемый тип {typeBase} не соответсвует типу проверки {(int) inspection.TypeBase}");
                    }
                    else
                    {
                        var listRules = this.GetRules(inspection);
                        var dataResult = listRules.Select(x => new { x.Id, Name = x.ResultName, x.ActionUrl }).ToList();
                        return new BaseDataResult(dataResult);
                    }
                }
                else
                {
                    return new BaseDataResult(
                        false,
                        "Необходимо указать либо тип проверки, либо тип документа для получения списка правил формирвоания документов ГЖИ");
                }
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, exc.Message);
            }
            finally
            {
                this.Container.Release(documentGjiDomain);
                this.Container.Release(inspectionGjiDomain);
            }
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипаПроверки
        /// </summary>
        public virtual List<IInspectionGjiRule> GetRules(InspectionGji inspection)
        {
            var inspectionRulesService = this.Container.ResolveAll<IInspectionGjiRule>();

            try
            {
                var result = new List<IInspectionGjiRule>();

                var rules =
                    inspectionRulesService
                        .Where(x => x.CodeRegion == this.CodeRegion && x.TypeInspectionInitiator == inspection.TypeBase);

                foreach (var rule in rules)
                {
                    // Необходим опроверить Валидацию правила а только в случае 
                    // положительного результата доабвить его в список
                    var validation = rule.ValidationRule(inspection);

                    if (validation.Success)
                    {
                        result.Add(rule);
                    }
                }

                return result;

            }
            finally
            {
                this.Container.Release(inspectionRulesService);
            }
           
        }

        /// <summary>
        /// Получаем правила формирования документов для ТипаПроверки
        /// </summary>
        public virtual List<IDocumentGjiRule> GetRules(DocumentGji document)
        {
            var documentRulesService = this.Container.ResolveAll<IDocumentGjiRule>();

            try
            {
                var result = new List<IDocumentGjiRule>();

                var rules = documentRulesService
                        .Where(x => x.CodeRegion == this.CodeRegion
                                    && x.TypeDocumentInitiator == document.TypeDocumentGji)
                        .ToList();

                foreach (var rule in rules)
                {
                    // Необходим опроверить Валидацию правила а только в случае 
                    // положительного результата доабвить его в список
                    var validation = rule.ValidationRule(document);

                    if (validation.Success)
                    {
                        result.Add(rule);
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(documentRulesService);
            }
        }

        /// <summary>
        /// Метод формирвоания дкоумента
        /// Документ ГЖИ может быть сформирвоан либо по основанию проверки, 
        /// либо по другому документу ГЖИ
        /// </summary>
        public virtual IDataResult CreateDocument(BaseParams baseParams)
        {
            var parentId = baseParams.Params.GetAs("parentId", 0);
            var ruleId = baseParams.Params.GetAs("ruleId", "");
            
            if (parentId == 0)
            {
                return new BaseDataResult(false, "Необходимо указать родителький документ");
            }

            if (string.IsNullOrEmpty(ruleId))
            {
                return new BaseDataResult(false, "Не указан идентификатор правила формирования документа");
            }

            var inspectionRulesService = this.Container.ResolveAll<IInspectionGjiRule>();
            var documentRulesService = this.Container.ResolveAll<IDocumentGjiRule>();
            var documentGjiDomain = this.Container.Resolve<IDomainService<DocumentGji>>();
            var inspectionGjiDomain = this.Container.Resolve<IDomainService<InspectionGji>>();

            try
            {
                var inspectionRule = inspectionRulesService.FirstOrDefault(x => x.CodeRegion == this.CodeRegion && x.Id == ruleId);
                var documentRules = documentRulesService.Where(x => x.CodeRegion == this.CodeRegion && x.Id == ruleId).ToList();

                if (inspectionRule == null && !documentRules.Any())
                {
                    return new BaseDataResult(false, $"По идентификатору {ruleId} не удалось определить правило формирования документа ГЖИ");
                }

                if (documentRules.Any())
                {
                    // Если правило по Документу, то и получаем по parentId документ ГЖИ
                    var document = documentGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (document == null)
                    {
                        return new BaseDataResult(false, $"По Id {parentId} не найден документ ГЖИ");
                    }
                    else
                    {
                        var validatedDocumentRules = new List<IDocumentGjiRule>();

                        if (documentRules.Count > 1)
                        {
                            validatedDocumentRules = documentRules.Where(x => x.ValidationRule(document).Success).ToList();
                        
                            if (validatedDocumentRules.Count != 1)
                            {
                                return new BaseDataResult(false, $"По идентификатору {ruleId} не удалось " +
                                    $"{(validatedDocumentRules.Count > 1 ? "однозначно" : "")} " +
                                    "определить правило формирования документа ГЖИ");
                            }
                        }

                        var documentRule = documentRules.Count == 1
                            ? documentRules.First()
                            : validatedDocumentRules.First();

                        // Сначала формируем входящие параметры
                        documentRule.SetParams(baseParams);

                        // Создаем документ из другого документа ГЖИ
                        return documentRule.CreateDocument(document);
                    }
                }
                else if (inspectionRule != null)
                {
                    var inspection = inspectionGjiDomain.GetAll().FirstOrDefault(x => x.Id == parentId);

                    if (inspection == null)
                    {
                        return new BaseDataResult(false, $"По Id {parentId} не найден документ ГЖИ");
                    }
                    else
                    {
                        // Сначала формируем входящие параметры
                        inspectionRule.SetParams(baseParams);

                        // Создаем документ из другого документа ГЖИ
                        return inspectionRule.CreateDocument(inspection);
                    }
                }

                return new BaseDataResult(false, "Что-то пошло не так" );
            }
            catch (Exception exc)
            {
                return new BaseDataResult(false, exc.Message);
            }
            finally
            {
                this.Container.Release(inspectionRulesService);
                this.Container.Release(documentRulesService);
                this.Container.Release(documentGjiDomain);
                this.Container.Release(inspectionGjiDomain);
            }
        }

        /// <summary>
        /// Метод создания документа по Основанию проверки - данный метод нужен для тех случаем когда Нет правил но документ надо создать из проверки
        /// Например какието специфичные проверки когда ЖЕстко в коде сказано что должна стоят ькнопка - по которой должен создаватся документ
        /// То есть не по правилам 
        /// </summary>
        public virtual IDataResult CreateDocument(InspectionGji inspection, TypeDocumentGji typeDocument)
        {
            var inspectionRulesService = this.Container.ResolveAll<IInspectionGjiRule>();
            try
            {
                var inspectionRule = inspectionRulesService.FirstOrDefault(x => x.CodeRegion == this.CodeRegion
                                        && x.TypeInspectionInitiator == inspection.TypeBase
                                        && x.TypeDocumentResult == typeDocument);

                if (inspectionRule == null)
                {
                    return new BaseDataResult(false, "Не найдено правило");
                }

                return inspectionRule.CreateDocument(inspection);
            }
            finally 
            {
                this.Container.Release(inspectionRulesService);
            }
        }
    }
}
