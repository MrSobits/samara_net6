namespace Bars.Gkh.Entities.Suggestion
{
    using System;
    using System.Linq;
    using B4.Modules.States;
    using B4.Utils.Annotations;
    using B4.Application;
    using B4.DataAccess;
    using B4.IoC;
    using Entities;

    /// <summary>
    /// Обращения граждан
    /// </summary>
    public partial class CitizenSuggestion : BaseGkhEntity, IStatefulEntity
    {
        // for nh
        public CitizenSuggestion()
        {
            
        }

        public CitizenSuggestion(Rubric rubric)
        {
            ArgumentChecker.NotNull(rubric, "rubric");

            Rubric = rubric;
        }

        public virtual string Number { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual Rubric Rubric { get; protected set; }

        public virtual string ApplicantFio { get; set; }

        public virtual string ApplicantAddress { get; set; }

        public virtual string ApplicantPhone { get; set; }

        public virtual string ApplicantEmail { get; set; }

        public virtual string Description { get; set; }

        public virtual bool HasAnswer { get; set; }

        public virtual string AnswerText { get; set; }

        public virtual DateTime? AnswerDate { get; set; }

        public virtual string Address { get; set; }

        public virtual ProblemPlace ProblemPlace { get; set; }

        public virtual SugTypeProblem SugTypeProblem { get; set; }

        public virtual ManagingOrganization ExecutorManagingOrganization { get; set; }

        public virtual Municipality ExecutorMunicipality { get; set; }

        public virtual ZonalInspection ExecutorZonalInspection { get; set; }

        public virtual ContragentContact ExecutorCrFund { get; set; }
        
        public virtual State State { get; set; }

        public virtual DateTime? Deadline { get; set; }

        public virtual MessageSubject MessageSubject { get; set; }

        public virtual Room Flat { get; set; }

        public virtual bool TestSuggestion { get; set; }

        /// <summary>
        /// Контрагент как корреспондент в обращении
        /// </summary>
        public virtual Contragent ContragentCorrespondent { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID
        /// </summary>
        public virtual string GisGkhGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Transport GUID
        /// </summary>
        public virtual string GisGkhTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Answer GUID
        /// </summary>
        public virtual string GisGkhAnswerGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ Answer Transport GUID
        /// </summary>
        public virtual string GisGkhAnswerTransportGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID родительского обращения
        /// </summary>
        public virtual string GisGkhParentGuid { get; set; }

        /// <summary>
        /// ГИС ЖКХ GUID контрагента, от которого обращение
        /// </summary>
        public virtual string GisGkhContragentGuid { get; set; }

        /// <summary>
        /// Признак работы с обращением в ГИС ЖКХ
        /// </summary>
        public virtual bool GisWork { get; set; }

        #region Адрес

        public virtual string Body { get; set; }

        public virtual string Room { get; set; }

        public virtual int Apartment { get; set; }

        public virtual string MunicipalityCodeKladr { get; set; }

        public virtual string CityCodeKladr { get; set; }

        public virtual string StreetCodeKladr { get; set; }

        public virtual string House { get; set; }

        public virtual string AddressFullCode { get; set; }

        #endregion

        // Поля для автогенерации новых номеров

        public virtual int Year { get; set; }

        public virtual int Num { get; set; }

        public virtual void Open()
        {
            var execType = GetCurrentExecutorType();
            var transitions = Rubric.Transitions.ToArray();

            foreach (var transition in transitions)
            {
                if (transition.InitialExecutorType == execType)
                {
                    ApplyTransition(transition);
                }
            }
        }

        public virtual void Close()
        {
            var container = ApplicationContext.Current.Container;
            var stateProvider = container.Resolve<IStateProvider>();
            var stateRepo = container.Resolve<IRepository<State>>();
            using (container.Using(stateProvider, stateRepo))
            {
                var typeInfo = stateProvider.GetStatefulEntityInfo(typeof(CitizenSuggestion));

                var state = stateRepo.GetAll()
                    .Where(x => x.TypeId == typeInfo.TypeId)
                    .FirstOrDefault(x => x.Code.ToLower() == "end");

                State = state;
            }
        }

        public virtual void SetRubric(Rubric rubric)
        {
            Rubric = rubric;
        }
    }
}