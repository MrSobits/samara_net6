namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using B4.Modules.States;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    using Entities;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик события смены статуса документа ГЖИ
    /// </summary>
    public class DocumentGjiStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId.Contains("gji_document"))
            {
                var document = entity as DocumentGji;

                if (document == null)
                {
                    throw new DocumentGjiStateChangeException("Не удалось привести к типу DocumentGji");
                }

                if (!document.DocumentNumber.IsEmpty())
                {
                    return;
                }

                return;
                if (newState.Code == "2") // Присвоение номера, мне кажется здесь я неправильно проверяю, но по-другому не нашел
                {
                    var serviceInspector = Container.Resolve<IDomainService<DocumentGjiInspector>>();
                    var zonalInspectionService = Container.Resolve<IDomainService<ZonalInspectionInspector>>();
                    var docGjiChildrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();
                    var docGjiService = Container.Resolve<IDomainService<DocumentGji>>();
                    var docResolutionProsecutorService = Container.Resolve<IDomainService<ResolPros>>();
                    var zonInspMunicipalityService = Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();

                    var maxNumber = 0;

                    try
                    {
                        switch (document.TypeDocumentGji)
                        {
                            case TypeDocumentGji.ResolutionProsecutor:
                            case TypeDocumentGji.Disposal: // Если это приказ или постановление прокуратуры - то сквозная нумерация
                                var gjiNumber = string.Empty;

                                // В зависимости от типа документа - индекс отдела получаем по-разному
                                if (document.TypeDocumentGji == TypeDocumentGji.Disposal)
                                {
                                    var inspector = serviceInspector.GetAll().FirstOrDefault(x => x.DocumentGji.Id == document.Id);

                                    if (inspector != null)
                                    {
                                        var zonalinspection =
                                            zonalInspectionService.GetAll()
                                                .FirstOrDefault(x => x.Inspector.Id == inspector.Inspector.Id);

                                        if (zonalinspection != null)
                                        {
                                            gjiNumber = zonalinspection.ZonalInspection.IndexOfGji;
                                        }
                                    }
                                }
                                else
                                {
                                    var resolutionProsecutor =
                                        docResolutionProsecutorService.GetAll().First(x => x.Id == document.Id);

                                    var zonalInspection = zonInspMunicipalityService.GetAll()
                                        .FirstOrDefault(x => x.Municipality == resolutionProsecutor.Municipality)
                                        .With(x => x.ZonalInspection);
                                    if (zonalInspection != null)
                                    {
                                        gjiNumber = zonalInspection.IndexOfGji;
                                    }
                                }

                                if (gjiNumber.IsEmpty())
                                {
                                    throw new DocumentGjiStateChangeException("Не заполнен индекс ГЖИ у отдела жилищной инспекции");
                                }

                                var docNumbers =
                                    docGjiService.GetAll()
                                        .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                                        .Where(x => x.DocumentNumber.Length > 4)
                                        .AsEnumerable()
                                        .Where(x => x.DocumentNumber.Substring(x.DocumentNumber.Length - 4, 4).ToInt() == DateTime.Now.Year)
                                        .Where(x => x.DocumentNumber.StartsWith(gjiNumber)) // Номер расчитывается в пределах отдела ЗЖИ
                                        .Select(x => x.DocumentNumber.Split('-')[2])
                                        .ToArray();
                                
                                if (docNumbers.Any())
                                {
                                    var ordinalNumber =
                                        docNumbers.Select(x => x.Substring(0,
                                                    x.IndexOf("/", StringComparison.InvariantCulture)
                                                    - (document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor
                                                        ? 2 // поправка на ПР в постановлении прокуратуры(515-219-01ПР/01/2013)
                                                        : 0)).ToInt()).ToList();

                                    if (ordinalNumber.Any())
                                    {
                                        maxNumber =
                                            ordinalNumber.Max();
                                    }
                                }
                                
                                maxNumber++;
                                document.DocumentNumber = string.Format(
                                    "{0}-{1}{2}/{3}",
                                    gjiNumber,
                                    maxNumber < 100 ? maxNumber.ToString("00") : maxNumber.ToString(CultureInfo.InvariantCulture),
                                    document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor ? "ПР" : string.Empty,
                                    document.DocumentYear);
                                break;
                            case TypeDocumentGji.Prescription:
                            case TypeDocumentGji.ActRemoval:
                            case TypeDocumentGji.Protocol:
                                var sameTypeDocuments =
                                    docGjiService.GetAll()
                                        .Where(x => x.DocumentNum == document.DocumentNum
                                                && x.TypeDocumentGji == document.TypeDocumentGji)
                                        .Select(x => x.DocumentNumber)
                                        .ToArray();

                                var disposal =
                                        docGjiService.GetAll()
                                            .FirstOrDefault(x => x.DocumentNum == document.DocumentNum
                                                    && x.TypeDocumentGji == TypeDocumentGji.Disposal);
                                if (disposal == null)
                                {
                                    throw new DocumentGjiStateChangeException("Не удалось получить родительский документ");
                                }

                                if (sameTypeDocuments.Count() == 1)
                                {
                                    // Если документов больше нет - берем номер приказа
                                    document.DocumentNumber = disposal.DocumentNumber;
                                }
                                else
                                {
                                    document.DocumentNumber = GetNumber(sameTypeDocuments, disposal.DocumentNumber);
                                }
                                break;
                            case TypeDocumentGji.ActCheck:
                                var checkParentDoc =
                                    docGjiChildrenService.GetAll()
                                        .FirstOrDefault(x => x.Children.Id == document.Id)
                                        .With(x => x.Parent);
                                if (checkParentDoc == null)
                                {
                                    throw new DocumentGjiStateChangeException("Не удалось получить родительский документ");
                                }

                                var disposalChildDocuments =
                                    docGjiChildrenService.GetAll()
                                        .Where(x => x.Children.TypeDocumentGji == document.TypeDocumentGji
                                                && x.Parent == checkParentDoc)
                                        .Select(x => x.Children.DocumentNumber)
                                        .ToArray();
                                document.DocumentNumber = disposalChildDocuments.Count() == 1
                                    ? checkParentDoc.DocumentNumber
                                    : GetNumber(disposalChildDocuments, checkParentDoc.DocumentNumber);
                                break;
                            case TypeDocumentGji.Resolution:
                                var parentDoc =
                                    docGjiChildrenService.GetAll()
                                        .FirstOrDefault(x => x.Children.Id == document.Id)
                                        .With(x => x.Parent);
                                if (parentDoc.TypeDocumentGji == TypeDocumentGji.Protocol)
                                {
                                    document.DocumentNumber = parentDoc.DocumentNumber;
                                }
                                else
                                {
                                    var resolChildDocuments =
                                        docGjiChildrenService.GetAll()
                                            .Where(x => x.Parent == parentDoc)
                                            .Select(x => x.Children.DocumentNumber)
                                            .ToArray();
                                    document.DocumentNumber = resolChildDocuments.Count() == 1
                                        ? parentDoc.DocumentNumber
                                        : this.GetNumber(resolChildDocuments, parentDoc.DocumentNumber);
                                }
                                break;
                            case TypeDocumentGji.Presentation:
                                var parentResolDoc =
                                    docGjiChildrenService.GetAll()
                                        .FirstOrDefault(x => x.Children.Id == document.Id)
                                        .With(x => x.Parent);

                                var resChildDocuments =
                                    docGjiChildrenService.GetAll()
                                        .Where(x => x.Parent == parentResolDoc)
                                        .Select(x => x.Children.DocumentNumber)
                                        .ToArray();
                                document.DocumentNumber = !resChildDocuments.Any()
                                    ? parentResolDoc.DocumentNumber
                                    : this.GetNumber(resChildDocuments, parentResolDoc.DocumentNumber);
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        if (e is DocumentGjiStateChangeException)
                        {
                            throw;
                        }

                        throw new Exception("Не удалось установить номер документа");
                    }
                }
            }
        }

        private string GetNumber(IEnumerable<string> numbers, string parentDocumentNumber)
        {
            var result = string.Empty;

            // ищем максимальный номер
            var maxNumber = 0;
            foreach (var item in numbers)
            {
                if (item.IsEmpty())
                {
                    continue;   
                }

                var elements = item.Split('/');
                if (elements.Length == 2)
                {
                    // Т.е номер вида 515-00-00/2013
                    maxNumber++;
                    result = string.Format("{0}/{1}/{2}", elements[0], maxNumber, elements[1]);
                }
                else if (elements.Length > 2)
                {
                    // Т.е номер вида 515-00-00/2/2013
                    if (maxNumber < elements[1].ToInt())
                    {
                        maxNumber = elements[1].ToInt();
                        result = string.Format("{0}/{1}/{2}", elements[0], maxNumber + 1, elements[2]);
                    }
                }
            }

            if (result.IsEmpty())
            {
                // Что скорее всего означает у переданных номеров неверный формат номера - считаем что данный документ будет первым
                return parentDocumentNumber;
            }

            return result;
        }

        private class DocumentGjiStateChangeException : Exception
        {
            public DocumentGjiStateChangeException(string message)
                : base(message)
            {
                
            }
        }
    }
}