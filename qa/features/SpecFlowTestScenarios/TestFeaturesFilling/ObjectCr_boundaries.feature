﻿
Функция: тесткейсы граничных значений для раздела "Объекты капитального ремонта"
Капитальный ремонт - Программы капитального ремонта - Объекты капитального ремонта


Сценарий: неудачное добавление объекта капитального ремонта при незаполненных обязательных полях
Дано пользователь добавляет новый объект капитального ремонта
Когда пользователь сохраняет этот объект капитального ремонта
Тогда запись по этому объекту капитального ремонта отсутствует в разделе объектов капитального ремонта
И падает ошибка с текстом "Не заполнены обязательные поля: Наименование Программа КР"