﻿@ScenarioInTransaction
Функция: тесткейсы для раздела "Госпошлина"
Справочники - Претензионная работа - Госпошлина

Предыстория:
Дано добавлено заявление в суд
| Code  | ShortName | FullName |
| 1тест | 1тест     | 1тест    |

И пользователь добавляет новую госпошлину

Структура сценария: успешное добавление госпошлины
Дано пользователь у этой госпошлины заполняет поле Тип суда "<Тип суда>"
Когда пользователь сохраняет эту госпошлину
Тогда запись по этой госпошлине присутствует в справочнике госпошлин
#И выходит сообщение с текстом "Госпошлина успешно сохранена"

Примеры:
| Тип суда                       |
| Судебный участок мирового суда |
| Арбитражный суд                |
| Районный (городской) суд       |

Структура сценария: успешное удаление госпошлины
Дано пользователь у этой госпошлины заполняет поле Тип суда "<Тип суда>"
Когда пользователь сохраняет эту госпошлину
И пользователь удаляет эту госпошлину
Тогда запись по этой госпошлине отсутствует в справочнике госпошлин

Примеры:
| Тип суда                       |
| Судебный участок мирового суда |
| Арбитражный суд                |
| Районный (городской) суд       |

Структура сценария: успешное добавление типа заявления к госпошлине
Дано пользователь у этой госпошлины заполняет поле Тип суда "<Тип суда>"
Когда пользователь сохраняет эту госпошлину
И пользователь добавляет к этой госпошлине это заявление в суд
Тогда запись по этому типу заявления притсутствует в списке заявлений этой госпошлины

Примеры:
| Тип суда                       |
| Судебный участок мирового суда |
| Арбитражный суд                |
| Районный (городской) суд       |