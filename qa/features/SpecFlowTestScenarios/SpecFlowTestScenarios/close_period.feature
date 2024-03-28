﻿
Функционал: Закрытие периода начислений
описание функционала закрытия периода 


#на примере РТ
Структура сценария: наполнение реестра жилых домов перед проведением закрытия периода
Когда пользователь добавляет дом в Реестр жилых домов
И у этого дома устанавливает поле Населённый пункт "<Населённый Пункт>"
И у этого дома устанавливает поле Улица "<Улица>"
И у этого дома устанавливает поле Номер Дома "<Номер Дома>"
И у этого дома устанавливает поле Вид дома "<Вид дома>"
И сохраняет этот дом
Тогда запись по новому дому добавляется в реестр жилых домов

Примеры: 
| Населённый Пункт                               | Улица       | Номер Дома | Вид дома        |
| Татарстан Респ, Пестречинский р-н, с. Пестрецы | Вишневая ул | 72         | Многоквартирный |


Сценарий: наполнение реестра лс данными через импорт абонентов
Когда пользователь в Импорте абонентов прикрепил файл импорта "имя" "с заменой" существующих сведений
То в Логах загрузок появляется запись с типом "Импорт абонентов" текущей датой и временем импорта
Если по импорту абонентов количество ошибок = "0"
То в реестре лицевых счетов добавлены новые записи из файла импорта
Если по по импорту абонентов количество ошибок < "0"
То пользователь скачивает лог импорта


Сценарий: наполнение реестра лс данными через реестр абонентов
Когда пользователь в доме по адресу = "ул. Совхозная, д. 111" добавляет помещение, где номер квартиры = "900", общая площадь = "51"
То в карточке дома в сведениях о помещениях появляется новая строка 
Когда пользователь в реестре абонентов добавляет нового абонента и заполняет имя = "т", фамилию = "т", отчество = "т"
То в реестре абонентов появляется новая строка
И у абонента "т т т" количество лс = "0"
Когда пользователь по абоненту "т т т" добавляет жилой дом "ул. Совхозная, д. 111" 
И выбирает квартиру = "900" и выбирает у квартиры долю собственности = "1" 
То у абонента "т т т" количество лс = "1"
И в реестре лс появляется новая запись


Сценарий: создание первичного периода начислений
Когда пользователь заходит в периоды начислений
И ни одного периода еще нет
То Период автоматически создается


Структура сценария: добавление к дому протокола решений собственников жилых помещений и привязка к счету регопа
Дано пользователь выбирает дом = "л. Совхозная, д. 111"
И добавлячет к нему протокол типа = "Протокол решения собственников жилых помещений" 
И заполняет у протокола поля Номер <Номер>
И заполняет у протокола поля Дата протокола <Дата протокола>
И заполняет у протокола поля Дата вступления в силу <Дата вступления в силу>
И заполняет у протокола поля Кредитная организация <Кредитная организация>
И заполняет у протокола поля способ формирования фонда <способ формирования фонда>
И формирует уведомление с номером = "1" и номером счета = "112"
Когда пользователь переводит протокол в статус = "Утверждено"
Тогда в счете регоператора дом доступен для прикрепления к счету и пользователь прикрепляет дом к счету регопа

Примеры: 
| Номер | Дата протокола | Дата вступления в силу | Кредитная организация | способ формирования фонда    |
| 1     | 01.01.2015     | 01.01.2015             | ОАО "Россельхозбанк"  | Счет регионального оператора |


Структура сценария: добавление к дому протокола решений собственников жилых помещений и привязка к сцец счету
Дано пользователь выбирает дом = "ул. Совхозная, д. 111"
И добавлячет к нему протокол типа = "Протокол решения собственников жилых помещений"
И заполняет у протокола поля Номер <Номер>
И заполняет у протокола поля Дата протокола <Дата протокола>
И заполняет у протокола поля Дата вступления в силу <Дата вступления в силу>
И заполняет у протокола поля Кредитная организация <Кредитная организация>
И заполняет у протокола поля способ формирования фонда <способ формирования фонда>
И заполняет у протокола поля Владелец специального счета <Владелец специального счета>
И формирует уведомление с номером = "2" и номером счета = "113"
Когда пользователь переводит протокол в статус = "Утверждено"
Тогда в спец счете регоператора дом доступен для прикрепления к счету и пользователь прикрепляет дом к счету регопа

Примеры: 
| Номер | Дата протокола | Дата вступления в силу | Кредитная организация | способ формирования фонда    | Владелец специального счета |
| 1     | 01.01.2015     | 01.01.2015             | ОАО "Россельхозбанк"  | Счет регионального оператора | Региональный оператор       |


