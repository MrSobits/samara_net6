﻿
Функционал: тестирование Импорта ДПКР (Москва)


Предыстория: 
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-msk"


Структура сценария: проверка формата импортируемого файла на наличие обязательных аттрибутов
Дано файл для импорта данных
Когда пользователь импортирует файл
То система проверяет файл на соответствие формату по наличию обязательных аттрибутов в файле <аттрибут>
Если в файле не указано значение хотя бы по одному из аттрибутов <аттрибут>
То файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла, количество загруженных записей, количество незагруженных записей, прочая информация

Примеры: 
| аттрибут |
| ID       |
| UID      |
| СЕРИЯ    |
| MKDB02   |
| MKDC01   |
| MKDC02   |
| MKDC03   |
| MKDC06   |
| MKDC07   |
| MKDC08   |


Структура сценария: проверка формата импортируемого файла на наличие других аттрибутов
Дано файл для импорта данных
Когда пользователь импортирует файл
То система проверяет файл на соответствие формату по наличию аттрибутов в файле <аттрибут>
Если в файле не указано значение хотя бы по одному из аттрибутов <аттрибут>
То файл загружается
И и в лог записывается количество загруженных записей, количество незагруженных записей, прочая информация
Если в файле отсутствует хотя бы один из аттрибутов <аттрибут>
То файл не грузится и в лог записывается ошибка и причина ошибки загрузки файла

Примеры: 
| аттрибут           |
| МС (ЭС)            |
| МС (ХВС-М)         |
| МС (ХВС)           |
| МС (Ф-Б)           |
| МС (Ф)             |
| МС (СТРОП)         |
| МС (ППАиДУ)        |
| МС (ПОДВАЛ)        |
| МС (ПВ)            |
| МС (ОС-М)          |
| МС (ОС)            |
| МС (МУС)           |
| МС (КРОВЛЯ)        |
| МС (КАН-М)         |
| МС (КАН)           |
| МС (ГС)            |
| МС (ГВС-М)         |
| МС (ГВС)           |
| МС (ВДСК)          |
| КРП (ЭС)           |
| КРП (ХВС-М)        |
| КРП (ХВС)          |
| КРП (Ф-Б)          |
| КРП (Ф)            |
| КРП (СТРОП)        |
| КРП (ППАиДУ)       |
| КРП (ПОДВАЛ)       |
| КРП (ПВ)           |
| КРП (ОС-М)         |
| КРП (ОС)           |
| КРП (МУС)          |
| КРП (КРОВЛЯ)       |
| КРП (КАН-М)        |
| КРП (КАН)          |
| КРП (ГС)           |
| КРП (ГВС-М)        |
| КРП (ГВС)          |
| КРП (ВДСК)         |
| БАЛЛЫ (ЭС)         |
| БАЛЛЫ (ХВС-М)      |
| БАЛЛЫ (ХВС)        |
| БАЛЛЫ (ФАС)        |
| БАЛЛЫ (Ф-Б)        |
| БАЛЛЫ (СТРОП)      |
| БАЛЛЫ (ППА)        |
| БАЛЛЫ (ПВ)         |
| БАЛЛЫ (ОС-М)       |
| БАЛЛЫ (ОС)         |
| БАЛЛЫ (МУС)        |
| БАЛЛЫ (КРОВ)       |
| БАЛЛЫ (КАН-М)      |
| БАЛЛЫ (КАН)        |
| БАЛЛЫ (ГС)         |
| БАЛЛЫ (ГВС-М)      |
| БАЛЛЫ (ГВС)        |
| БАЛЛЫ (ВДСК)       |
| ЭСП                |
| ГСП                |
| КАНП               |
| ЦОП                |
| МУСП               |
| ППАП               |
| ХВСП               |
| ПВП                |
| ГВСП               |
| ГВС-МП             |
| ХВС-МП             |
| ЦО-МП              |
| КАН-МП             |
| ФАСП               |
| ВДСКП              |
| КРОП               |
| СТРОПП             |
| ПОДП               |


Структура сценария: проверка загрузки аттрибутов из файла к нам в систему
Дано файл для импорта данных дпкр
Когда пользователь импортирует файл
То система грузит данные из файла по аттрибутам <аттрибут>

Примеры: 
| аттрибут           |
| ID                 |
| UID                |
| MKDB02             |
| MKDC01             |
| MKDC02             |
| MKDC03             |
| MKDC06             |
| MKDC07             |
| MKDC08             |
| КРП (ЭС)           |
| КРП (ХВС-М)        |
| КРП (ХВС)          |
| КРП (Ф-Б)          |
| КРП (Ф)            |
| КРП (СТРОП)        |
| КРП (ППАиДУ)       |
| КРП (ПОДВАЛ)       |
| КРП (ПВ)           |
| КРП (ОС-М)         |
| КРП (ОС)           |
| КРП (МУС)          |
| КРП (КРОВЛЯ)       |
| КРП (КАН-М)        |
| КРП (КАН)          |
| КРП (ГС)           |
| КРП (ГВС-М)        |
| КРП (ГВС)          |
| КРП (ВДСК)         |
| БАЛЛЫ (ЭС)         |
| БАЛЛЫ (ХВС-М)      |
| БАЛЛЫ (ХВС)        |
| БАЛЛЫ (ФАС)        |
| БАЛЛЫ (Ф-Б)        |
| БАЛЛЫ (СТРОП)      |
| БАЛЛЫ (ППА)        |
| БАЛЛЫ (ПВ)         |
| БАЛЛЫ (ОС-М)       |
| БАЛЛЫ (ОС)         |
| БАЛЛЫ (МУС)        |
| БАЛЛЫ (КРОВ)       |
| БАЛЛЫ (КАН-М)      |
| БАЛЛЫ (КАН)        |
| БАЛЛЫ (ГС)         |
| БАЛЛЫ (ГВС-М)      |
| БАЛЛЫ (ГВС)        |
| БАЛЛЫ (ВДСК)       |
| ЭСП                |
| ГСП                |
| КАНП               |
| ЦОП                |
| МУСП               |
| ППАП               |
| ХВСП               |
| ПВП                |
| ГВСП               |
| ГВС-МП             |
| ХВС-МП             |
| ЦО-МП              |
| КАН-МП             |
| ФАСП               |
| ВДСКП              |
| КРОП               |
| СТРОПП             |
| ПОДП               |
| ДОМ БАЛЛЫ (ПОЛНЫЕ) |
| ОЧЕРЕДЬ            |


