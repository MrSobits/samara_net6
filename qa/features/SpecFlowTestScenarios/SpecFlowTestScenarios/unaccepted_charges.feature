﻿
Функционал: проверки по Реестру неподтвержденных начислений
#у каждого сценария данной фичи есть ID (например, "un_chs-1", где un_chs - название фичи, 1 - номер сценария )


#un_chs-1
#для всех регионов, на примере Камчатки
Структура сценария: Проверка наличия аттрибутов в реестре неподтвержденных начислений
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Когда пользователь заходит в реестр неподтвержденных начислений
То в реестре имеются аттрибуты <аттрибуты>

Примеры: 
| аттрибуты                                         |
| общее количество лс, по которым произведен расчет |
| общее количество лс, по которым есть перерасчет   |
| общее количество лс, по которым есть пени         |
| итого начислено                                   |
| итого перерасчет                                  |
| итого пени                                        |
| примечание                                        |

#un_chs-2
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "общее количество лс, по которым произведен расчет"
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"

#un_chs-3
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "общее количество лс, по которым есть перерасчет"
Дано Пользователь логин "admin", пароль "admin"
И система "http://gkh-test.bars-open.ru/dev-kamchatka"
Дано пользователь находится в реестре неподтвержденных начислений
Если в реестре есть аттрибут "общее количество лс, по которым есть перерасчет"
То по каждому неподтвержденному начислению значение аттрибута равно: считается количество ЛС, у которых признак операции перерасчета = 0

#un_chs-4
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "общее количество лс, по которым есть пени" в реестре
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Дано пользователь находится в реестре неподтвержденных начислений
Если в реестре есть аттрибут "общее количество лс, по которым есть пени"
То по каждому неподтвержденному начислению значение аттрибута равно: считается количество ЛС, у которых сумма пени не равна 0

#un_chs-5
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "итого начислено" в реестре
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"

#un_chs-6
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "итого перерасчет" в реестре
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"

#un_chs-7
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "итого пени" в реестре
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"

#un_chs-8
#для всех регионов, на примере Камчатки
Сценарий: проверка расчета значения в поле "примечание" в реестре
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"

#un_chs-9
#для всех регионов, на примере Камчатки
Структура сценария: проверка наличия аттрибутов в карточке неподтвержденного начисления
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Когда пользователь заходит в карточку неподтвержденного начисления
То в карточке имеются аттрибуты <аттрибуты>

Примеры: 
| аттрибуты                                         |
| общее количество лс, по которым произведен расчет |
| общее количество лс, по которым есть перерасчет   |
| общее количество лс, по которым есть пени         |
| Номер лицевого счета                              |
| Номер расчетного счета                            |
| Муниципальный район                               |
| Муниципальное образование                         |
| Населенный пункт                                  |
| Улица, дом, квартира                              |
| Протокол расчета                                  |


#un_chs-10
#для всех регионов, на примере Камчатки
Структура сценария: проверка расчета значений по полям общего количества лс
Дано рассчитывается аналогично сценариям un_chs-2, un_chs-3, un_chs-4
Когда пользователь вызывает действие по аттрибуту <аттрибут>
То записи в гриде карточки неподтвержденного начисления соответственно отфильтровываются 

Примеры: 
| аттрибут                                          |
| общее количество лс, по которым произведен расчет |
| общее количество лс, по которым есть перерасчет   |
| общее количество лс, по которым есть пени         |


#un_chs-11
#для всех регионов, на примере Камчатки
Сценарий: проверка перехода из карточки неподтвержденного начисления в протокол расчета
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Когда пользователь находится в карточке неподтвержденного начисления и переходит по конкретному лс в протокол расчета
То пользователю доступна информация из протокола расчета лс

#un_chs-12
#для всех регионов, на примере Камчатки
Сценарий: проверка возможности подтверждения неподтвержденного начисления из карточки
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Дано пользователь находится в карточке неподтвержденного
Когда пользователь подтверждает начисление
То отрабатывает функция подтверждения по аналогии с реестром неподтвержденных начислений
И подтверждается только текущее начисление

#un_chs-13
#для всех регионов, на примере Камчатки
Структура сценария: фильтрация в карточке неподтвержденного начисления
Дано Пользователь логин "admin", пароль "admin"
И тестируемая система "http://gkh-test.bars-open.ru/dev-kamchatka"
Дано пользователь находится в карточке неподтвержденного
Когда пользователь использует фильтр аттрибута <аттрибут>
То доступны для использования виды фильтрации <виды_фильтрации>

Примеры: 
| аттрибут                  | виды_фильтрации |
| Номер лицевого счета      | >               |
| Номер лицевого счета      | <               |
| Номер лицевого счета      | !=              |
| Номер лицевого счета      | >=              |
| Номер лицевого счета      | <=              |
| Номер лицевого счета      | с ... по ...    |
| Номер расчетного счета    | >               |
| Номер расчетного счета    | <               |
| Номер расчетного счета    | !=              |
| Номер расчетного счета    | >=              |
| Номер расчетного счета    | <=              |
| Номер расчетного счета    | с ... по ...    |
| Муниципальный район       | содержит        |
| Муниципальный район       | не содержит     |
| Муниципальное образование | содержит        |
| Муниципальное образование | не содержит     |
| Населенный пункт          | содержит        |
| Населенный пункт          | не содержит     |
| Улица, дом, квартира      | содержит        |
| Улица, дом, квартира      | не содержит     |



