﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Типы домов"
Справочники - Общие - Типы домов

Предыстория: 
Дано добавлена единица измерения
| Name | ShortName | Description |
| тест | тест      | тест        |

Дано пользователь добавляет Тип группы ООИ с Наименованием "тестовый тип группы ООИ" и Кодом "123"
И пользователь добавляет объект общего имущества с Наименованием "тестовый объект общего имущества" и Кодом "1233"
И пользователь добавляет группу конструктивных элементов "тестовая группа конструктивных элементов"
И пользователь добавляет конструктивный элемент "тестовый конструктивный элемент"

Дано пользователь добавляет новый Тип дома
И пользователь у этого Типа дома заполняет поле Наименование "тип дома тестовый 2"
И пользователь у этого Типа дома заполняет поле Код "123"
И пользователь у этого Типа дома заполняет поле Предельная стоимость ремонта "123"

Сценарий: успешное добавление Типа дома
Когда пользователь сохраняет этот Тип дома
Тогда запись по этому Типу дома присутствует в справочнике Типов домов

@ignore
#GKH-2375
Сценарий: успешное удаление записи из справочника Типы домов
Когда пользователь сохраняет этот Тип дома
Тогда запись по этому Типу дома присутствует в справочнике Типов домов
И пользователь удаляет этот Тип дома
Тогда запись по этому Типу дома отсутствует в справочнике Типов домов

Сценарий: успешное добавление Общей характеристики типа дома
Когда пользователь сохраняет этот Тип дома
Дано пользователь добавляет Общую характеристику типа жилых домов 
И заполняет у этого Общего параметра поле Наименование "тест общая характер"
И заполняет у этого Общего параметра поле Минимальное значение "12312"
И заполняет у этого Общего параметра поле Максимальное значение "12312"
Когда пользователь сохраняет Общую характеристику типа жилых домов
Тогда запись по этой Общей характеристике присутствует в Типе дома

Сценарий: успешное удаление Общей характеристики типа дома
Когда пользователь сохраняет этот Тип дома
Дано пользователь добавляет Общую характеристику типа жилых домов 
И заполняет у этого Общего параметра поле Наименование "123ntcn"
И заполняет у этого Общего параметра поле Минимальное значение "123"
И заполняет у этого Общего параметра поле Максимальное значение "123"
Когда пользователь сохраняет Общую характеристику типа жилых домов
Когда пользователь удаляет Общую характеристику типа жилых домов
Тогда запись по этой Общей характеристике отсутствует в Типе дома

Сценарий: успешное добавление дубля в справочник Типы домов
Когда пользователь сохраняет этот Тип дома
Дано пользователь добавляет новый Тип дома
И пользователь у этого Типа дома заполняет поле Наименование "тип дома тестовый 1111"
И пользователь у этого Типа дома заполняет поле Код "123"
Когда пользователь сохраняет этот Тип дома
Тогда запись по этому Типу дома присутствует в справочнике Типов домов

Сценарий: успешное добавление Конструктивного элемента типа дома
Когда пользователь сохраняет этот Тип дома
Дано пользователь добавляет Конструктивный элемент типа жилых домов
И заполняет поле КЭ присутствует отсутствует в доме "false" 
Когда пользователь сохраняет Конструктивный элемент типа жилых домов
Тогда запись по этому Конструктивному элементу присутствует в Типе дома

Сценарий: успешное удаление Конструктивного элемента типа дома
Когда пользователь сохраняет этот Тип дома
Дано пользователь добавляет Конструктивный элемент типа жилых домов
И заполняет поле КЭ присутствует отсутствует в доме "false" 
Когда пользователь сохраняет Конструктивный элемент типа жилых домов
Когда пользователь удаляет Конструктивный элемент типа жилых домов
Тогда запись по этому Конструктивному элементу отсутствует в Типе дома

@ignore
#GKH-2955
Сценарий: успешное добавление муниципальных образований к типу дома
Дано добавлены муниципальные образования
| Name | RegionName |
| 1    | 1          |
| 2    | 2          |
| 3    | 3          |

Когда пользователь сохраняет этот тип дома
И добавляет к типу муниципальные образования
Когда пользователь сохраняет эти муниципальные образования
Тогда запись по этим муниципальным образованиям присутствует в этом типе дома