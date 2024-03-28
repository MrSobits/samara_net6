namespace Bars.GkhGji.Regions.Nnovgorod.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class AppealCitsAnswerInterceptor : EmptyDomainInterceptor<AppealCitsAnswer>
    {
        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            if (string.IsNullOrEmpty(entity.DocumentNumber))
            {
                // Сформировать номер
                var docNumber = string.Empty;
                var gjiIndex = string.Empty;

                if (entity.AppealCits.ZonalInspection != null)
                {
                    gjiIndex += entity.AppealCits.ZonalInspection.IndexOfGji + "-";
                }
                else if (entity.AppealCits.Accepting == Accepting.Managment)
                {
                    gjiIndex += "515-03-";
                }

                var year = entity.DocumentDate.HasValue ? entity.DocumentDate.Value.Year : DateTime.Now.Year;

                // Все номера ответов
                var numbers = service.GetAll()
                    .Where(x => x.Year == year)
                    .Select(x => x.DocumentNumber.Replace(" ", string.Empty))
                    .ToArray();

                // Все правильно сформированные номера (типа "515-03-123/2014", где 123 сквозной порядковый номер)
                var regex = new Regex(@"-\w+/");
                var matched = new List<int>();

                foreach (var number in numbers)
                {
                    if (regex.IsMatch(number))
                    {
                        var mtch = regex.Match(number);
                        if (mtch.Groups.Count > 0)
                        {
                            var num = Regex.Replace(mtch.Captures[0].Value, @"[^\d]", string.Empty);
                            matched.Add(num.ToInt());
                        }
                    }
                }

                // Ищем след. макс. номер ответа
                var nextnum = 0;
                if (matched.Count > 0)
                {
                    nextnum = matched.Max();
                }

                // Формируем номер + проверяем на уникальность
                bool unique = false;
                while (!unique)
                {
                    nextnum++;
                    docNumber = gjiIndex + nextnum + "/" + year;
                    unique = numbers.All(x => x != docNumber);
                }

                entity.DocumentNumber = docNumber;
                entity.Year = year;
            }
            else
            {
                // Проверить введенный пользователем номер на уникальность
                return this.NumberIsUnique(service, entity);
            }

            return Success();
        }

        /// <summary>Метод вызывается перед обновлением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            // Проверить введенный пользователем номер на уникальность
            return this.NumberIsUnique(service, entity);
        }

        /// <summary> Проверка номера документа на уникальность</summary>
        /// <param name="service">Домен-сервис ответов по обращению</param>
        /// <param name="entity">Ответ по обращению</param>
        /// <returns>Успешный/Неудачный ответ запроса</returns>
        private IDataResult NumberIsUnique(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            if (!string.IsNullOrEmpty(entity.DocumentNumber))
            {
                if (service.GetAll().Any(x => x.DocumentNumber == entity.DocumentNumber && x.Id != entity.Id))
                {
                    return Failure("Введенный номер документа уже существует!");
                }
            }

            return Success();
        }
    }
}
