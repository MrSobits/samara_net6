﻿Функция: Закрытие периода
Региональный фонд

@ignore
#долго выполняется
Сценарий: создание первичного периода начислений
Когда пользователь заходит в периоды начислений
И ни одного периода еще нет
То Период автоматически создается

Сценарий: успешное закрытие периода начислений
Дано пользователь выбирает Период "2015 Февраль"
Когда пользователь вызывает операцию расчета лс
Тогда в реестре задач появилась задача с Наименованием "Расчет задолженности ЛС"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"
Когда в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи Состояние Ожидает
И у этой записи начислений есть детальная информация
И у этой детальной информации количество записей по лс = количеству лс, которые попадают в условия расчета лс
И пользователь подтверждает эту запись начислений
Тогда у этой записи Состояние Подтверждено
Когда пользователь закрывает текущий период
Тогда в реестре задач появилась задача с Наименованием "Закрытие периода"
И у этой задачи заполнено поле Дата запуска "текущая дата"
И в течении 9 мин статус задачи стал "Успешно выполнена"
И в течении 0 мин процент выполнения задачи стал "100"
И в течении 0 мин ход выполнения задачи стал "Завершено"