Структура сценария: проверка загрузки аттрибута "Год последнего ремонта"
Дано файл для импорта данных
Когда пользователь импортирует файл
И данные из аттрибута <аттрибут> загружаются без ошибок
То система находит дома по соотнесению аттрибута "uid" с аттрибутом "id" из нашей системы
И в каждом найденном доме загружаются данные из файла по конструктивным характеристикам дома

Примеры: 
| аттрибут           |
| КРП (ЭС)           |
| КРП (ХВС-М)        |
| КРП (ХВС)          |
| КРП (Ф-Б)          |
| КРП (Ф)            |
| КРП (СТРОП)        |
| КРП (ППАиДУ)       |
| КРП (ПОДВАЛ)       |
| КРП (ПВ)           |
| КРП (ОС-М)         |
| КРП (ОС)           |
| КРП (МУС)          |
| КРП (КРОВЛЯ)       |
| КРП (КАН-М)        |
| КРП (КАН)          |
| КРП (ГС)           |
| КРП (ГВС-М)        |
| КРП (ГВС)          |
| КРП (ВДСК)         |

#не использовать этот сценарий
Структура сценария: проверка загрузки аттрибута "Срок эксплуатации"
Дано файл для импорта данных
Когда пользователь импортирует файл
То система находит дома по соотнесению аттрибута "uid" с аттрибутом "id" из нашей системы
И по найденному дому в конструктивных характеристиках по аттрибутам <аттрибут> создает конструктивный элемент <новый_кэ> в соответствии с указанным в аттрибуте значением <значение> и наименованием КЭ <наименование_кэ> (формула слудующая: <новый_кэ> = <наименование_кэ> (<значение>))

Примеры: 
| аттрибут    | наименование_кэ                                                 | значение | новый_кэ                                                             |
| МС (ЭС)     | Система электроснабжения                                        | 20       | Система электроснабжения (20)                                        |
| МС (ХВС-М)  | Системы холодного водоснабжения (магистрали)                    | 30       | Системы холодного водоснабжения (магистрали) (30)                    |
| МС (ХВС)    | Системы холодного водоснабжения (стояки)                        | 21       | Системы холодного водоснабжения (стояки) (21)                        |
| МС (Ф-Б)    | Балконная плита                                                 | 30       | Балконная плита (30)                                                 |
| МС (Ф)      | Фасад                                                           | 50       | Фасад (50)                                                           |
| МС (СТРОП)  | Стропилы                                                        | 50       | Стропилы (50)                                                        |
| МС (ППАиДУ) | Внутридомовая система дымоудаления и противопожарной автоматики | 30       | Внутридомовая система дымоудаления и противопожарной автоматики (30) |
| МС (ПОДВАЛ) | Подвальные помещения, относящиеся к общему имуществу            | 21       | Подвальные помещения (21)                                            |
| МС (ПВ)     | Пожарный водопровод                                             | 30       | Пожарный водопровод (30)                                             |
| МС (ОС-М)   | Системы отопления (магистрали)                                  | 20       | Системы отопления (магистрали) (20)                                  |
| МС (ОС)     | Системы отопления (стояки)                                      | 30       | Системы отопления (стояки) (30)                                      |
| МС (МУС)    | Мусоропроводы                                                   | 45       | Мусоропроводы (45)                                                   |
| МС (КРОВЛЯ) | Крыша                                                           | 15       | Крыша (15)                                                           |
| МС (КАН-М)  | Системы канализации и водоотведения (магистрали)                | 40       | Системы канализации и водоотведения (магистрали) (40)                |
| МС (КАН)    | Системы канализации и водоотведения (стояки)                    | 40       | Системы канализации и водоотведения (стояки) (40)                    |
| МС (ГС)     | Газовые сети                                                    | 20       | Газовые сети (20)                                                    |
| МС (ГВС-М)  | Системы горячего водоснабжения (магистрали)                     | 20       | Системы горячего водоснабжения (магистрали) (20)                     |
| МС (ГВС)    | Системы горячего водоснабжения (стояки)                         | 30       | Системы горячего водоснабжения (стояки) (30)                         |
| МС (ВДСК)   | Водосток                                                        | 35       | Водосток (35)                                                        |


Сценарий: проверка повторной загрузки файла импорта
Дано файл для импорта данных
Когда пользователь повторно импортирует файл
То все перечисленные в файле данные по дому перезаписываются


Сценарий: добавление записей в ДПКР
Дано файл для импорта данных
Когда пользователь импортирует файл
То создается запись в разделе "Долгосрочная программа"
И новая запись в разделе дома "Конструктивные характеристики" не создается


Сценарий: наличие КЭ для ООИ
Когда пользователь заходит в карточку дома
Тогда для каждого ООИ количество КЭ = "1"
Но для ООИ "Фасад" и "Электроснабжение" количество КЭ >= "1"

