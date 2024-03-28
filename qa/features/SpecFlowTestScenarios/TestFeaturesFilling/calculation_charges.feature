﻿
Функция: Расчет начислений

#должны быть созданы лс: 1й с нулевой долей собственности, 2й с нулевой общей площадью помещения
Сценарий: Расчет нулевой доли собственности для лс при нулевой доли собственности или площади помещения
Дано пользователь вызывает операцию расчета лс
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
Когда пользователь подтверждает эту запись начислений
Тогда у этой записи начислений Состояние "Подтверждено"
И в протоколе расчета по текущему периоду у ЛС "" есть запись по начислению
И у этой записи для ЛС "" заполнено поле С-по "дата открытия периода начислений"
И у этой записи для ЛС "" заполнено поле Тариф на КР ""
И у этой записи для ЛС "" заполнено поле Доля собственности ""
И у этой записи для ЛС "" заполнено поле Площадь помещения ""
И у этой записи для ЛС "" заполнено поле Количество дней ""
И у этой записи для ЛС "" заполнено поле Итого ""

И в протоколе расчета по текущему периоду у ЛС "" есть запись по начислению
И у этой записи для ЛС "" заполнено поле С-по "дата открытия периода начислений"
И у этой записи для ЛС "" заполнено поле Тариф на КР ""
И у этой записи для ЛС "" заполнено поле Доля собственности ""
И у этой записи для ЛС "" заполнено поле Площадь помещения ""
И у этой записи для ЛС "" заполнено поле Количество дней ""
И у этой записи для ЛС "" заполнено поле Итого ""

#должен быть создан лс с нулевым тарифом
Сценарий: Расчет нулевой доли собственности для лс при нулевом тарифе
Дано в размере взносов на кр есть запись показателя
И у этой записи показателя заполнено поле Окончание периода ""
И у этой записи показателя есть детальная информация
И у этой детальной информации есть запись по муниципальному образованию "Никольское сельское поселение"
И у этогй записи по муниципальному образованию заполнено поле Размер взноса "7,8"
И пользователь вызывает операцию расчета лс
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
Когда пользователь подтверждает эту запись начислений
Тогда у этой записи начислений Состояние "Подтверждено"
И в протоколе расчета по текущему периоду у ЛС "" есть запись по начислению
И у этой записи для ЛС "" заполнено поле С-по "дата открытия периода начислений"
И у этой записи для ЛС "" заполнено поле Тариф на КР ""
И у этой записи для ЛС "" заполнено поле Доля собственности ""
И у этой записи для ЛС "" заполнено поле Площадь помещения ""
И у этой записи для ЛС "" заполнено поле Количество дней ""
И у этой записи для ЛС "" заполнено поле Итого ""
И И у этогй записи по муниципальному образованию заполнено поле Размер взноса "0,00"

#должны быть созданы лс: 1й со статусом "Закрыт", 2й со статусом "Не активен"
Структура сценария: отсутствие расчета начислений для лс со статусами "Закрыт" и "Не активен"
Дано пользователь вызывает операцию расчета лс
И в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений Состояние "Ожидание"
Когда пользователь подтверждает эту запись начислений
Тогда у этой записи начислений Состояние "Подтверждено"
И у этой записи начислений есть детальная информация по начислениям
И у этой детальной информации по начислениям отсутствует запись по ЛС "<лс>"
И в протоколе расчета по текущему периоду у ЛС "<лс>" отсутствует запись по начислению

Примеры:
| лс |
|    |
|    |

#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
Сценарий: отсутствие расчета начислений при отсутствии настройки в Настройке параметров
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "false"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "false"
И пользователь в единых настройках приложения заполняет поле Специальный счет "false"
И пользователь в единых настройках приложения заполняет поле Не выбран "false" 
Когда пользователь вызывает операцию расчета лс
Тогда в задачах не появляется запись по расчету лс

#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
Структура сценария: расчет начислений при заполненной настройке в Настройке параметров со всеми включенными параметрами
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 

Примеры:
| лс        |
| 140014331 |
|           |
|           |
|           |

#Администрирование/Настройки приложения/Единые настройки приложения - групбокс "Расчет вести для домов, у которых способ формирования фонда"
#должны быть домавлены 4 лс, у которых дома с протоколами в конечном статусе со способами формирования Счет регионального оператора, Специальный счет регионального оператора, Специальный счет, Не выбран 
Структура сценария: расчет начислений при заполненной настройке в Настройке параметров по одному параметру
Дано пользователь в единых настройках приложения заполняет поле "<способ формирования фонда>" "true"
Когда пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в задачах появляется запись по расчету лс
И у этой записи по расчету лс заполнено поле Дата запуска "текущая дата"
И у этой записи по расчету лс заполнено поле Наименование "Расчет задолжности ЛС"
И у этой записи по расчету лс заполнено поле Статус "Успешно выполнена"
И у этой записи по расчету лс заполнено поле Процент выполнения "100"
И у этой записи по расчету лс заполнено поле Ход выполнения "Завершено"
И в Реестре неподтвержденных начислений появляется запись начислений 
И у этой записи начислений есть детальная информация по начислениям
И у этой детальной информации по начислениям присутствует запись по ЛС "<лс>"

Примеры:
| способ формирования фонда                | лс |
| Счет регионального оператора             |    |
| Специальный счет регионального оператора |    |
| Специальный счет                         |    |
| Не выбран                                |    |

#должны быть добавлены 2 дома: 1 с сотоянием Аварийный, 2 с состоянием Снесен, и лицевые счета к ним
Структура сценария: отсутствие расчета начислений для лс, у которых дома с состояниями Аварийный и Снесен
Когда пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в задачах не появляется запись по расчету ЛС "<лс>"

Примеры:
| лс |
|    |
|    |

#должны быть 4 дома с протоколами, у которых статус не переведен в конечный
Структура сценария: неудачный расчет начислений при заполненной настройке с протоколом НЕ в конечном статусе
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в задачах не появляется запись по расчету лс

Примеры:
| лс |
|    |
|    |
|    |
|    |

#должны быть 4 дома с протоколами, у которых статус переведен в конечный, но даты протоколов 
Структура сценария: неудачный расчет начислений при заполненной настройке с протоколом в конечном статусе, но с датой вступления в силу > текущей даты
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС "<лс>"
Тогда в задачах не появляется запись по расчету лс

Примеры:
| лс |
|    |
|    |
|    |
|    |

#должны быть 1 дом с протоколам, у которого статус переведен в конечный, дата актуальная, а ведение лс = собственниками
Сценарий: неудачный расчет начислений при заполненной настройке с протоколом, в котором ведение лс = Собственниками
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС ""
Тогда в задачах не появляется запись по расчету лс

#должны быть 1 дом с протоколам, 1 ркц, в этот ркц добавлен лс от этого дома, и проставлена галочка в CheckBox "РКЦ проводит начисления"
Сценарий: неудачный расчет начислений для лс, добавленных в ркц с "РКЦ проводит начисления" = true
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС ""
Тогда в задачах не появляется запись по расчету лс

#должны быть 1 дом с протоколам, и у этого дома в CheckBox "Дом не участвует в программе КР" стоит галочка
Сценарий: неудачный расчет начислений для лс, не участвующих а программе КР
Дано пользователь в единых настройках приложения заполняет поле Счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет регионального оператора "true"
И пользователь в единых настройках приложения заполняет поле Специальный счет "true"
И пользователь в единых настройках приложения заполняет поле Не выбран "true" 
Когда пользователь вызывает операцию расчета лс для ЛС ""
Тогда в задачах не появляется запись по расчету лс





