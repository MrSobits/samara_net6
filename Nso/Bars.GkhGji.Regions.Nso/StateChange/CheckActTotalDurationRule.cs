namespace Bars.GkhGji.Regions.Nso.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using B4.Utils;
    using Castle.Windsor;
    using ConfigSections;
    using Entities;
    using Gkh.Enums;
    using Gkh.Utils;
    using GkhGji.Entities;
    using GkhGji.Enums;

    /// <summary>
    /// Правило Акт проверки ГЖИ Общая продолжительность проверок
    /// </summary>
    public class CheckActTotalDurationRule : IRuleChangeStatus
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id
        {
            get { return "CheckActTotalDurationRule"; }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string Name
        {
            get { return "Контроль соответствия продолжительности проверки типу предпринимательства"; }
        }

        /// <summary>
        /// Тип
        /// </summary>
        public string TypeId
        {
            get { return "gji_document_actcheck"; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description
        {
            get
            {
                return
                    "При переводе статуса будет проверяться, что общая продолжительность проверки по таблице " +
                    "\"Дата и время проведения проверки\", не превышает максимальную продолжительность проверки для типа предпринимательства, " +
                    "к которому относится проверяемая организация";
            }
        }

        /// <summary>
        /// Домен сервис Дата и время проведения проверки
        /// </summary>
        public IDomainService<ActCheckPeriod> ActCheckPeriod { get; set; }

        /// <summary>
        /// Контролер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /* 1) В Основании проверки на основании которой создан Приказ, на основании которого создан Акт у которого переводим статус, смотрим значение поля Объект проверки.
            Если Объект проверки = Физическое лицо, то переводим статус - конец алгоритм.
            Если Объект проверки Организация или Должностное лицо, то:
            2) В карточке контрагента, указанного в поле "Юридическое лицо", смотрим значение поля Тип предпринимательства.
            Если Тип предпринимательства = Не задано, то переводим статус true.
            Иначе проверяем продолжительность проверки.
            Общая продолжительность проверки по таблице "Дата и время проведения проверки" по всем указанным дням не должна превышать продолжительность 
            заданную в Администрирование / Настройки приложения / Единые настройки приложения / Жилищная инспекция / Настройки продолжительности проверки
            3) Если продолжительность проверки превышена, то статус переводить И выводить ошибку:"Указанная продолжительность проверки превышает максимальную 
            продолжительность проверки для типа предпринимательства - <Тип>"
            Где <Тип> - тип предпринимательства из карточки контрагента.*/

        /// <summary>
        /// Проверка
        /// </summary>
        /// <param name="statefulEntity">Сущность проверки</param>
        /// <param name="oldState">Текущий статус</param>
        /// <param name="newState">Новый статус</param>
        /// <returns></returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = ValidateResult.Yes();
            if (statefulEntity is NsoActCheck)
            {
                var actCheck = statefulEntity as NsoActCheck;
                switch (actCheck.Inspection.PersonInspection)
                {
                    case PersonInspection.PhysPerson:
                        break;
                    case PersonInspection.Organization:
                    case PersonInspection.Official:
                        switch (actCheck.Inspection.Contragent.TypeEntrepreneurship)
                        {
                            case TypeEntrepreneurship.NotSet:
                                break;
                            case TypeEntrepreneurship.Average:
                            case TypeEntrepreneurship.Micro:
                            case TypeEntrepreneurship.Small:
                                result.Success = this.CheckTotalDuration(actCheck.Inspection.Contragent.TypeEntrepreneurship, actCheck.Id);
                                break;
                        }
                        break;
                }

                if (!result.Success)
                {
                    result.Message = string.Format(
                        "Указанная продолжительность проверки превышает максимальную продолжительность проверки для типа предпринимательства - {0}",
                        actCheck.Inspection.Contragent.TypeEntrepreneurship.GetEnumMeta().Display);
                }
            }

            return result;
        }

        // Метод проверки Общая продолжительность проверки по таблице "Дата и время проведения проверки" по всем указанным дням не должна превышать продолжительность 
        //заданную в Администрирование / Настройки приложения / Единые настройки приложения / Жилищная инспекция / Настройки продолжительности проверки
        private bool CheckTotalDuration(TypeEntrepreneurship type, long id)
        {
            var conf = Container.GetGkhConfig<HousingInspection>();

            var timeIntervals = GetTimeIntervals(id,conf);
            var actCheckDuration = GetActCheckDuration(timeIntervals,conf);
            var configCheckDuration = GetConfigCheckDuration(type, conf);

            if (configCheckDuration < actCheckDuration)
            {
                return false;
            }
            return true;
        }
        
        private List<TimeInterval> GetTimeIntervals(long id, HousingInspection conf)
        {
            var actCheckPeriods = this.ActCheckPeriod.GetAll()
                .Where(x => x.ActCheck.Id == id)
                .Select(x => new TimeInterval
                {
                    DateCheck = x.DateCheck,
                    DateStart = x.DateStart,
                    DateEnd = x.DateEnd
                })
                .OrderBy(x => x.DateCheck)
                .ToArray();

            if (actCheckPeriods.IsEmpty())
            {
                return new List<TimeInterval>();
            }

            var dateCheck = actCheckPeriods[0].DateCheck;

            // Занесение данных в список интервалов для того чтобы было с чем сравневать
            var listTimeInterval = new List<TimeInterval>()
            {
                new TimeInterval
                {
                    DateCheck = actCheckPeriods[0].DateCheck,
                    DateStart = actCheckPeriods[0].DateStart,
                    DateEnd = actCheckPeriods[0].DateEnd
                }
            };
           
            foreach (var actCheck in actCheckPeriods)
            {
                if (dateCheck == actCheck.DateCheck)
                {
                    if (actCheck.DateStart < actCheck.DateEnd)
                    {
                        var retryCount = 0;
                        
                        /*Список для добавления нового промежутка*/ 
                        var list = new List<TimeInterval>();
                        foreach (var timeInterval in listTimeInterval)
                        {
                            var beginLunchtime = actCheck.DateCheck + conf.SettingsOfTheDay.BeginLunchtime;
                            var endLunchtime = actCheck.DateCheck + conf.SettingsOfTheDay.EndLunchtime;
                            
                            /*Проверка текущего промежутка из списка промежутков */
                            if (new DateRange(actCheck.DateStart,
                                actCheck.DateEnd).Intersect(
                                    new DateRange(timeInterval.DateStart, timeInterval.DateEnd)) && timeInterval.DateCheck == actCheck.DateCheck)
                            {

                                /*Проверка начала текущего промежутка с началом промежутка из списка, если текущей 
                                промежуток шире то расширяем промежуток из списка начальным значением текущем промежутком   */
                                if (actCheck.DateStart < timeInterval.DateStart)
                                {
                                    timeInterval.DateStart = actCheck.DateStart;
                                }
                                /*Такая ситуация только с концами промежутков */
                                if (actCheck.DateEnd > timeInterval.DateEnd)
                                {
                                    timeInterval.DateEnd = actCheck.DateEnd;
                                }
                                /* Проверка если измененный промежуток в ходит промежуток обеда, это если ктото изменить настройках время обеда */
                                if (new DateRange(actCheck.DateStart,
                                    actCheck.DateEnd).Intersect(
                                        new DateRange(timeInterval.DateStart, timeInterval.DateEnd)))
                                {
                                    if (timeInterval.DateEnd > beginLunchtime &&
                                        timeInterval.DateStart < beginLunchtime &&
                                        timeInterval.DateEnd < endLunchtime)
                                    {
                                        timeInterval.DateEnd = beginLunchtime;
                                    }
                                    else if (timeInterval.DateStart > beginLunchtime &&
                                             timeInterval.DateEnd > endLunchtime &&
                                             timeInterval.DateStart < endLunchtime)
                                    {
                                        timeInterval.DateStart = endLunchtime;
                                    }
                                }

                                break;
                            }
                            else
                            {
                                /*Счетчик попыток нахождения пересечения*/
                                retryCount++;
                                /* Если все попытки пройдены то добавляем промежуток в список промежутков*/
                                if (retryCount == listTimeInterval.Count)
                                {
                                    var newTimeinterval = new TimeInterval();
                                    /*Правело если промежутоу в пересекается с обеденным */
                                    if (actCheck.DateEnd > beginLunchtime &&
                                        actCheck.DateStart < beginLunchtime &&
                                        actCheck.DateEnd < endLunchtime)
                                    {
                                        newTimeinterval.DateStart = actCheck.DateStart;
                                        newTimeinterval.DateEnd = endLunchtime;
                                        newTimeinterval.DateCheck = actCheck.DateCheck;
                                        list.Add(newTimeinterval);

                                    }
                                    else if (actCheck.DateStart > beginLunchtime &&
                                             actCheck.DateEnd > endLunchtime &&
                                             actCheck.DateStart < endLunchtime)
                                    {
                                        newTimeinterval.DateStart = beginLunchtime;
                                        newTimeinterval.DateEnd = actCheck.DateEnd;
                                        newTimeinterval.DateCheck = actCheck.DateCheck;
                                        list.Add(newTimeinterval);

                                    }

                                    /* Добавление промежутка */
                                    else if ((actCheck.DateStart < beginLunchtime && actCheck.DateEnd < endLunchtime) ||
                                        (actCheck.DateStart > beginLunchtime && actCheck.DateEnd > endLunchtime))
                                    {
                                        newTimeinterval.DateStart = actCheck.DateStart;
                                        newTimeinterval.DateEnd = actCheck.DateEnd;
                                        newTimeinterval.DateCheck = actCheck.DateCheck;
                                        list.Add(newTimeinterval);
                                    }

                                }
                            }
                        }

                        /*Добавление промежутков список промежутков*/
                        listTimeInterval.AddRange(list);
                    }
                }
                else
                {
                    dateCheck = actCheck.DateCheck;
                    listTimeInterval.Add(actCheck);
                }
            }
            return listTimeInterval;
        }

        private TimeSpan? GetActCheckDuration(List<TimeInterval> listTimeInterval, HousingInspection conf)
        {
            TimeSpan? result = new TimeSpan();
            foreach (var timeInterval in listTimeInterval)
            {
                result += timeInterval.DateEnd - timeInterval.DateStart;
                var endLunchtime = timeInterval.DateCheck + conf.SettingsOfTheDay.EndLunchtime;
                var beginLunchtime = timeInterval.DateCheck + conf.SettingsOfTheDay.BeginLunchtime;

                if (new DateRange(timeInterval.DateStart, timeInterval.DateEnd)
                    .Intersect(new DateRange(endLunchtime, beginLunchtime)))
                {
                    result -= endLunchtime - beginLunchtime;
                }
            }
            return result;
        }

        private TimeSpan? GetConfigCheckDuration(TypeEntrepreneurship type, HousingInspection conf )
        {
            var checkDuration  = 0;
            switch (type)
            {
                case TypeEntrepreneurship.Micro:
                    checkDuration = conf.SettingTheVerification.SubjectsMicrobusinessesConfig.Duration;
                    break;
                case TypeEntrepreneurship.Average:
                    checkDuration = conf.SettingTheVerification.SubjectMediumBusinessConfig.Duration;
                    break;
                case TypeEntrepreneurship.Small:
                    checkDuration = conf.SettingTheVerification.SubjectBusinessConfig.Duration;
                    break;
            }

            if (conf.SettingTheVerification.SubjectMediumBusinessConfig.Units == Units.Day)
            {
                var endOfTheDay = conf.SettingsOfTheDay.EndOfTheDay;
                var beginningOfTheDay = conf.SettingsOfTheDay.BeginningOfTheDay;
                var endLunchtime = conf.SettingsOfTheDay.EndLunchtime;
                var beginLunchtime = conf.SettingsOfTheDay.BeginLunchtime;
                var workhours = endOfTheDay - beginningOfTheDay - (endLunchtime - beginLunchtime);
                checkDuration *= workhours.Hours;
            }

            TimeSpan? result = new TimeSpan(checkDuration, 0, 0);

            return result;
        }

        private class TimeInterval
        {
            public DateTime? DateCheck { get; set; }
            public DateTime? DateStart { get; set; }
            public DateTime? DateEnd { get; set; }
        }
    }
}