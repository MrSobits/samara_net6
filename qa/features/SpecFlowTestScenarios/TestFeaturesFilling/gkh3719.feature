﻿
Функция: Проверка даты платежа на лс и на доме
Региональный фонд - Настройки - Реестр оплат платежных агентов
Региональный фонд - Счета - Реестр лс 

Сценарий: проверка даты платежа с реестра оплат платежных агентов
Когда пользователь производит импорт документов из банка
И в задачах появляется запись
И у этой записи заполнено поле Дата запуска "текущая дата"
И у этой записи заполнено поле Наименование "Импорт документов из банка"
И у этой записи заполнено поле Статус "Успешно выполнена"
И у этой записи заполнено поле Процент выполнения "100"
И у этой записи заполнено поле Ход выполнения "Завершено"
И в реестре оплат платежных агентов присутствует запись по оплате
И у этой записи по оплате заполнено поле Дата операции "текущая дата"
И у этой записи по оплате заполнено поле Дата сводного реестра
И у этой записи по оплате заполнено поле Номер сводного реестра 
И у этой записи по оплате заполнено поле Сумма по документу (руб.)
И у этой записи по оплате заполнено поле Код платежного агента
И у этой записи по оплате заполнено поле Наименование платежного агента
И у этой записи по оплате заполнено поле Пользователь
И у этой записи по оплате заполнено поле Статус "Загружен без предупреждений"
И у этой записи по оплате есть детальная информация
И у этой детальной информации есть запись по лицевому счету
И у этой записи по лицевому счету заполнено поле Лицевой счет ""
И у этой записи по лицевому счету заполнено поле Р/С получателя
И у этой записи по лицевому счету заполнено поле Тип оплаты
И у этой записи по лицевому счету заполнено поле Сумма
И у этой записи по лицевому счету заполнено поле Дата оплаты
И у этой записи по лицевому счету заполнено поле Номер платежа в УС агента
И у этой записи по лицевому счету заполнено поле Статус
Тогда у этого лицевого счета "" по текущему периоду есть детальная информация по лс
И у этой детальной информации по лс есть запись по операции
И у этой записи по операции заполнено поле Дата операции = Дата оплаты из детальной информации по оплате
И у этой записи по операции заполнено поле Название операции
И у этой записи по операции заполнено поле Изменение сальдо
