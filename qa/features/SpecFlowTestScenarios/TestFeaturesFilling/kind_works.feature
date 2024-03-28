﻿
Функционал: тесткейсы для раздела "Виды работ"
Справочники - Общие - Виды работ


Предыстория: 
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет новый вид работы
И пользователь у этого вида работы заполняет поле Наименование "тест"
И пользователь у этого вида работы заполняет поле Ед. измерения
И пользователь у этого вида работы заполняет поле Код "тест"
И пользователь у этого вида работы заполняет поле Норматив "111"
И пользователь у этого вида работы заполняет поле Описание "тест"

Сценарий: успешное добавление вида работы c заполненными обязательными полями
Когда пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ

Сценарий: успешное добавление вида работы c пометкой в поле Соответствие 185 ФЗ
Когда пользователь у этого вида работы помечает поле Соответствие 185 ФЗ
И пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ
И значение в поле Соответствие 185 ФЗ = true

Сценарий: успешное добавление вида работы c пометкой в поле Доп. работа
Когда пользователь у этого вида работы помечает поле Доп. работа
И пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ
И значение в поле Соответствие Доп. работа = true

Сценарий: успешное добавление вида работы c пометками в поле Соответствие 185 ФЗ, Доп. работа
Когда пользователь у этого вида работы помечает поле Соответствие 185 ФЗ
И пользователь у этого вида работы помечает поле Доп. работа
И пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ
И значение в поле Соответствие 185 ФЗ = true
И значение в поле Соответствие Доп. работа = true

Структура сценария: успешное добавление вида работы с заполненным полем Тип работ
Когда пользователь у этого вида работы заполняет поле Тип работ "<тип работ>"
И пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ

Примеры: 
| тип работ |
| Работа    |
| Услуга    |

Сценарий: успешное добавление вида работы c пометках в источниках финансирования
Когда пользователь у этого вида работы помечает поле Бюджет фонда
И пользователь у этого вида работы помечает поле Бюджет региона
И пользователь у этого вида работы помечает поле Бюджет МО
И пользователь у этого вида работы помечает поле Средства собственника
И пользователь у этого вида работы помечает поле Иные источники
И пользователь сохраняет этот вид работы
Тогда запись по этому виду работы присутствует в реестре видов работ
И значение в поле Бюджет фонда = true
И значение в поле Бюджет региона = true
И значение в поле Бюджет МО = true
И значение в поле Средства собственника = true
И значение в поле Иные источники = true

Сценарий: успешное удаление вида работы c заполненными обязательными полями
Когда пользователь сохраняет этот вид работы
И пользователь удаляет этот вид работы
Тогда запись по этому виду работы отсутствует в реестре видов работ