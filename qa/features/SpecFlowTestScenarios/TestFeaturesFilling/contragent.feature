﻿
Функция: тесткейсы для раздела "Контрагенты"
Участники процесса - Контрагенты - Контрагенты


Предыстория: 
Дано добавлен жилой дом
| region    | place                                         | street             |houseNumber |
| kamchatka | Камчатский край, Алеутский р-н, с. Никольское | ул. 50 лет Октября |75          |

Дано добавлена организационно-правовая форма
| Name | Code | OkopfCode |
| тест | тест | тест      |

Дано пользователь добавляет нового контрагента
И пользователь у этого контрагента заполняет поле Наименование "тест111"
И пользователь у этого контрагента заполняет поле Организационно-правовая форма
И пользователь у этого контрагента заполняет поле ИНН "5702001741"
И пользователь у этого контрагента заполняет поле КПП "771501001"
И пользователь у этого контрагента заполняет поле Юридический адрес этим жилым домом
И пользователь у этого контрагента заполняет поле Фактический адрес этим жилым домом
И пользователь у этого контрагента заполняет поле Адрес за пределами субъекта "Адрес за пределами субъекта"
И пользователь у этого контрагента заполняет поле ОГРН "1025700517292"


Сценарий: успешное добавление контрагента c заполненными обязательными полями
Когда пользователь сохраняет этого контрагента
Тогда запись по этой форме присутствует в реестре контрагентов

Сценарий: успешное добавление контрагента с заполненным доп.полем ИНН
Когда пользователь заполняет поле ИНН "5702001741"
И пользователь сохраняет этого контрагента
Тогда запись по этой форме присутствует в реестре контрагентов
