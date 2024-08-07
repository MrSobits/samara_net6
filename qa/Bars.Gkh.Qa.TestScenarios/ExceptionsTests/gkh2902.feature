﻿@ScenarioInTransaction
Функция: Привязка лицевого счета к Расчетно-кассовому центру
Учатники процесса - Роли контрагента - Расчетно кассовые центры

Сценарий: привязка лс к ркц
Допустим пользователь выбирает РКЦ у которого контрагент "Муниципальное бюджетное учреждение "Никольская районная библиотека""
И пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта РКЦ добавляет лицевой счет "100000002"
Когда пользователь сохраняет этот объект РКЦ
Тогда запись по этому объекту присутствует в списке объектов у этого РКЦ
Допустим пользователь выбирает РКЦ у которого контрагент "Администрация Алеутского муниципального района"
Тогда запись по этому объекту отсутствует в списке объектов у этого РКЦ
#И у этого ркц отображаются только лицевые счета, которые привязаны к нему

Сценарий: удаление лс из ркц
Допустим пользователь выбирает РКЦ у которого контрагент "Муниципальное бюджетное учреждение "Никольская районная библиотека""
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта РКЦ добавляет лицевой счет "100000388"
Когда пользователь сохраняет этот объект РКЦ
И пользователь удаляет этот объект
Тогда запись по этому объекту отсутствует в списке объектов у этого РКЦ

@ignore 
#не проверить,обновление происходит посредством js, кот отправляет запрос. Тест должен проверять факт отправки запроса.
Сценарий: обновление списка объектов ркц
Допустим пользователь выбирает РКЦ у которого контрагент "Никольская районная библиотека""
Допустим пользователь в объектах РКЦ добавляет новый объект
И пользователь у этого объекта РКЦ заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта РКЦ заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта РКЦ добавляет лицевой счет "100000388"
Когда пользователь сохраняет этот объект РКЦ
И пользользователь открывает раздел Объекты
Тогда список с объектами автоматически обновляется