select id as "Id дома", 
       address as "Адрес",
       case when ro.type_house = 0 then 'Не задано'
            when ro.type_house = 10 then 'Блокированной застройки'
            when ro.type_house = 20 then 'Индивидуальный'
            when ro.type_house = 30 then 'Многоквартирный'
            when ro.type_house = 40 then 'Общежитие'
            end as "Тип дома",
            ro.build_year as "Год постройки",
            ro.physical_wear as "Физический износ",
            ro.area_mkd as "Общая площадь дома",
            ro.Area_Living as "в т.ч. жилых всего",
            ro.floors as "Этажность",
            ro.maximum_floors as "Максимальная этажность",
            ro.Number_Apartments as  "Количество квартир",
            case when ro.method_form_fund = 10 then 'На счете регионального оператора'
                 when ro.method_form_fund = 20 then 'На специальном счете'
                 when ro.method_form_fund = 0 then 'Не задано' end as "Cпособ формирования фонда КР"
from gkh_reality_object ro