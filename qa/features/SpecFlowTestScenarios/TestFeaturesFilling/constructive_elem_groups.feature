﻿
Функционал: тесткейсы для раздела "Группы конструктивных элементов"
Справочники - Жилищно-коммунальное хозяйство - Группы конструктивных элементов


Предыстория: 
Дано пользователь добавляет новую группу конструктивных элементов
И пользователь у этой группы конструктивных элементов заполняет поле Наименование "группа конструктивных элементов тест"


Сценарий: успешное добавление группы конструктивных элементов
Когда пользователь сохраняет эту группу конструктивных элементов
Тогда запись по этой группе конструктивных элементов присутствует в справочнике групп конструктивных элементов

Сценарий: успешное добавление группы конструктивных элементов с Обязательность = true
Дано пользователь у этой группы конструктивных элементов заполняет поле Обязательность = true
Когда пользователь сохраняет эту группу конструктивных элементов
Тогда запись по этой группе конструктивных элементов присутствует в справочнике групп конструктивных элементов

Сценарий: успешное добавление группы конструктивных элементов с Обязательность = false
Дано пользователь у этой группы конструктивных элементов заполняет поле Обязательность = false
Когда пользователь сохраняет эту группу конструктивных элементов
Тогда запись по этой группе конструктивных элементов присутствует в справочнике групп конструктивных элементов

Сценарий: успешное удаление записи из справочника групп конструктивных элементов
Когда пользователь сохраняет эту группу конструктивных элементов
И пользователь удаляет эту группу конструктивных элементов
Тогда запись по этой группе конструктивных элементов отсутствует в справочнике групп конструктивных элементов
