using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities.PosAppeal
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    public class Rootobject
    {
        /// <summary>
        /// Содержимое
        /// </summary>
        public Content[] content { get; set; }

        /// <summary>
        /// Признак разбивки по страницам (пагинации)
        /// </summary>
        public Pageable pageable { get; set; }

        /// <summary>
        /// Количество элементов (всего)
        /// </summary>
        public int totalElements { get; set; }

        /// <summary>
        /// Количество страниц
        /// </summary>
        public int totalPages { get; set; }

        /// <summary>
        /// Является последним в перечне получаемых
        /// </summary>
        public bool last { get; set; }

        /// <summary>
        /// Является первым в перечне получаемых
        /// </summary>
        public bool first { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public int number { get; set; }

        /// <summary>
        /// Признак сортировки
        /// </summary>
        public Sort1 sort { get; set; }

        /// <summary>
        /// Количество элементов в данном экземпляре
        /// </summary>
        public int numberOfElements { get; set; }

        /// <summary>
        /// Размер
        /// </summary>
        public int size { get; set; }

        /// <summary>
        /// Не содержит информации
        /// </summary>
        public bool empty { get; set; }
    }

    public class Pageable
    {
        /// <summary>
        /// Признак сортировки
        /// </summary>
        public Sort sort { get; set; }

        /// <summary>
        /// Размер страницы
        /// </summary>
        public int pageSize { get; set; }

        /// <summary>
        /// Номер страницы
        /// </summary>
        public int pageNumber { get; set; }

        /// <summary>
        /// Отступ (от начала страницы?)
        /// </summary>
        public int offset { get; set; }

        /// <summary>
        /// Присутствует пагинация
        /// </summary>
        public bool paged { get; set; }

        /// <summary>
        /// Без пагинации
        /// </summary>
        public bool unpaged { get; set; }
    }

    public class Sort
    {
        /// <summary>
        /// Отсортировано
        /// </summary>
        public bool sorted { get; set; }

        /// <summary>
        /// Не сортировано
        /// </summary>
        public bool unsorted { get; set; }

        /// <summary>
        /// Не содержит информации
        /// </summary>
        public bool empty { get; set; }
    }

    public class Sort1
    {
        /// <summary>
        /// Отсортировано
        /// </summary>
        public bool sorted { get; set; }

        /// <summary>
        /// Не сортировано
        /// </summary>
        public bool unsorted { get; set; }

        /// <summary>
        /// Не содержит информации
        /// </summary>
        public bool empty { get; set; }
    }

    public class Content
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public string regNumber { get; set; }

        /// <summary>
        /// Дата присвоения регистрационного номера
        /// </summary>
        public string regNumberSetAt { get; set; }

        /// <summary>
        /// ID категории
        /// </summary>
        public int subjectId { get; set; }

        /// <summary>
        /// Наименование/имя категории
        /// </summary>
        public string subjectName { get; set; }

        /// <summary>
        /// ID подкатегории
        /// </summary>
        public int subsubjectId { get; set; }

        /// <summary>
        /// Наименование/имя подкатегории
        /// </summary>
        public string subsubjectName { get; set; }

        /// <summary>
        /// ID факта
        /// </summary>
        public object factId { get; set; }

        /// <summary>
        /// Наименование факта
        /// </summary>
        public object factName { get; set; }

        /// <summary>
        /// Ответить до
        /// </summary>
        public DateTime answerAt { get; set; }

        /// <summary>
        /// Признак фасттрека
        /// </summary>
        public bool fastTrack { get; set; }

        /// <summary>
        /// Время создания
        /// </summary>
        public DateTime createdAt { get; set; }

        /// <summary>
        /// ID региона
        /// </summary>
        public string regionId { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        public string regionName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// ID ЛКО
        /// </summary>
        public int opaId { get; set; }

        /// <summary>
        /// Наименование ЛКО
        /// </summary>
        public string opaName { get; set; }

        /// <summary>
        /// Признак коллективного сообщения
        /// </summary>
        public bool shared { get; set; }

        /// <summary>
        /// Информация о заявителе
        /// </summary>
        public Applicant applicant { get; set; }

        /// <summary>
        /// Информация о созаявителях
        /// </summary>
        public object coApplicants { get; set; }

        /// <summary>
        /// Ссылки на прикрепленные файлы
        /// </summary>
        public string[] attachments { get; set; }

        /// <summary>
        /// Список настраиваемых полей
        /// </summary>
        public object[] customFieldValues { get; set; }

        /// <summary>
        /// Координаты адреса проблемы
        /// </summary>
        public string coordinates { get; set; }

        /// <summary>
        /// Информация об адресе проблемы из dadata
        /// </summary>
        public Locationaddress locationAddress { get; set; }
    }

    public class Applicant
    {
        /// <summary>
        /// Фамилия заявителя
        /// </summary>
        public string surname { get; set; }

        /// <summary>
        /// Имя заявителя
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Отчество заявителя
        /// </summary>
        public string patronymic { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string postAddress { get; set; }

        /// <summary>
        /// Признак необходимости отправки ответа по почте
        /// </summary>
        public bool sendWithRussiaPost { get; set; }
    }

    public class Locationaddress
    {
        /// <summary>
        /// ID записи
        /// </summary>
        public int? id { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string postalCode { get; set; }

        /// <summary>
        /// Наименование страны
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// ФИАС идентификатор региона
        /// </summary>
        public string regionFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор региона
        /// </summary>
        public string regionKladrId { get; set; }

        /// <summary>
        /// Регион с описанием типа
        /// </summary>
        public string regionWithType { get; set; }

        /// <summary>
        /// Тип региона
        /// </summary>
        public string regionType { get; set; }

        /// <summary>
        /// Регион с полным описанием типа
        /// </summary>
        public string regionTypeFull { get; set; }

        /// <summary>
        /// ФИАС идентификатор области
        /// </summary>
        public string areaFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор области
        /// </summary>
        public string areaKladrId { get; set; }

        /// <summary>
        /// Область с описанием типа
        /// </summary>
        public string areaWithType { get; set; }

        /// <summary>
        /// Тип области
        /// </summary>
        public string areaType { get; set; }

        /// <summary>
        /// Область с полным описанием типа
        /// </summary>
        public string areaTypeFull { get; set; }

        /// <summary>
        /// Наименование области/региона
        /// </summary>
        public string area { get; set; }

        /// <summary>
        /// ФИАС идентификатор города
        /// </summary>
        public string cityFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор города
        /// </summary>
        public string cityKladrId { get; set; }

        /// <summary>
        /// Город с описанием типа
        /// </summary>
        public string cityWithType { get; set; }

        /// <summary>
        /// Тип города
        /// </summary>
        public string cityType { get; set; }

        /// <summary>
        /// Город с полным описанием типа
        /// </summary>
        public string cityTypeFull { get; set; }

        /// <summary>
        /// Наименование города
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Область города
        /// </summary>
        public object cityArea { get; set; }

        /// <summary>
        /// ФИАС идентификатор района города
        /// </summary>
        public object cityDistrictFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор района города
        /// </summary>
        public object cityDistrictKladrId { get; set; }

        /// <summary>
        /// Район города с описанием типа
        /// </summary>
        public object cityDistrictWithType { get; set; }

        /// <summary>
        /// Тип района города
        /// </summary>
        public object cityDistrictType { get; set; }

        /// <summary>
        /// Район города с полным описанием типа
        /// </summary>
        public object cityDistrictTypeFull { get; set; }

        /// <summary>
        /// Наименование района города
        /// </summary>
        public object cityDistrict { get; set; }

        /// <summary>
        /// ФИАС идентификатор поселения
        /// </summary>
        public string settlementFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор поселения
        /// </summary>
        public string settlementKladrId { get; set; }

        /// <summary>
        /// Поселение с описанием типа
        /// </summary>
        public string settlementWithType { get; set; }

        /// <summary>
        /// Тип поселения
        /// </summary>
        public string settlementType { get; set; }

        /// <summary>
        /// Поселение с полным описанием типа
        /// </summary>
        public string settlementTypeFull { get; set; }

        /// <summary>
        /// Наименование поселения
        /// </summary>
        public string settlement { get; set; }

        /// <summary>
        /// ФИАС идентификатор улицы
        /// </summary>
        public string streetFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор улицы
        /// </summary>
        public string streetKladrId { get; set; }

        /// <summary>
        /// Улица с описанием типа
        /// </summary>
        public string streetWithType { get; set; }

        /// <summary>
        /// Тип улицы
        /// </summary>
        public string streetType { get; set; }

        /// <summary>
        /// Улица с полным описанием типа
        /// </summary>
        public string streetTypeFull { get; set; }

        /// <summary>
        /// Наименование улицы
        /// </summary>
        public string street { get; set; }

        /// <summary>
        /// ФИАС идентификатор дома
        /// </summary>
        public string houseFiasId { get; set; }

        /// <summary>
        /// КЛАДР идентификатор дома
        /// </summary>
        public string houseKladrId { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public string houseType { get; set; }

        /// <summary>
        /// Дом с полным описанием типа
        /// </summary>
        public string houseTypeFull { get; set; }

        /// <summary>
        /// Наименование дома
        /// </summary>
        public string house { get; set; }

        /// <summary>
        /// Тип корпуса
        /// </summary>
        public string blockType { get; set; }

        /// <summary>
        /// Корпус с полным описанием типа
        /// </summary>
        public string blockTypeFull { get; set; }

        /// <summary>
        /// Наименование корпуса
        /// </summary>
        public string block { get; set; }

        /// <summary>
        /// Тип квартиры
        /// </summary>
        public object flatType { get; set; }

        /// <summary>
        /// Квартира с полным описанием типа
        /// </summary>
        public object flatTypeFull { get; set; }

        /// <summary>
        /// Наименование квартиры
        /// </summary>
        public object flat { get; set; }

        /// <summary>
        /// Площадь квартиры
        /// </summary>
        public object flatArea { get; set; }

        /// <summary>
        /// Цена квадратного метра квартиры
        /// </summary>
        public float? squareMeterPrice { get; set; }

        /// <summary>
        /// Стоимость квартиры
        /// </summary>
        public object flatPrice { get; set; }

        /// <summary>
        /// Почтовый ящик
        /// </summary>
        public object postalBox { get; set; }

        /// <summary>
        /// ФИАС идентификатор адреса
        /// </summary>
        public string fiasId { get; set; }

        /// <summary>
        /// ФИАС код адреса
        /// </summary>
        public string fiasCode { get; set; }

        /// <summary>
        /// ФИАС уровень адреса
        /// </summary>
        public string fiasLevel { get; set; }

        /// <summary>
        /// Актуальное состояние ФИАС
        /// </summary>
        public string fiasActualityState { get; set; }

        /// <summary>
        /// КЛАДР идентификатор адреса
        /// </summary>
        public string kladrId { get; set; }

        /// <summary>
        /// Идентификатор геолокации
        /// </summary>
        public string geonameId { get; set; }

        /// <summary>
        /// Рынок капитала адреса
        /// </summary>
        public string capitalMarker { get; set; }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public string okato { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public string oktmo { get; set; }

        /// <summary>
        /// Налоговая служба
        /// </summary>
        public string taxOffice { get; set; }

        /// <summary>
        /// Налоговая инспекция
        /// </summary>
        public string taxOfficeLegal { get; set; }

        /// <summary>
        /// Часовой пояс
        /// </summary>
        public string timezone { get; set; }

        /// <summary>
        /// Географическая широта
        /// </summary>
        public float? geoLat { get; set; }

        /// <summary>
        /// Географическая долгота
        /// </summary>
        public float? geoLon { get; set; }

        /// <summary>
        /// Внутри кольцевой?
        /// </summary>
        public object beltwayHit { get; set; }

        /// <summary>
        /// Расстояние от кольцевой в километрах
        /// </summary>
        public object beltwayDistance { get; set; }

        /// <summary>
        /// Код точности координат
        /// </summary>
        public string qcGeo { get; set; }

        /// <summary>
        /// Подходит для рассылки?
        /// </summary>
        public object qcComplete { get; set; }

        /// <summary>
        /// Признак наличия дома в ФИАС
        /// </summary>
        public object qcHouse { get; set; }

        /// <summary>
        /// История значений
        /// </summary>
        public object historyValues { get; set; }

        /// <summary>
        /// Неразобранные части
        /// </summary>
        public object unparsedParts { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        public string source { get; set; }

        /// <summary>
        /// Код проверки - требуется ли вручную проверить распознанное значение
        /// </summary>
        public object qc { get; set; }

        /// <summary>
        /// Идентификатор региона
        /// </summary>
        public int? regionId { get; set; }
    }
}