Структура сценария: добавление к дому протокола решения органа государственной власти и привязка к счету регопа
Дано пользователь выбирает дом = "ул. Совхозная, д. 111"
И добавлячет к нему протокол типа = "Протокол решения органа государственной власти"
И заполняет у протокола поля Номер <Номер>
И заполняет у протокола поля Дата протокола <Дата протокола>
И заполняет у протокола поля Дата вступления в силу <Дата вступления в силу>
И заполняет у протокола поля Кредитная организация <Кредитная организация>
И заполняет у протокола поля Способ формирования фонда на счету регионального оператора <Способ формирования фонда на счету регионального оператора>
Когда пользователь переводит протокол в статус = "Утверждено"
Тогда в счете регоператора дом доступен для прикрепления к счету и пользователь прикрепляет дом к счету регопа

Примеры: 
| Номер | Дата протокола | Дата вступления в силу | Кредитная организация | Способ формирования фонда на счету регионального оператора |
| 1     | 01.01.2015     | 01.01.2015             | ОАО "Россельхозбанк"  | true                                                       |


Структура сценария: добавление к дому протокола решения органа государственной власти и привязка к спец счету
Дано пользователь выбирает дом = "ул. Совхозная, д. 111"
И добавлячет к нему протокол типа = "Протокол решения органа государственной власти"
И заполняет у протокола поля Номер <Номер>
И заполняет у протокола поля Дата протокола <Дата протокола>
И заполняет у протокола поля Дата вступления в силу <Дата вступления в силу>
И заполняет у протокола поля Кредитная организация <Кредитная организация>
И заполняет у протокола поля Способ формирования фонда на счету регионального оператора <Способ формирования фонда на счету регионального оператора>
Когда пользователь переводит протокол в статус = "Утверждено"
Тогда дома нет в списке домов на счете регопа и нет в списке домов у спец счета

Примеры: 
| Номер | Дата протокола | Дата вступления в силу | Кредитная организация | Способ формирования фонда на счету регионального оператора |
| 1     | 01.01.2015     | 01.01.2015             | ОАО "Россельхозбанк"  | false                                                      |


#Структура сценария: заполнение настройки параметров РФ
#Когда пользователь заходит в настройку параметров рф
#Тогда для checkbox <название> проставлены галочки

#Примеры: 
#| название                                 |
#| Счет регионального оператора             |
#| Специальный счет регионального оператора |
#| Специальный счет                         |
#| Не выбран                                |


Структура сценария: заполнение размеров взносов на КР
Когда пользователь добавляет запись в Размеры взносов на КР
И заполняет аттрибуты <аттрибут> значениеми <значение>
Тогда в появляется новая запись в Размерах взносов на КР

Примеры: 
| аттрибут   | значение   |
| с          | по         |
| 01.01.2015 | 31.12.2015 |


Сценарий: подтверждение начислений после расчета
Дано лицевые счета есть в реестре лицевых счетов
Дано на домах есть протоколы расчета в статусе "Утверждено"
Когда пользователь вызывает операцию расчета лс
Тогда в Реестре неподтвержденных начислений появляется запись с количеством ЛС, которые попадают в условия расчета лс
Если начисления не подтверждены 
И пользователь заврывает период
Тогда период не закрывается и падает ошибка с предупреждением о необходимости подтверждения начислений
Когда пользователь подтверждает начисления в реестре неподтвержденных начислений
Тогда у записи по выбранным начислениям в неподтвержденных начислениях меняется статус с "Ожидание" на "Подтверждено"
И создается протокол расчета по каждому лс за текущий период


Сценарий: операция закрытие периода
Дано лицевые счета есть в реестре лицевых счетов
Дано на домах есть протоколы расчета в статусе "Утверждено" и с привязкой к счету регопа или к спец счету
Дано на счете регопа или спец счета привязаны дома
Дано произведен расчет начислений и подтверждены начисления
Когда пользователь закрывает текущий период
То на ЛС, по которым был произведен расчет, появляются начисления по закрывающемуся периоду
И на домах, которые привязаны к ЛС в счете начислений появляются начисления по закрывающемуся периоду
И рассчитывается исходящее сальдо по закрывающемуся периоду
И обновляется баланс счета (на доме, и в ЛС)
И статус текущего периода = "Закрыт" и дата закрытия = текущей дате
И в периодах появляется новая запись с новым периодом, у которой статус = "Открыт"
И в счете дома и в карточке ЛС появляется новая запись по новому периоду
И входящее сальдо нового периода равно исходящему сальдо прошлого периода


Сценарий: сбой закрытия периода
Дано лицевые счета есть в реестре лицевых счетов
Дано на домах есть протоколы расчета в статусе "Утверждено" и с привязкой к счету регопа или к спец счету
Дано на счете регопа или спец счета привязаны дома
Дано произведен расчет начислений и подтверждены начисления
Дано пользователь запустил закрытие периода
Дано на ЛС, по которым был произведен расчет, появляются начисления по закрывающемуся периоду
Когда произошел сбой в системе/выключили свет/упал сервак
И пользователь повторно запустил закрытие периода
То начисления, произведенные не до конца при первом закрытии периода, откатываюсь и задвоений по суммам нет

