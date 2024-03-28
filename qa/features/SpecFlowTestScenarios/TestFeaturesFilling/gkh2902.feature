﻿
Функция: Привязка лицевого счета к Расчетно-кассовому центру
Учатники процесса - Роли контрагента - Расчетно кассовые центры

Сценарий: привязка лс к ркц
Когда пользователь открывает РКЦ "Муниципальное бюджетное учреждение "Никольская районная библиотека""
И пользователь в объектах ркц добавляет новый объект
И пользователь у этого объекта заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта добавляет лицевой счет "050132813"
И пользователь сохраняет этот объект
Тогда выходит сообщение с текстом "Лицевые счета сохранены успешно"
И запись по этому объекту присутствует в списке объектов у этого ркц
И запись по этому объекту отсутствует в списке объектов у ркц "Администрация Алеутского муниципального района"
И у этого ркц отображаются только лицевые счета, которые привязаны к нему

Сценарий: удаление лс из ркц
Когда пользователь открывает РКЦ "Муниципальное бюджетное учреждение "Никольская районная библиотека""
И пользователь в объектах ркц добавляет новый объект
И пользователь у этого объекта заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта добавляет лицевой счет "050132813"
И пользователь сохраняет этот объект
И пользователь удаляет этот объект
Тогда запись по этому объекту отсутствует в списке объектов у этого ркц

Сценарий: обновление списка объектов
Когда пользователь открывает РКЦ "Муниципальное бюджетное учреждение "Никольская районная библиотека""
И пользователь в объектах ркц добавляет новый объект
И пользователь у этого объекта заполняет поле Дата начала действия договора "текущая дата"
И пользователь у этого объекта заполняет поле Дата окончания действия договора "текущая дата"
И пользователь у этого объекта добавляет лицевой счет "050132813"
И пользователь сохраняет этот объект
И пользользователь открывает раздел Объекты
Тогда список с объектами автоматически обновляется




