﻿@ScenarioInTransaction
Функционал: тесткейсы для раздела "Этапы работ"
Справочники - Капитальный ремонт - Этапы работ


Предыстория: 
Дано пользователь добавляет новый этап работы
И пользователь у этого этапа работы заполняет поле Наименование "этап работы тест"
И пользователь у этого этапа работы заполняет поле Код "тест"

Сценарий: успешное добавление этапа работы
Когда пользователь сохраняет этот этап работы
Тогда запись по этому этапу работы присутствует в разделе этапов работ

Сценарий: успешное удаление этапа работы
Когда пользователь сохраняет этот этап работы
И пользователь удаляет этот этап работы
Тогда запись по этому этапу работы отсутствует в разделе этапов работ

Сценарий: успешное добавление дубля этапа работы
Когда пользователь сохраняет этот этап работы
Дано пользователь добавляет новый этап работы
И пользователь у этого этапа работы заполняет поле Наименование "этап работы тест"
И пользователь у этого этапа работы заполняет поле Код "тест"
Когда пользователь сохраняет этот этап работы
Тогда запись по этому этапу работы присутствует в разделе этапов работ