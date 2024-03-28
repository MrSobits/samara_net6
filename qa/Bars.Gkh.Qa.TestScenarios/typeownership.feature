﻿@ScenarioInTransaction
Функция: справочник форм собственности
Справочник - Жилищно-коммунальное хозяйство - Формы собственности

Предыстория: 
Дано пользователь добавляет новую форму собственности
И у этой формы собственности устанавливает поле Наименование "Тест"

Сценарий: создание формы собственности в справочнике
Когда пользователь сохраняет эту форму собственности
Тогда созданная форма собственности появляется в справочнике

Сценарий: удаление формы собственности в справочнике
Когда пользователь сохраняет эту форму собственности
И пользователь удаляет эту форму собственности
Тогда созданная форма собственности отсутствует в справочнике