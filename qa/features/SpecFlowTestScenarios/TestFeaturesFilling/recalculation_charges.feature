﻿
Функция: Перерасчет начислений

#сделать в  транзакции
Сценарий: успешный перерасчет начислений при изменении площади помещения
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
И пользователь вызывает операцию расчета лс для ЛС "010114939"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114939"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
И у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "347,80"
И в реестре жилых домов есть запись по дому с Адресом "Камчатский край, г. Петропавловск-Камчатский, ш. Петропавловское, д. 31"
И у этого дома есть запись в Сведениях о помещениях
И у этой записи в Сведениях о помещениях заполнено поле № квартиры/помещения "9"
Когда пользователь у этого помещения заполняет поле Общая площадь "100"
И пользователь вызывает операцию расчета лс для ЛС "010114939"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114939"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
Тогда у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "740"

#сделать в  транзакции
Сценарий: успешный перерасчет начислений при изменении доли собственности
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
И пользователь вызывает операцию расчета лс для ЛС "010114948"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114948"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
И у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "270,84"
И в реестре абонентов есть запись по абоненту "НАДТОЧИЙ ОКСАНА ЭДУАРДОВНА"
И у этой записи по абоненту есть запись о помещении с лс "010114948"
Когда пользователь у этой записи о помещении у доли собственности заполняет поле Дата вступления в силу "текущая дата"
И пользователь у этой записи о помещении у доли собственности заполняет поле Новое значение "0,5"
И пользователь вызывает операцию расчета лс для ЛС "010114948"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114948"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
Тогда у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "135,42"

#сделать в  транзакции
Сценарий: успешный перерасчет начислений при изменении тарифа
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
И пользователь вызывает операцию расчета лс для ЛС "010114951"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114951"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
И у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "341,14"
И в размере взносов на кр есть запись показателя
И у этой записи показателя заполнено поле Окончание периода "28.08.2015"
И у этой записи показателя есть детальная информация
И у этой детальной информации есть запись по муниципальному образованию "Петропавловск-Камчатский городской округ"
Когда пользователь у этогй записи по муниципальному образованию заполняет поле Размер взноса "8"
И пользователь вызывает операцию расчета лс для ЛС "010114951"
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
И пользователь подтверждает эту запись начислений
И у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация
И у этой детальной информации есть запись по начислению для этого ЛС
И у этой записи по начислению для этого ЛС заполнено поле Счет "010114951"
И у этой записи по начислению для этого ЛС заполнено поле Статус "Открыт"
Тогда у этой записи по начислению для этого ЛС заполнено поле Начислено по базовому тарифу "368,80"
