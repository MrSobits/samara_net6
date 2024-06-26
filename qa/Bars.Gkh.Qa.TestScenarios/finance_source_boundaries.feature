﻿@ScenarioInTransaction
Функционал: тесткейсы граничных значений для раздела "Разрезы финансирования"
Справочники - Капитальный ремонт - Разрезы финансирования

@ignore
#GKH-2415
Сценарий: неудачное создание нового разреза финансирования с пустыми полями, Наименование
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого рзареза финансирования заполняет поле Код "123"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования отсутствует в справочнике
И падает ошибка с текстом "Не заполнены обязательные поля: Наименование"

@ignore
#GKH-2415
Сценарий: удачное создание нового разреза финансирования с пустыми полями, Код
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование "разрез финансирования тест"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в справочнике

Сценарий: удачное создание нового разреза финансирования при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование 300 символов "1"
И пользователь у этого рзареза финансирования заполняет поле Код "123"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в справочнике

@ignore
#GKH-2415
Сценарий: неудачное создание нового разреза финансирования при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование 301 символов "1"
И пользователь у этого рзареза финансирования заполняет поле Код "123"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования отсутствует в справочнике
И падает ошибка с текстом "Количество знаков в поле Наименование не должно превышать 300 символов"

Сценарий: удачное создание нового разреза финансирования при вводе граничных условий в 200 знаков, Код
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование "разрез финансирования тест"
И пользователь у этого разреза финансирования заполняет поле Код 200 символов "1"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования присутствует в справочнике

@ignore
#GKH-2415
Сценарий: неудачное создание нового разреза финансирования при вводе граничных условий в 201 знаков, Код
Дано пользователь добавляет новый разрез финансирования
И пользователь у этого разреза финансирования заполняет поле Наименование "разрез финансирования тест"
И пользователь у этого разреза финансирования заполняет поле Код 201 символов "1"
Когда пользователь сохраняет этот разрез финансирования
Тогда запись по этому разрезу финансирования отсутствует в справочнике
И падает ошибка с текстом "Количество знаков в поле Наименование не должно превышать 300 символов"