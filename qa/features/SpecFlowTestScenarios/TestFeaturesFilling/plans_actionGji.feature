﻿
Функционал: тесткейсы для раздела "Планы мероприятий"
Справочники - ГЖИ - Планы мероприятий


Предыстория: 
Дано пользователь добавляет новый план мероприятия
И пользователь у этого плана мероприятия заполняет поле Наименование "план мероприятия кровли тест"
И пользователь у этого плана мероприятия заполняет поле Дата начала "01.01.2015"
И пользователь у этого плана мероприятия заполняет поле Дата окончания "31.12.2016"

Сценарий: успешное добавление плана мероприятия
Когда пользователь сохраняет этот план мероприятия
Тогда запись по этому плану мероприятия присутствует в справочнике планов мероприятий

Сценарий: успешное удаление записи из справочника планов мероприятий
Когда пользователь сохраняет этот план мероприятия
И пользователь удаляет этот план мероприятия
Тогда запись по этому плану мероприятия отсутствует в справочнике планов мероприятий

Сценарий: успешное добавление дубля плана мероприятия
Когда пользователь сохраняет этот план мероприятияли
Допустим пользователь добавляет новый план мероприятия
И пользователь у этого плана мероприятия заполняет поле Наименование "план мероприятия кровли тест"
И пользователь у этого плана мероприятия заполняет поле Дата начала "01.01.2015"
И пользователь у этого плана мероприятия заполняет поле Дата окончания "31.12.2016"
Когда пользователь сохраняет этот план мероприятия
Тогда запись по этому плану мероприятия присутствует в справочнике планов мероприятий