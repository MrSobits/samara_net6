namespace Bars.GkhGji.StateChange
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using ValidateResult = Bars.B4.Modules.States.ValidateResult;
    using Castle.Windsor;

    public abstract class BaseDocValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }
        public abstract string Name { get; }
        public abstract string TypeId { get; }
        public abstract string Description { get; }

        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is DocumentGji)
            {
                /*
                 * Тут необходимо выяснить все проверки осуществляемые для всех видов документа
                 * и проверить относительно каждого типа
                 * То есть данное правило будет проверять каждую карточку на правильность заполненности необходимой информации в том числе и субтаблиц
                 */

                if (statefulEntity is Disposal)
                {
                    var disposal = statefulEntity as Disposal;

                    // Если Документ является Распоряжением
                    // проверяем заполненность обязательных полей

                    // если распоряжение не "вне инспекторской деятельности", то проверяем все обязательные поля
                    if (disposal.TypeDisposal != TypeDisposalGji.NullInspection)
                    {
                        // Получаем количество типов обследований
                        var typeSurveysCount = Container.Resolve<IDomainService<DisposalTypeSurvey>>().GetAll()
                            .Count(x => x.Disposal.Id == disposal.Id);

                        // получаем количество инспекторов
                        var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                            .Count(x => x.DocumentGji.Id == disposal.Id);

                        if (disposal.DocumentDate == null || disposal.DateStart == null ||
                            disposal.IssuedDisposal == null || inspectorCount == 0)
                        {
                            result.Message = "Необходимо заполнить обязательные поля на форме";
                            result.Success = false;
                            return result;
                        }

                        //if (typeSurveysCount == 0)
                        //{
                        //    result.Message = "Необходимо указать типы обследования";
                        //    result.Success = false;
                        //    return result;
                        //}
                    }
                    else // иначе, проверяем только дату документа и дл, вынесшее распоряжение, т.к. других полей просто нет
                    {
                        if (disposal.DocumentDate == null || disposal.IssuedDisposal == null)
                        {
                            result.Message = "Необходимо заполнить обязательные поля на форме";
                            result.Success = false;
                            return result;
                        }
                    }
                }
                else if (statefulEntity is ActCheck)
                {
                    return ValidateActCheck(statefulEntity as ActCheck);
                }
                else if (statefulEntity is ActSurvey)
                {
                    // Если Документ является актом обследования
                    var actSurvey = statefulEntity as ActSurvey;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Count(x => x.DocumentGji.Id == actSurvey.Id);

                    if (actSurvey.DocumentDate == null || actSurvey.Area == null || actSurvey.Area == 0 || inspectorCount == 0)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is ActRemoval)
                {
                    // Если Документ является актом предписания (он же акт устранения нарушения)

                    // Если Документ является протоколом
                    var actRemoval = statefulEntity as ActRemoval;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Count(x => x.DocumentGji.Id == actRemoval.Id);

                    if (actRemoval.DocumentDate == null || inspectorCount == 0)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }

                    if (actRemoval.TypeRemoval == YesNoNotSet.NotSet)
                    {
                        result.Message = "Необходимо выставить признак устранены или не устранены нарушения";
                        result.Success = false;
                        return result;
                    }

                    if (actRemoval.TypeRemoval == YesNoNotSet.Yes &&
                        Container.Resolve<IDomainService<ActRemovalViolation>>().GetAll()
                        .Count(x => x.Document.Id == actRemoval.Id && x.DateFactRemoval == null) > 0)
                    {
                        result.Message = "Если нарушения устранены, то необходимо выставить даты фактического устранения";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is Prescription)
                {
                    // Если Документ является предписанием
                    var prescription = statefulEntity as Prescription;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Count(x => x.DocumentGji.Id == prescription.Id);

                    if (prescription.DocumentDate == null || prescription.Executant == null || inspectorCount == 0)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }
                    //у всех нарушений должна быть проставлена плановая дата устранения
                    if (Container.Resolve<IDomainService<PrescriptionViol>>().GetAll().Any(x => x.Document.Id == prescription.Id && x.DatePlanRemoval == null))
                    {
                        result.Message = "У всех нарушений должен быть проставлен срок устранения";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is Protocol)
                {
                    // Если Документ является протоколом
                    var protocol = statefulEntity as Protocol;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                        .Count(x => x.DocumentGji.Id == protocol.Id);

                    if (protocol.DocumentDate == null || protocol.Executant == null || inspectorCount == 0)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }

                    var articleLawExist = Container.Resolve<IDomainService<ProtocolArticleLaw>>().GetAll().Any(x => x.Protocol.Id == protocol.Id);

                    if (!articleLawExist)
                    {
                        result.Message = "Протокол должен содержать статьи закона";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is Resolution)
                {
                    // Если Документ является постановлением
                    var resolution = statefulEntity as Resolution;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    if (resolution.DocumentDate == null || resolution.Executant == null)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is ResolPros)
                {
                    // Если Документ является постановлением прокуратуры
                    var resolPros = statefulEntity as ResolPros;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    if (resolPros.DocumentDate == null || resolPros.Executant == null)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }

                    if (!Container.Resolve<IDomainService<ResolProsRealityObject>>().GetAll().Any(x => x.ResolPros.Id == resolPros.Id))
                    {
                        result.Message = "Обязательно наличие хотя бы одного дома во вкладке \"Адрес правонарушения\"";
                        result.Success = false;
                        return result;
                    }
                }
                else if (statefulEntity is ProtocolMvd)
                {
                    // Если Документ является протоколом МВД
                    var protocolMvd = statefulEntity as ProtocolMvd;

                    // проверяем заполненность обязательных полей
                    // получаем количество инспекторов
                    if (protocolMvd.DocumentDate == null)
                    {
                        result.Message = "Необходимо заполнить обязательные поля на форме";
                        result.Success = false;
                        return result;
                    }

                    if (!Container.Resolve<IDomainService<ProtocolMvdRealityObject>>().GetAll().Any(x => x.ProtocolMvd.Id == protocolMvd.Id))
                    {
                        result.Message = "Обязательно наличие хотя бы одного дома во вкладке \"Адрес правонарушения\"";
                        result.Success = false;
                        return result;
                    }
                }
            }

            result.Success = true;

            return result;
        }

        protected virtual ValidateResult ValidateActCheck(ActCheck act)
        {
            // проверяем заполненность обязательных полей
            // получаем количество инспекторов
            var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                .Count(x => x.DocumentGji.Id == act.Id);

            if (act.DocumentDate == null || act.Area == null || act.Area == 0 || inspectorCount == 0)
            {
                return ValidateResult.No("Необходимо заполнить обязательные поля на форме");
            }

            // Для Акта проверки также необходимо проверить чтобы у всех домов стоял признак выявлены или невыявлены проверки
            // То ест ьесли ест ьхотя бы один дом с признаком 'Незадано' То выдаем ошибку
            if (Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                .Count(x => x.HaveViolation == YesNoNotSet.NotSet && x.ActCheck.Id == act.Id) > 0)
            {
                return ValidateResult.No("Необходимо указать результаты проверки по всем домам");
            }

            return ValidateResult.Yes();
        }
    }
}