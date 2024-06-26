﻿@ScenarioInTransaction
Функция: установка и изменение сальдо

#делать в транзакции
Сценарий:  успешная смена сальдо
Дано пользователь выбирает Период "2015 Февраль"
И пользователь в реестре ЛС выбирает лицевой счет "100000390"
Допустим пользователь для текущего ЛС вызывает операцию Установка и изменение сальдо
И пользователь в форме установка и изменение сальдо заполняет поле Новое значение "1000"
И пользователь в форме установка и изменение сальдо заполняет поле Причина установки/изменения сальдо "тест сальдо"
Когда пользователь сохраняет изменения в форме установка и изменение сальдо
Тогда у этого лицевого счета в истории изменений присутствует запись 
И у этой записи, в истории изменений ЛС, Наименование параметра "Установка и изменение сальдо"
И у этой записи, в истории изменений ЛС, Описание измененного атрибута "Ручное изменение сальдо с 3027.08 на 1000" 
И у этой записи, в истории изменений ЛС, Значение "1000"
И у этой записи, в истории изменений ЛС, Дата начала действия значения "текущая дата" 
И у этой записи, в истории изменений ЛС, Дата установки значения "текущая дата"
И у этой записи, в истории изменений ЛС, Причина "тест сальдо"
И в карточке лс появляется запись по операции по текущему периоду
И у этой записи по операции по текущему периоду есть детальная информация
И у этой детальной информации есть запись "Установка/изменение сальдо"
#И у этой записи по пени в операциях заполнено поле Дата операции "текущая дата"
И у этой записи по пени заполнено поле Изменение сальдо "-2027,0800000000"
Тогда у поля Начислено взносов по минимальному тарифу всего есть детальная информация по начисленным взносам
#И у этой детальной информации по начисленным взносам есть запись по текущему периоду
И у этой записи по текущему периоду заполнено поле Сумма "-2027,08000"
#И в карточке лс заполнено поле Задолженность по взносам всего "1000"
И у поля Задолженность по взносам всего есть детальная информация по задолженности по взносам
#И у этой детальной информации по задолженности по взносам есть запись по текущему периоду
И у этой записи по текущему периоду заполнено поле Сумма "-2027,08000"

@ignore
#не отловить js
Сценарий:  неудачная смена сальдо без выбора ЛС
Когда пользователь вызывает операцию Установка и изменение сальдо без ваыбора лс
Тогда выходит сообщение об ошибке с текстом "Необходимо выбрать один лицевой счет!"

@ignore
#не отловить js
Сценарий:  неудачная сальдо сальдо для одновременно двух ЛС
Когда пользователь для ЛС "100000007" и "100000008" вызывает операцию Установка и изменение сальдо без выбора лс
Тогда падает ошибка с текстом "Необходимо выбрать один лицевой счет!"

Сценарий:  неудачная смена сальдо для закрытого ЛС
Дано пользователь выбирает Период "2015 Февраль"
И пользователь в реестре ЛС выбирает лицевой счет "100000390"
Допустим пользователь для текущего ЛС вызывает операцию Установка и изменение сальдо
И пользователь в форме установка и изменение сальдо заполняет поле Новое значение "1000"
И пользователь в форме установка и изменение сальдо заполняет поле Причина установки/изменения сальдо "тест сальдо"
Когда пользователь сохраняет изменения в форме установка и изменение сальдо
Тогда падает ошибка с текстом "Для изменения пени счет не должен быть закрыт"