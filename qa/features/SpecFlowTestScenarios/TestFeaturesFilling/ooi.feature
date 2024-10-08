﻿
Функционал: тесткейсы для раздела "Объекты общего имущества"
Справочники - Капитальный ремонт - Объекты общего имущества


Предыстория: 
Дано добавлен тип группы ООИ
| Code | Name                |
| тест | тип группы оои тест |

Дано пользователь добавляет новый объект общего имущества
И пользователь у этого объекта общего имущества заполняет поле Код "тест"
И пользователь у этого объекта общего имущества заполняет поле Тип группы
И пользователь у этого объекта общего имущества заполняет поле Наименование "объект общего имущества тест"
И пользователь у этого объекта общего имущества заполняет поле Краткое наименование "оои1"
И пользователь у этого объекта общего имущества заполняет поле Вес "1"
И пользователь у этого объекта общего имущества заполняет поле Соответствует ЖК РФ "true"
И пользователь у этого объекта общего имущества заполняет поле Включен в программу субъекта "true"
И пользователь у этого объекта общего имущества заполняет поле Является инженерной сетью "true"
И пользователь у этого объекта общего имущества заполняет поле Множественный объект "true"
И пользователь у этого объекта общего имущества заполняет поле Является основным "true"


Сценарий: успешное добавление объекта общего имущества
Когда пользователь сохраняет этот объект общего имущества
Тогда запись по эCuba Libre, бар-ресторантому объекту общего имущества присутствует в справочнике объектов общего имущества

Сценарий: успешное удаление объекта общего имущества
Когда пользователь сохраняет этот объект общего имущества
И пользователь удаляет этот объект общего имущества
Тогда запись по этому объекту общего имущества отсутствует в справочнике объектов общего имущества

Сценарий: успешное добавление дубля объекта общего имущества
Когда пользователь сохраняет этот объект общего имущества
Дано пользователь добавляет новый объект общего имущества
И пользователь у этого объекта общего имущества заполняет поле Код "тест"
И пользователь у этого объекта общего имущества заполняет поле Тип группы
И пользователь у этого объекта общего имущества заполняет поле Наименование "объект общего имущества тест"
И пользователь у этого объекта общего имущества заполняет поле Краткое наименование "оои1"
И пользователь у этого объекта общего имущества заполняет поле Вес "1"
Когда пользователь сохраняет этот объект общего имущества
Тогда запись по этому объекту общего имущества присутствует в справочнике объектов общего имущества

Сценарий: успешное добавление группы конструктивных элементов к ООИ
Когда пользователь сохраняет этот объект общего имущества
И пользователь добавляет к этому ООИ группу конструктивных элементов
И пользователь у этой группы конструктивных элементов заполняет поле Наименование "группа конструктивных элементов тест"
И пользователь у этой группы конструктивных элементов заполняет поле Является обязательным "true"
И пользователь у этой группы конструктивных элементов заполняет поле Используется в расчете ДПКР "true"
И пользователь сохраняет эту группу конструктивных элементов
Тогда запись по этой группе конструктивных элементов присутствует в списке групп у ООИ

Структура сценария: успешное добавление характеристики группы конструктивных элементов к группе конструктивных элементов
Когда пользователь сохраняет этот объект общего имущества
И пользователь добавляет к этому ООИ группу конструктивных элементов
И пользователь у этой группы конструктивных элементов заполняет поле Наименование "группа конструктивных элементов тест"
И пользователь сохраняет эту группу конструктивных элементов
И пользователь добавляет к этой группе конструктивных элементов характеристику группы конструктивных элементов
И пользователь у этой характеристики группы конструктивных элементов заполняет поле Наименование "характеристика группы кэ"
И пользователь у этой характеристики группы конструктивных элементов заполняет поле Тип атрибута "<тип аттрибута>"
И пользователь у этой характеристики группы конструктивных элементов заполняет поле Обязательность "true"
И пользователь у этой характеристики группы конструктивных элементов заполняет поле Подсказка "Подсказка"
Тогда запись по этой группе конструктивных элементов присутствует в списке групп у ООИ

Примеры: 
| тип аттрибута |
| Целое         |
| Вещественное  |
| Строка        |
| Логическое    |