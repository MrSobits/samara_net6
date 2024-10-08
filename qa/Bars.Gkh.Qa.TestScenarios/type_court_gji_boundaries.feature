﻿@ScenarioInTransaction
Функционал: тесткейсы граничных значений для раздела "Виды суда"
Справочники - ГЖИ - Виды суда

Сценарий: создание нового вида суда с пустыми полями Наименование
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Код "тест"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда присутствует в справочнике видов суда

Сценарий: создание нового вида суда с пустыми полями Код
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Наименование "вид суда тест"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда присутствует в справочнике видов суда

Сценарий: удачное создание нового вида суда при вводе граничных условий в 300 знаков, Наименование
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Наименование 300 символов "1"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда присутствует в справочнике видов суда

@ignore
#GKH-2542
Сценарий: неудачное создание нового вида суда при вводе граничных условий в 301 знаков, Наименование
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Наименование 301 символов "1"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда отсутствует в справочнике видов суда
И падает ошибка с текстом "Количество знаков в поле Наименование не должно превышать 300 символов"
#И количество символов в поле Наименование = 300
#И выходит предупреждение с текстом "Наименование сохранено не полностью, так как превышена максимально возможная длина в 300 символов"

Сценарий: удачное создание нового вида суда при вводе граничных условий в 300 знаков, Код
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Наименование "111tets"
И пользователь у этого вида суда заполняет поле Код 300 символов "1"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда присутствует в справочнике видов суда

@ignore
#GKH-2542
Сценарий: неудачное создание нового вида суда при вводе граничных условий в 301 знаков, Код
Дано пользователь добавляет новый вид суда
И пользователь у этого вида суда заполняет поле Наименование "111test"
И пользователь у этого вида суда заполняет поле Код 301 символов "1"
Когда пользователь сохраняет этот вид суда
Тогда запись по этому виду суда отсутствует в справочнике видов суда
И падает ошибка с текстом "Количество знаков в поле Код не должно превышать 300 символов"
#И количество символов в поле Код = 300
#И выходит предупреждение с текстом "Код сохранен не полностью, так как превышена максимально возможная длина в 300 символов"