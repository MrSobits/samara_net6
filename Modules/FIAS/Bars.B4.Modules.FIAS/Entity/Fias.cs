using System;

namespace Bars.B4.Modules.FIAS
{
    using DataAccess;

    /// <summary>
    /// Запись в таблице ФИАС
    /// </summary>
    public class Fias : PersistentObject
    {

        #region Данные поля не загружаются из ФИАС
        /// <summary>
        /// Итоговый код записи
        /// </summary>
        public virtual string CodeRecord { get; set; }

        /// <summary>
        /// Итоговый код записи
        /// Если мы добавляем новую запись, то проставляется - TypeRecord.User
        /// Если мы загружаем запись из ФИАС, то проставляется - TypeRecord.Fias
        /// </summary>
        public virtual FiasTypeRecordEnum TypeRecord { get; set; }

        /// <summary>
        /// Зеркало, для того чтобы ставить ссылки на территориальные объекты чтобы подтягивать их информацию 
        /// </summary>
        public virtual string MirrorGuid { get; set; }
        #endregion

        /// <summary>
        /// Глобальный уникальный идентификатор адресного объекта 
        /// </summary>
        public virtual string AOGuid { get; set; }

        /// <summary>
        /// Идентификатор объекта родительского объекта
        /// </summary>
        public virtual string ParentGuid { get; set; }

        /// <summary>
        /// Уникальный идентификатор записи внутри базы. Ключевое поле.
        /// </summary>
        public virtual string AOId { get; set; }

        /// <summary>
        /// Идентификатор записи связывания с предыдушей исторической записью
        /// </summary>
        public virtual string PrevId { get; set; }

        /// <summary>
        /// Идентификатор записи связывания с последующей исторической записью
        /// </summary>
        public virtual string NextId { get; set; }

        /// <summary>
        /// Уровень адресного объекта 
        /// </summary>
        public virtual FiasLevelEnum AOLevel { get; set; }

        /// <summary>
        /// Официальное наименование 
        /// </summary>
        public virtual string OffName { get; set; }

        /// <summary>
        /// Формализованное наименование
        /// </summary>
        public virtual string FormalName { get; set; }

        /// <summary>
        /// Краткое наименование типа объекта
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        public virtual string CodeRegion { get; set; }

        /// <summary>
        /// Код автономии
        /// </summary>
        public virtual string CodeAuto { get; set; }

        /// <summary>
        /// Код района
        /// </summary>
        public virtual string CodeArea { get; set; }

        /// <summary>
        /// Код города
        /// </summary>
        public virtual string CodeCity { get; set; }

        /// <summary>
        /// Код внутригородского района
        /// </summary>
        public virtual string CodeCtar { get; set; }

        /// <summary>
        /// Код населенного пункта
        /// </summary>
        public virtual string CodePlace { get; set; }

        /// <summary>
        /// Код улицы
        /// </summary>
        public virtual string CodeStreet { get; set; }

        /// <summary>
        /// Код дополнительного адресообразующего элемента
        /// </summary>
        public virtual string CodeExtr { get; set; }

        /// <summary>
        /// Код подчиненного дополнительного адресообразующего элемента
        /// </summary>
        public virtual string CodeSext { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public virtual string PostalCode { get; set; }

        /// <summary>
        /// Код ИФНС ФЛ
        /// </summary>
        public virtual string IFNSFL { get; set; }

        /// <summary>
        /// Код территориального участка ИФНС ФЛ
        /// </summary>
        public virtual string TerrIFNSFL { get; set; }

        /// <summary>
        /// Код ИФНС ЮЛ
        /// </summary>
        public virtual string IFNSUL { get; set; }

        /// <summary>
        /// Код территориального участка ИФНС ЮЛ
        /// </summary>
        public virtual string TerrIFNSUL { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public virtual string OKATO { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual string OKTMO { get; set; }

        /// <summary>
        /// Дата  внесения записи
        /// </summary>
        public virtual DateTime? UpdateDate { get; set; }

        ///<summary>
        /// Код адресного объекта одной строкой с признаком актуальности из КЛАДР 4.0. 
        /// </summary>
        public virtual string KladrCode { get; set; }

        ///<summary>
        /// Код адресного объекта из КЛАДР 4.0 одной строкой без признака актуальности (последних двух цифр)
        /// </summary>
        public virtual string KladrPlainCode { get; set; }

        ///<summary>
        /// Статус актуальности КЛАДР 4 (последние две цифры в коде)
        /// </summary>
        public virtual int KladrCurrStatus { get; set; }

        ///<summary>
        /// Статус актуальности адресного объекта ФИАС. Актуальный адрес на текущую дату. Обычно последняя запись об адресном объекте.
        /// 0-Не актуальный
        /// 1-Актуальный 
        /// </summary>
        public virtual FiasActualStatusEnum ActStatus { get; set; }

        ///<summary>
        /// Статус центра
        /// </summary>
        public virtual FiasCenterStatusEnum CentStatus { get; set; }

        ///<summary>
        /// Статус действия над записью – причина появления записи
        /// </summary>
        public virtual FiasOperationStatusEnum OperStatus { get; set; }

        ///<summary>
        /// Начало действия записи
        /// </summary>
        public virtual DateTime? StartDate { get; set; }

        ///<summary>
        /// Окончание действия записи
        /// </summary>
        public virtual DateTime? EndDate { get; set; }

        ///<summary>
        /// Внешний ключ на нормативный документ
        /// </summary>
        public virtual string NormDoc { get; set; }
    }
}
