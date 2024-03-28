﻿
Функционал: тесткейсы для добавления видов работ к разрезу финансирования в разделе "Разрезы финансирования"
Справочники - Капитальный ремонт - Разрезы финансирования

Предыстория: 
Дано добавлен разрез финансирования
| Name                           | TypeFinanceGroup   | TypeFinance |
| разрез финансирования тестовый | ул. 50 лет Октября | Другие      |

И добавлена единица измерения
| Name                       | ShortName | Description |
| тестовая единица измерения | тест      | тест        |

И добавлен вид работы
| Name                 | UnitMeasure                 | Code  |
| тестовый вид работы1 | тестовая единица измерения1 | тест1 |
| тестовый вид работы2 | тестовая единица измерения2 | тест2 |


Сценарий: успешное добавление вида работ к разрезу финансирования
Дано пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы1
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы2
Когда пользователь сохраняет этот разрез финансирования
Тогда записи по этим видам работ присутствуют в этом разрезе финансирования

Сценарий: успешное удаление вида работ из разреза финансирования
Дано пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы1
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы2
Когда пользователь сохраняет этот разрез финансирования
И пользователь удаляет эту запись по виду работы тестовый вид работы1
И пользователь удаляет эту запись по виду работы тестовый вид работы2
Тогда записи по этим видам работ отсутствуют в этом разрезе финансирования

Сценарий: неудачное удаление разреза финансирования
Дано пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы1
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы2
Когда пользователь сохраняет этот разрез финансирования
И пользователь удаляет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в разделе разрезов финансирования
И падает ошибка с текстом "Виды работ источников финансирования"

Сценарий: неудачное добавление дубля вида работ к разрезу финансирования
Дано пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы1
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы2
Когда пользователь сохраняет этот разрез финансирования
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы1
И пользователь к этому разрезу финансирования добавляет запись по виду работы тестовый вид работы2
Тогда дубли записей по этим видам работ отсутствуют в этом разрезе финансирования