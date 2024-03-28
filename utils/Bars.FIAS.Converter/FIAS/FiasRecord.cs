using System;

namespace Bars.FIAS.Converter
{
    public class FiasRecord
    {
        #region Данные поля незагружаются из ФИАС

        /// <summary>
        /// Идентификатор записи в Б4 (Он нужен для Update)
        /// </summary>
        public int Id;

        /// <summary>
        /// Итоговый код записи
        /// </summary>
        public string CodeRecord;

        /// <summary>
        /// Тип записи. всегда 0 (1-Это добавленная пользователем)
        /// </summary>
        public int TypeRecord = 10;
        #endregion

        /// <summary>
        /// Глобальный уникальный идентификатор адресного объекта 
        /// </summary>
        public string AOGuid;

        /// <summary>
        /// Идентификатор объекта родительского объекта
        /// </summary>
        public string ParentGuid;

        /// <summary>
        /// Уникальный идентификатор записи. Ключевое поле.
        /// </summary>
        public string AOId;

        /// <summary>
        /// Идентификатор записи связывания с предыдушей исторической записью
        /// </summary>
        public string PrevId;

        /// <summary>
        /// Идентификатор записи связывания с последующей исторической записью
        /// </summary>
        public string NextId;

        /// <summary>
        /// Уровень адресного объекта 
        /// </summary>
        public int AOLevel;

        /// <summary>
        /// Официальное наименование 
        /// </summary>
        public string OffName;

        /// <summary>
        /// Формализованное наименование
        /// </summary>
        public string FormalName;

        /// <summary>
        /// Краткое наименование типа объекта
        /// </summary>
        public string ShortName;

        /// <summary>
        /// Код региона
        /// </summary>
        public string CodeRegion;

        /// <summary>
        /// Код автономии
        /// </summary>
        public string CodeAuto;

        /// <summary>
        /// Код района
        /// </summary>
        public string CodeArea;

        /// <summary>
        /// Код города
        /// </summary>
        public string CodeCity;

        /// <summary>
        /// Код внутригородского района
        /// </summary>
        public string CodeCtar;

        /// <summary>
        /// Код населенного пункта
        /// </summary>
        public string CodePlace;

        /// <summary>
        /// Код улицы
        /// </summary>
        public string CodeStreet;

        /// <summary>
        /// Код дополнительного адресообразующего элемента
        /// </summary>
        public string CodeExtr;

        /// <summary>
        /// Код подчиненного дополнительного адресообразующего элемента
        /// </summary>
        public string CodeSext;

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode;

        /// <summary>
        /// Код ИФНС ФЛ
        /// </summary>
        public string IFNSFL;

        /// <summary>
        /// Код территориального участка ИФНС ФЛ
        /// </summary>
        public string TerrIFNSFL;

        /// <summary>
        /// Код ИФНС ЮЛ
        /// </summary>
        public string IFNSUL;

        /// <summary>
        /// Код территориального участка ИФНС ЮЛ
        /// </summary>
        public string TerrIFNSUL;

        /// <summary>
        /// ОКАТО
        /// </summary>
        public string OKATO;

        /// <summary>
        /// ОКТМО
        /// </summary>
        public string OKTMO;

        /// <summary>
        /// Дата внесения записи
        /// </summary>
        public DateTime? UpdateDate;

        ///<summary>
        /// Код адресного объекта одной строкой с признаком актуальности из КЛАДР 4.0. 
        /// </summary>
        public string KladrCode;

        ///<summary>
        /// Код адресного объекта из КЛАДР 4.0 одной строкой без признака актуальности (последних двух цифр)
        /// </summary>
        public string KladrPlainCode;

        ///<summary>
        /// Статус актуальности КЛАДР 4 (последние две цифры в коде)
        /// </summary>
        public int KladrCurrStatus;

        ///<summary>
        /// Статус актуальности адресного объекта ФИАС. Актуальный адрес на текущую дату. Обычно последняя запись об адресном объекте.
        /// 0-Не актуальный
        /// 1-Актуальный
        /// </summary>
        public int ActStatus;

        ///<summary>
        /// Статус центра
        /// </summary>
        public int CentStatus;

        ///<summary>
        /// Статус действия над записью – причина появления записи
        /// </summary>
        public int OperStatus;

        ///<summary>
        /// Начало действия записи
        /// </summary>
        public DateTime? StartDate;

        ///<summary>
        /// Окончание действия записи
        /// </summary>
        public DateTime? EndDate;

        ///<summary>
        /// Внешний ключ на нормативный документ
        /// </summary>
        public string NormDoc;

        /// <summary>
        /// Метод получает Код записи по уровням
        /// тоесть Код региона +код АО + код Района...
        /// </summary>
        /// <returns></returns>
        public string GetCodeRecord()
        {
            var result = "";

            switch (this.AOLevel)
            {
                case 1:
                    {
                        result += this.CodeRegion;
                    }
                    break;

                case 2:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                    }
                    break;

                case 3:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                    }
                    break;

                case 4:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                    }
                    break;

                case 5:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                        result += this.CodeCtar;
                    }
                    break;

                case 6:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                        result += this.CodeCtar;
                        result += this.CodePlace;
                    }
                    break;

                case 7:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                        result += this.CodeCtar;
                        result += this.CodePlace;
                        result += this.CodeStreet;
                    }
                    break;

                case 8:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                        result += this.CodeCtar;
                        result += this.CodePlace;
                        result += this.CodeStreet;
                        result += this.CodeExtr;
                    }
                    break;

                case 9:
                    {
                        result += this.CodeRegion;
                        result += this.CodeAuto;
                        result += this.CodeArea;
                        result += this.CodeCity;
                        result += this.CodeCtar;
                        result += this.CodePlace;
                        result += this.CodeStreet;
                        result += this.CodeExtr;
                        result += this.CodeSext;
                    }
                    break;
            }

            return result;
        }
    }
}