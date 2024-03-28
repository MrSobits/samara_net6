namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Entities;

    /// <summary>
    /// Интерцептор для сущностей Заявка на доступ к экзамену 
    /// </summary>
    public class PersonRequestToExamInterceptor : EmptyDomainInterceptor<PersonRequestToExam>
    {

        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<QualifyTestSettings> QualifyTestSettingsDomain { get; set; }

        public IDomainService<QExamQuestion> QExamQuestionDomain { get; set; }
        /// <summary>
        ///  Действие до создания    
        /// </summary>
        /// <param name="service">Домайн сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>результат</returns>
        public override IDataResult BeforeCreateAction(IDomainService<PersonRequestToExam> service, PersonRequestToExam entity)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(entity);
            
            var maxNum = service.GetAll()
                .Where(x => x.RequestDate.Year == entity.RequestDate.Year)
                .Select(x => x.RequestNum)
                .AsEnumerable()
                .SafeMax(x => x.ToInt()) + 1;

            entity.RequestNum = maxNum.ToStr();

            return this.Success();
        }
        /// <summary>
        ///  Действие до обновления    
        ///  RequestNum уникален только в рамках одного года.
        /// </summary>
        /// <param name="service">Домайн сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>результат</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<PersonRequestToExam> service, PersonRequestToExam entity)
        {

            if (service.GetAll()
                .Any(x => x.RequestNum == entity.RequestNum && x.Id != entity.Id && x.RequestDate.Year == entity.RequestDate.Year))
            {
                return this.Failure("Заявка с таким номером существует");
            }
            try{

                var qExamSetings = QualifyTestSettingsDomain.GetAll()
                    .Where(x => x.DateFrom <= DateTime.Now)
                    .Where(x => !x.DateTo.HasValue || x.DateTo.Value > DateTime.Now)
                    .OrderByDescending(x => x.Id)
                    .FirstOrDefault();

                //var answersList = QExamQuestionDomain.GetAll()
                //    .Where(x => x.PersonRequestToExam == entity);

                //var correctAnswersList = QExamQuestionDomain.GetAll()
                //    .Where(x => x.PersonRequestToExam == entity)
                //    .Where(x=> x.QualifyTestQuestionsAnswers != null && x.QualifyTestQuestionsAnswers.IsCorrect == Enums.YesNoNotSet.Yes).ToList();

                //var correctAnswersCount = correctAnswersList.Count;
                //var scoresGathed = qExamSetings.CorrectBall > 0 ? correctAnswersCount * qExamSetings.CorrectBall : 0;
                //var examResult = scoresGathed >= qExamSetings.AcceptebleRate ? true : false;
               

                //entity.CorrectAnswersPercent = scoresGathed;
                var examResult = entity.CorrectAnswersPercent >= qExamSetings.AcceptebleRate ? true : false;
                if (examResult)
                {
                    var correctState = StateDomain.GetAll()
                         .Where(x => x.TypeId == "gkh_person_request_exam")
                         .Where(x => x.Name == "Экзамен сдан").FirstOrDefault();
                    if (correctState != null)
                    {
                        entity.State = correctState;
                    }
                }
                else
                {
                    var uncorrectState = StateDomain.GetAll()
                         .Where(x => x.TypeId == "gkh_person_request_exam")
                         .Where(x => x.Name == "Экзамен не сдан").FirstOrDefault();
                    if (uncorrectState != null)
                    {
                        entity.State = uncorrectState;
                    }
                }               

               
            }
            catch (Exception e)
            {
               // return Failure(e.Message);
            }

            return this.Success();
        }

        /// <summary>
        ///  Действие до удаления   
        /// </summary>
        /// <param name="service">Домайн сервис</param>
        /// <param name="entity">Сущность</param>
        /// <returns>результат</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<PersonRequestToExam> service, PersonRequestToExam entity)
        {
            var certificateDomain = this.Container.ResolveDomain<PersonQualificationCertificate>();

            try
            {
                if (certificateDomain.GetAll().Any(x => x.RequestToExam.Id == entity.Id))
                {
                    return this.Failure("Существует созданный от этой заявки аттестат");
                }
            }
            finally
            {
                this.Container.Release(certificateDomain);
            }

            return this.Success();
        }
    }
}