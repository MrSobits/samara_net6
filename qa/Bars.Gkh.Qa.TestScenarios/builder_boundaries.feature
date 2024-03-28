﻿@ScenarioInTransaction

Функционал: тесткейсы граничных значений для раздела "Подрядные организации"
Участники процесса - Роли контрагента - Подрядные организации

Предыстория: 
Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано добавлен контрагент с организационно правовой формой
| Name |
| тест |

#GKH-2847
@ignore
Сценарий: неудачное добавление подрядной организации при незаполненных обязательных полях
Дано пользователь добавляет новую подрядную организацию
Когда пользователь сохраняет эту подрядную организацию
Тогда запись по этой подрядной организации отсутствует в разделе подрядных организаций
И падает ошибка с текстом "Не заполнены обязательные поля: Контрагент"


Сценарий: успешное добавление подрядной организации при вводе граничных условий в 500 знаков, Описание
Дано пользователь добавляет новую подрядную организацию
И пользователь у этой подрядной организации заполняет поле Контрагент
И пользователь у этой подрядной организации заполняет поле Описание 500 символов "1"
Когда пользователь сохраняет эту подрядную организацию
Тогда запись по этой подрядной организации присутствует в разделе подрядных организаций

#GKH-2847
@ignore
Сценарий: неудачное добавление подрядной организации при вводе граничных условий в 501 знаков, Описание
Дано пользователь добавляет новую подрядную организацию
И пользователь у этой подрядной организации заполняет поле Контрагент
И пользователь у этой подрядной организации заполняет поле Описание 501 символов "1"
Когда пользователь сохраняет эту подрядную организацию
Тогда запись по этой подрядной организации отсутствует в разделе подрядных организаций
И падает ошибка с текстом "Количество знаков в поле Описание не должно превышать 500 символов"