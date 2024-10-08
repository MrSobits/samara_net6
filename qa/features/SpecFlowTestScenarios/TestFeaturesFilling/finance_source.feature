﻿
Функция: тесткейсы для раздела "Разрезы финансирования"
Справочники - Капитальный ремонт - Разрезы финансирования


Предыстория: 
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование "разрез финансирования тестовый"
И пользователь у этого разреза финансирования заполняет поле Код "тест"


Сценарий: успешное добавление разреза финансирования
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в разделе разрезов финансирования

Структура сценария: успешное добавление разреза финансирования для поля Группа финансирования
Дано пользователь у этого вида работы заполняет поле Группа финансирования <Группа финансирования>
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в разделе разрезов финансирования

Примеры: 
| Группа финансирования |
| Другие                |
| Программа КР          |

Структура сценария: успешное добавление разреза финансирования для поля Тип разреза
Дано пользователь у этого вида работы заполняет поле Тип разреза <Тип разреза>
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в разделе разрезов финансирования

Примеры: 
| Тип разреза       |
| Другие            |
| Лизинг            |
| Не указано        |
| Федеральный закон |

Сценарий: успешное удаление разреза финансирования
Когда пользователь сохраняет этот разрез финансирования
И пользователь удаляет этот разрез финансирования
Тогда запись по этому разрезу финансирования отсутствует в разделе разрезов финансирования

Сценарий: успешное добавление дубля разреза финансирования
Когда пользователь сохраняет этот разрез финансирования
И пользователь добавляет новый разрез финансирования
И пользователь у этого вида работы заполняет поле Наименование "разрез финансирования тестовый"
И пользователь у этого вида работы заполняет поле Код "тест"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в разделе разрезов финансирования