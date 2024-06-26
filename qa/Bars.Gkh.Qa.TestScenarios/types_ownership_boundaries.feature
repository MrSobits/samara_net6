﻿@ScenarioInTransaction
Функционал: тесткейсы граничных значений для раздела "Формы собственности"
Справочники - Жилищно-коммунальное хозяйство - Формы собственности

@ignore
#GKH-2295
Сценарий: создание новой формы собственности с пустыми полями
Дано пользователь добавляет новую форму собственности 
Когда пользователь сохраняет эту форму собственности
Тогда созданная форма собственности появляется в справочнике

Сценарий: удачное создание нового формы собственности при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новую форму собственности 
И пользователь у этой формы собственности заполняет поле Наименование 300 символов "1"
Когда пользователь сохраняет эту форму собственности
Тогда созданная форма собственности появляется в справочнике

@ignore
#GKH-2295
Сценарий: неудачное создание новой формы собственности при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новую форму собственности 
И пользователь у этой формы собственности заполняет поле Наименование 301 символов "1"
Когда пользователь сохраняет эту форму собственности
Тогда созданная форма собственности отсутствует в справочнике
И падает ошибка с текстом "Количество знаков в поле Наименование не должно превышать 300 символов"
#И количество символов в поле Наименование = 300
#И выходит предупреждение с текстом "Наименование сохранено не полностью, так как превышена максимально возможная длина в 300 символов"