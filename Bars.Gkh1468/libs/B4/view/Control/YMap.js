Ext.define('B4.view.Control.YMap', {
    alternateClassName: ['B4.YMap'],
    singleton: true,

    clusterer: null,
    
    collection: [],
    
    districtCoords: null,
    
    /**
    * Events: click, doubleClick
    **/
    init: function(config) {
        var me = this;
        config = config || {};

        //#warning временный костыль
        if (!window.ymaps || !window.ymaps.regions) {
            return;
        }

        ymaps.ready(function() {
            me.map = new ymaps.Map(config.renderTo || "y-map", {
                center: [1, 1],
                zoom: 1,
                 behaviors: ['default', 'scrollZoom']
            }, {
                scrollZoomSpeed: 0,
                dblClickZoomDuration: 0
            });

            ymaps.regions.load('RU', {
                lang: 'ru',
                quality: 1
            }).then(function(result) {
                var regions = result.geoObjects; // ссылка на коллекцию GeoObjectCollection
                regions.each(function (reg) {
                    var name = reg.properties.get('name');

                    if (name == 'Татарстан') {
                        ymaps.geocode('респ. Татарстан', { results: 1 }).then(function(res) {
                            // Получаем координаты по адресу
                            var firstGeoObject = res.geoObjects.get(0);
                            var coords = firstGeoObject.geometry.getCoordinates();
                            
                            me.districtCoords = coords;
                            
                            me.showRegions();
                        });
                    }
                });
                
                result.geoObjects.events.add('click', function(e) {
                    var target = e.get('target');
                    me.ownerCt.coords = e.get('coordPosition');
                    if (me.lastActiveRegion) {
                        me.lastActiveRegion.options.set('preset', '');
                    }
                    me.lastActiveRegion = target;
                    me.lastActiveRegion.options.set('preset', {
                        strokeWidth: 3,
                        fillColor: 'F99',
                        strokeColor: 'F99'
                    });
                });
                
            }, function() {
                alert('No response');
            });
        });
    },

    showRegions: function(subjectId) {

        var me = this,
            regionData;
        
        B4.Ajax.request({
            url: B4.Url.action('getMunicipalityMap', 'Municipality')
        }).next(function (response) {
            regionData = Ext.JSON.decode(response.responseText);
            Ext.iterate(regionData.data, function (item) {
                if (item.coords) {
                    var coords = Ext.decode(item.coords.replace('\'',''));
                    var polygon = new ymaps.Polygon(coords,
                    {
                        hintContent: '<div><b>' + item.raion + '</b></div>' +
                                     '<div><span>Средний процент по МКД: </span><span>' + item.pmkd + ' % </span></div>' +
                                     '<div><span>Средний процент по ЖД: </span><span>' + item.ph + ' % </span></div>' +
                                     '<div><span>Средний процент по ОКИ:</span><span>' + item.poki + ' % </span></div>'
                    },
                    {
                        fillColor: me.getColorByDistrictPercent((item.pmkd + item.ph + item.poki) / 3), // цвет полигона
                        fillOpacity: 0.4, // прозрачность полигона
                        strokeWidth: 2, // толщина линии границы
                        strokeColor: '#5ecfef' // цвет границы
                    }
                );

                    polygon.events.add('click', function (e) {
                        e.preventDefault();
                        me.showHouses(item.id);
                    });

                    me.collection.push(polygon);
                    me.map.geoObjects.add(polygon);
                }
            });
        }).error(function () {
            // B4.QuickMsg.msg('Ошибка', 'Произошла ошибка', 'error');
        });

        if (me.districtCoords) {
            me.map.setCenter(me.districtCoords, 7);
        }

        if (me.clusterer != null) {
            me.map.geoObjects.remove(me.clusterer);
        }
        
        Ext.iterate(me.collection, function (item, key) {
            me.map.geoObjects.remove(item);
        });
        
        
    },

    showHouses: function (districtId) {
        var me = this;
        me.districtId = districtId;

        B4.Ajax.request({
            url: B4.Url.action('listRealityobjectInfo', 'Realityobject')
        }).next(function (response) {
            me.showHousesHandler(Ext.JSON.decode(response.responseText));
        }).error(function () {
            //B4.QuickMsg.msg('Ошибка', 'Произошла ошибка', 'error');
        });
    },
    
    showHousesHandler: function (houseData) {
        var me = this;
        // Границы крайних координат домов
        var result = [[999, 999], [0, 0]];

        var clusterPercent = 0;

        var houseCount = 0;

        Ext.iterate(me.collection, function (item, key) {
            me.map.geoObjects.remove(item);
        });

        Ext.iterate(houseData, function(house) {
            clusterPercent += house.percent;

            houseCount++;
        });

        Ext.ComponentQuery.query('#controlPanel')[0].show();
        Ext.ComponentQuery.query('#maportlet')[0].setTitle('Муниципалитет');
        var store = Ext.ComponentQuery.query('#pgrid')[0].getStore();
        store.getProxy().extraParams['muId'] = me.districtId;
        store.load();

        clusterPercent = clusterPercent / houseCount;
        
        // кластер объетов
        this.clusterer = new ymaps.Clusterer({
            preset: me.getColorForClusterByPercent(clusterPercent), // иконка
            showInAlphabeticalOrder: true,
            synchAdd: true,
            gridSize: 96 // размер кластеризации объектов
        });

        // Перебираем все дома
        Ext.each(houseData.data, function (house) {
            // Эту операцию лучше выполнить заранее для всех домов и сохранить в базу, т.к. геокодирование трудоемкий процесс
            // г. Казань вбил хардкодом для примера
            (function (h) {
                ymaps.geocode('респ. Татарстан, г.Казань ' + h.AddressName, { results: 1 }).then(function (res) {
                    // Получаем координаты по адресу
                    var firstGeoObject = res.geoObjects.get(0);
                    var coords = firstGeoObject.geometry.getCoordinates();

                    // Создаем объект
                    var point = new ymaps.Placemark(coords, {
                        iconContent: '',
                        balloonContent: '<div class="b-wrap">' +
                                            '<div><b>' + (h.AddressName || '') + '</b></div>' +
                                            '<div><span>Процент заполнения паспорта: </span><span> ' + (h.Percent || 0) + '% </span></div>' +
                                        '<div>'
                    }, {
                        preset: me.getColorByHousePercent(h.Percent)
                    });

                    // Обновляем границы крайних точек
                    result[0][0] = Math.min(result[0][0], coords[0]);
                    result[0][1] = Math.min(result[0][1], coords[1]);
                    result[1][0] = Math.max(result[1][0], coords[0]);
                    result[1][1] = Math.max(result[1][1], coords[1]);

                    // добавляем объект в кластер
                    me.clusterer.add(point);

                    // На основе крайних точек устанавливаем масшатаб
                    me.map.setBounds(result, {
                        checkZoomRange: true
                    });
                    
                }, function (err) {
                    alert(err.message);
                });
            })(house);
        });
        
        me.map.geoObjects.add(me.clusterer);
    },

    // Функция определения цвета по процентам района
    getColorByDistrictPercent: function (percent) {
        // цвет по умолчанию
        var color = '#fff';
        percent = parseInt(percent);

        if (percent == 100) {
            color = '#5cb85c'; // Код зеленого цвета
        }
        else if (percent > 74 && percent < 100) {
            color = '#f0ad4e'; // Код желтого цвета
        }
        else if (percent > 49 && percent < 75) {
            color = '#f9a09d'; // Код розового цвета
        }
        else if (percent > 24 && percent < 50) {
            color = '#8c5b2f'; // Код коричневого цвета
        }
        else {
            color = '#d9534f'; // Код красного цвета
        }

        return color;
    },
    
    // Функция определения цвета по процентам дома
    getColorByHousePercent: function (percent) {
        // цвет по умолчанию
        var color = 'twirl#redIcon';
        percent = parseInt(percent);

        if (percent == 100) {
            color = 'twirl#greenIcon';
        }
        else if (percent > 74 && percent < 100) {
            color = 'twirl#yellowIcon';
        }
        else if (percent > 49 && percent < 75) {
            color = 'twirl#pinkIcon';
        }
        else if (percent > 24 && percent < 50) {
            color = 'twirl#brownIcon';
        }
        else {
            color = 'twirl#redIcon';
        }

        return color;
    },
    
    // Функция определения цвета кластера по среднему арифметическому координат домов
    getColorForClusterByPercent: function (percent) {
        // цвет по умолчанию
        var color = 'twirl#blueClusterIcons';
        
        percent = parseInt(percent);

        if (percent == 100) {
            color = 'twirl#greenClusterIcons';
        }
        else if (percent > 74 && percent < 100) {
            color = 'twirl#yellowClusterIcons';
        }
        else if (percent > 49 && percent < 75) {
            color = 'twirl#brownClusterIcons';
        }
        else if (percent > 24 && percent < 50) {
            color = 'twirl#pinkClusterIcons';
        }
        else {
            color = 'twirl#redClusterIcons';
        }

        return color;
    }
});