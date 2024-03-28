﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Периоды программ"
Справочники - Капитальный ремонт - Периоды программ


Предыстория: 
Дано пользователь добавляет новый период программы
И пользователь у этого периода программы заполняет поле Наименование "период программы тест"
И пользователь у этого периода программы заполняет поле Дата начала "01.01.2015"
И пользователь у этого периода программы заполняет поле Дата окончания "31.01.2015"

Сценарий: успешное добавление периода программы
Когда пользователь сохраняет этот период программы
Тогда запись по этому периоду программы присутствует в справочнике периодов программ

Сценарий: успешное удаление периода программы
Когда пользователь сохраняет этот период программы
И пользователь удаляет этот период программы
Тогда запись по этому периоду программы отсутствует в справочнике периодов программ

Сценарий: успешное добавление дубля периода программы
Когда пользователь сохраняет этот период программы
Дано пользователь добавляет новый период программы
И пользователь у этого периода программы заполняет поле Наименование "период программы тест"
И пользователь у этого периода программы заполняет поле Дата начала "01.01.2015"
И пользователь у этого периода программы заполняет поле Дата окончания "31.01.2015"
Когда пользователь сохраняет этот период программы
Тогда запись по этому периоду программы присутствует в справочнике периодов программ