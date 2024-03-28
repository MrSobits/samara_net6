/**
* @mixin Применяется если нужно отображать яндекс карту
Можно открывать как в модальном окне так и в отдельном табе
*
*/
Ext.define('B4.mixins.YandexMapLoader', {
    
    defaultContainerSelector: '#contentPanel',
    
    strExceptions: [
        'край. ',
        'ненецкий ао, '
    ],

    // Метод открытия модального окна с картой. asp - ссылка на аспект в котором используется
    showMapWindow: function (asp) {
        if (asp.mapWindowSelector) {
            var mapWindow = Ext.ComponentQuery.query(asp.mapWindowSelector)[0];

            if (!mapWindow) {

                mapWindow = asp.controller.getView(asp.mapWindowView).create(
                    {
                        constrain: true,
                        autoDestroy: true,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    }
                );

                mapWindow.show();
                asp.fireEvent('rendermap', asp);
            }

            return mapWindow;
        }
    },
    
    // Загрузка карты в компонент. args - аргумент содержащий record. В случае модального окна лучше передавать как ссылку на аспект.
    loadMap: function (args) {
        var me = this;
        // Если нет прокси, то глобального объекта ymaps не существует.
        // Соответственно, чтобы не падали лишние ошибки, обязательно проверяем
        if (ymaps) {

            ymaps.ready(function () {
                ymaps.geocode(me.normalizeQuery(args.record.SearchAddress), { results: 1 })
                .then(function (res) {
                    var firstGeoObject = res.geoObjects.get(0);
                    if (firstGeoObject) {
                        var bounds = firstGeoObject.properties.get('boundedBy');

                        // Вычисляем координаты
                        var leftX = bounds[0][0], leftY = bounds[0][1];
                        var rightX = bounds[1][0], rightY = bounds[1][1];

                        var resPosX = ((leftX + rightX) / 2).toFixed(6), resPosY = ((leftY + rightY) / 2).toFixed(6);

                        // Создаем метки
                        var myPlacemark = new ymaps.Placemark([resPosX, resPosY],
                        {
                            address: args.record.FullAddress,
                            typeHouse: B4.enums.TypeHouse.displayRenderer(args.record.TypeHouse),
                            stateHouse: B4.enums.ConditionHouse.displayRenderer(args.record.ConditionHouse),
                            yearExplot: args.record.DateCommissioning != null ? new Date(args.record.DateCommissioning).getFullYear() : "",
                            yearCapRepair: args.record.DateLastOverhaul != null ? new Date(args.record.DateLastOverhaul).getFullYear() : "",
                            floor: args.record.Floors,
                            squareMkd: args.record.AreaMkd,
                            gatesCount: args.record.NumberEntrances,
                            apartamentsCount: args.record.NumberApartments,
                            peoplesCount: args.record.NumberLiving,
                            manOrg: args.record.ManOrgNames
                        });

                        // Создаем шаблон для отображения контента балуна
                        var myBalloonLayout = ymaps.templateLayoutFactory.createClass(
                            '<h3><b>$[properties.address]</b></h3>' +
                            '<p><strong>Тип дома:</strong> <b>$[properties.typeHouse]</b></p>' +
                            '<p><strong>Состояние дома:</strong> <b>$[properties.stateHouse]</b></p>' +
                            '<p><strong>Год сдачи в эксплуатацию:</strong> <b>$[properties.yearExplot]</b></p>' +
                            '<p><strong>Год последнего кап ремонта:</strong> <b>$[properties.yearCapRepair]</b></p>' +
                            '<p><strong>Этажность:</strong> <b>$[properties.floor]</b></p>' +
                            '<p><strong>Общая площадь МКД:</strong> <b>$[properties.squareMkd]</b></p>' +
                            '<p><strong>Количество подъездов:</strong> <b>$[properties.gatesCount]</b></p>' +
                            '<p><strong>Количество квартир:</strong> <b>$[properties.apartamentsCount]</b></p>' +
                            '<p><strong>Количество проживающих:</strong> <b>$[properties.peoplesCount]</b></p>' +
                            '<p><strong>Управляющая организация:</strong> <b>$[properties.manOrg]</b></p>'
                        );

                        // Помещаем созданный шаблон в хранилище шаблонов. Теперь наш шаблон доступен по ключу 'my#superlayout'.
                        ymaps.layout.storage.add('my#templateBaloon', myBalloonLayout);

                        // Задаем наш шаблон для балуна
                        myPlacemark.options.set({
                            balloonContentBodyLayout: 'my#templateBaloon',
                            balloonContentLayout: 'my#templateBaloon',
                            // Максимальная ширина балуна в пикселах
                            balloonMaxWidth: 450
                        });

                        window.document.getElementById(me.getMainView().domId).innerHTML = '';

                        args.myMap = new ymaps.Map(me.getMainView().domId, {
                            center: firstGeoObject ? [resPosX, resPosY] : [55, 38],
                            zoom: 15,
                            controls: []
                        });

                        args.myMap.controls
                            // Список типов карты
                            .add('typeSelector')
                            // Ползунок изменения масштаба
                            .add('zoomControl')
                            .add('rulerControl', {
                            // Масштабная линейка
                                scaleLine: true
                            });

                        args.myMap.geoObjects.add(myPlacemark);

                    } else {
                        B4.QuickMsg.msg('Информация!', 'Дом не найден на карте', 'warning');
                        window.document.getElementById('yaMapDiv').innerHTML = '';
                    }
                });
            });
        }
    },
    
    // Перерисовка карты. Подразумевается что в loadMap передали ссылку на аспект, у которого после этого появилась myMap.
    // Метод используется только в модальном окне
    redrawMap: function () {
        if (arguments[5].asp.myMap) {
            arguments[5].asp.myMap.container.fitToViewport();
        }
    },

    normalizeQuery: function (str) {
        var me = this,
            result = str;
        
        Ext.Array.each(me.strExceptions, function (el) {
            if (str && str.toLowerCase().indexOf(el) == 0) {
                result = str.substr(el.length);
            }
        });

        // Убирает буквы имени и отчества в названии именной улицы.
        // Например, ул. имени С.Н.Калмыкова - так Yandex не находит, а если убрать С.Н. - то всё ок))
        result = result.replace(/\S\.\S\./g, '');
        return result;
    }
});