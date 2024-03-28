﻿
Функционал: тесткейсы граничных значений для раздела "Решения суда"
Справочники - ГЖИ - Решения суда


Сценарий: создание нового решения суда с пустыми полями
Дано пользователь добавляет новое решение суда
Когда пользователь сохраняет это решение суда
Тогда запись по этому решению суда присутствует в справочнике решений суда

Сценарий: удачное создание нового решения суда при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новое решение суда
И пользователь у этого решения суда заполняет поле Наименование 300 символов "1"
Когда пользователь сохраняет это решение суда
Тогда запись по этому решению суда присутствует в справочнике решений суда

Сценарий: неудачное создание нового решения суда при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новое решение суда
И пользователь у этого решения суда заполняет поле Наименование 301 символов "1"
Когда пользователь сохраняет это решение суда
Тогда запись по этому решению суда отсутствует в справочнике решений суда
И падает ошибка с текстом "Количество знаков в поле Наименование не должно превышать 300 символов"
#И количество символов в поле Наименование = 300
#И выходит предупреждение с текстом "Наименование сохранено не полностью, так как превышена максимально возможная длина в 300 символов"

Сценарий: удачное создание нового решения суда при вводе граничных условий в 300 знаков, Код
Дано пользователь добавляет новое решение суда
И пользователь у этого решения суда заполняет поле Код 300 символов "1"
Когда пользователь сохраняет это решение суда
Тогда запись по этому решению суда присутствует в справочнике решений суда

Сценарий: неудачное создание нового решения суда при вводе граничных условий в 301 знаков, Код
Дано пользователь добавляет новое решение суда
И пользователь у этого решения суда заполняет поле Код 301 символов "1"
Когда пользователь сохраняет это решение суда
Тогда запись по этому решению суда отсутствует в справочнике решений суда
И падает ошибка с текстом "Количество знаков в поле Код не должно превышать 300 символов"
#И количество символов в поле Код = 300
#И выходит предупреждение с текстом "Код сохранен не полностью, так как превышена максимально возможная длина в 300 символов"