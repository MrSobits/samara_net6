﻿
Функционал: тесткейсы для раздела "Группы капитальности"
Справочники - Жилищно-коммунальное хозяйство - Группы капитальности


Предыстория: 
Дано пользователь добавляет новую группу капитальности
И пользователь у этой группы капитальности заполняет поле Наименование "тест"
И пользователь у этой группы капитальности заполняет поле Код "тест"
И пользователь у этой группы капитальности заполняет поле Описание "тест"


Сценарий: успешное добавление группы капитальности
Когда пользователь сохраняет эту группу капитальности
Тогда запись по этой группе капитальности присутствует в справочнике групп капитальности

Сценарий: успешное удаление записи из справочника групп капитальности
Когда пользователь сохраняет эту группу капитальности
И пользователь удаляет эту группу капитальности
Тогда запись по этой группе капитальности отсутствует в справочнике групп капитальности

Сценарий: успешное добавление дубля группы капитальности
Когда пользователь сохраняет эту группу капитальности
Дано пользователь добавляет новую группу капитальности
И пользователь у этой группы капитальности заполняет поле Наименование "тест"
И пользователь у этой группы капитальности заполняет поле Код "тест"
И пользователь у этой группы капитальности заполняет поле Описание "тест"
Когда пользователь сохраняет эту группу капитальности
Тогда запись по этой группе капитальности присутствует в справочнике групп капитальности