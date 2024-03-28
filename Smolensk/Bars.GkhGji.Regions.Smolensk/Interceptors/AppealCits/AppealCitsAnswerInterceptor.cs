namespace Bars.GkhGji.Regions.Smolensk.Interceptors
{
    using System;
    using System.Linq;
    using B4;

    using Bars.GkhGji.Enums;

    using GkhGji.Entities;
    using Enums;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Bars.B4.Utils;
    using Castle.Core.Internal;

    public class AppealCitsAnswerInterceptor : EmptyDomainInterceptor<AppealCitsAnswer>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {

            if (string.IsNullOrEmpty(entity.DocumentNumber))
            {//сформировать номер
                string docNumber = "";
                string gjiIndex = "";

                if (entity.AppealCits.ZonalInspection != null)
                {
                    gjiIndex += entity.AppealCits.ZonalInspection.IndexOfGji + "-";
                }
                else if (entity.AppealCits.Accepting == Accepting.Managment)
                {
                    gjiIndex += "515-03-";
                }

                var currYear = DateTime.Now.Year;

                //все номера ответов
                var numbers = service.GetAll()
                    .Where(x => x.DocumentDate.Value.Year == currYear)
                    .Select(x => x.DocumentNumber.Replace(" ",""))
                    .ToArray();

                //все правильно сформированные номера (типа "515-03-123/2014", где 123 сквозной порядковый номер)
                var regex = new Regex(@"-\w+/");
                var matched = new List<int>();
                foreach (var number in numbers){
                    if (regex.IsMatch(number)){
                        var mtch = regex.Match(number);
                        if (mtch.Groups.Count > 0){
                            var num = Regex.Replace(mtch.Captures[0].Value, @"[^\d]","");
                            matched.Add(num.ToInt());
                        }
                    }
                }

                //ищем след. макс. номер ответа
                int nextnum = numbers.Count(); 
                if (matched.Count > 0){
                    nextnum = matched.Max();
                }

                //формируем номер + проверяем на уникальность
                bool unique = false;
                while (!unique){
                    nextnum++;
                    docNumber = gjiIndex + nextnum + "/" + currYear;
                    unique = numbers.All(x => x != docNumber);
                }

                entity.DocumentNumber = docNumber;
            }
            else
            {//проверить введенный пользователем номер на уникальность
                return NumberIsUnique(service, entity);
            }

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {
            //проверить введенный пользователем номер на уникальность
            return NumberIsUnique(service, entity);
        }

        private IDataResult NumberIsUnique(IDomainService<AppealCitsAnswer> service, AppealCitsAnswer entity)
        {//проверка номера документа на уникальность
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
