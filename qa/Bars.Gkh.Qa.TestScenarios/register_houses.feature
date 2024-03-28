﻿@ScenarioInTransaction
Функция: реестр жилых домов
Жилищный фонд - Объекты жилищного фонда - Реестр жилых домов

Предыстория: 
Дано пользователь добавляет новый дом в Реестр жилых домов
И у этого дома устанавливает поле Населённый пункт
| region     | place                                         |
| testregion | Камчатский край, Алеутский р-н, с. Никольское |

И у этого дома устанавливает поле Улица
| region     | street             |
| testregion | ул. 50 лет Октября |

И у этого дома устанавливает поле Номер Дома
| region     | houseNumber |
| testregion | test        |

Сценарий: создание жилого дома
Когда пользователь сохраняет этот жилой дом
Тогда запись по этому дому присутствует в реестр жилых домов

Сценарий: удаление жилого дома
Когда пользователь сохраняет этот жилой дом
И пользователь удаляет этот жилой дом
Тогда запись по этому дому отсутствует в реестре жилых домов