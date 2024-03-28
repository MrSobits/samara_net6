namespace Bars.GkhGji.Regions.Nnovgorod.NumberRule
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Entities;
    using Castle.Windsor;

    public class AppealCitsNumberRuleNNovgorod : IAppealCitsNumberRule
    {
        public IWindsorContainer Container { get; set; }

        public void SetNumber(IEntity entity)
        {
            /* В Нижнем Новгороде номер документа автоматически ставится при переводе статуса с "Новое" на "В работе"
             * и при включенной настройке "Проверка формирования номера обращения НН".
             * При этом, если доступна возможность ручного изменения номера документа, возникает проблемы с нумерацией, если
             * не синхронизированы поля DocumentNumber и IntNumber, IntSubnumber.
             * Поэтому здесь выполняем синхронизацию этих полей:
             *    если "Номер документа" был очищен - присваиваем IntNumber и IntSubnumber = 0
             *    если "Номер документа" был изменен - пытаемся распарсить по слешу (номер/подномер) целочисленные значения для IntNumber и IntSubnumber,
             *    где номер и подномер могут содержать в себе символы не являющиеся цифрами как "1771д/1061ж"
             */

            var appeal = entity as AppealCits;

            if (appeal == null || appeal.Id == 0)
            {
                return;
            }

            if (string.IsNullOrEmpty(appeal.DocumentNumber))
            {
                appeal.IntNumber = 0;
                appeal.IntSubnumber = 0;
            }
            else
            {
                appeal.IntNumber = 0;
                appeal.IntSubnumber = 0;

                var splitNumbers = appeal.DocumentNumber.Split('/');

                if (splitNumbers.Length > 2)
                {
                    // Номер должен быть вида (номер/подномер)
                    return;
                }

                var number = splitNumbers[0];
                appeal.IntNumber = this.ParseInt(number);

                if (splitNumbers.Length > 1)
                {
                    var subnumber = splitNumbers[1];
                    appeal.IntSubnumber = this.ParseInt(subnumber);
                }
            }
        }

        /// <summary>
        /// Пытается вытащить из строки целое число. В конце строки могут быть не цифровые символы.
        /// Если получить число не удается, возвращает 0.
        /// </summary>
        /// <param name="str">Строка, в начале которой содержится число</param>
        /// <returns>Целое число</returns>
        private int ParseInt(string str)
        {
            int result;

            try
            {
                var number = System.Text.RegularExpressions.Regex.Match(str, @"^\d+").Value;

                int.TryParse(number, out result);
            }
            catch (Exception e)
            {
                result = 0;
            }

            return result;
        }
    }
}