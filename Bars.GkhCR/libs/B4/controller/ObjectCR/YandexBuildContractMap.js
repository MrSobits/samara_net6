Ext.define('B4.controller.objectcr.YandexBuildContractMap', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.YandexMapLoader'
    ],

    mixins: {
        map: 'B4.mixins.YandexMapLoader',
        context: 'B4.mixins.Context'
    },
    
    views: [
        'objectcr.MapBuildContractPanel'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'objectcrmappanel'
        }
    ],
    
    index: function (id) {
        var me = this, args = {}, tmp,
            view = me.getMainView() || Ext.widget('objectcrmappanel');
        me.bindContext(view);
        me.application.deployView(view);
        
        B4.Ajax.request({
            url: B4.Url.action('getformap', 'BuildContract'),
            method: 'POST',
            params: { id: id }
        }).next(function (response) {
            tmp = Ext.decode(response.responseText);
            args.record = tmp.data;
            me.loadMap(args);
        });
    },

    loadMap: function (args) {
        var me = this;
        // Если нет прокси тоглобального объекта ymaps несуществует
        // соответсвенночтобы непадало лишних ошибок обязательно проверяем
        if (ymaps) {
            ymaps.ready(function () {
                if (args.record.x !== null && args.record.x !== null) {
                    var x = args.record.x;
                    var y = args.record.y;
                    var myPlacemark = new ymaps.Placemark([x, y]);
    
                    window.document.getElementById(me.getMainView().id).innerHTML = '';
                    args.myMap = new ymaps.Map(me.getMainView().id, {
                        center: [x, y],
                        zoom: 15
                    });
                    args.myMap.controls.add('typeSelector').add('zoomControl').add('mapTools').add(new ymaps.control.ScaleLine());
                    args.myMap.geoObjects.add(myPlacemark);
                } else {
                    B4.QuickMsg.msg('Информация!', 'Объект не найден на карте', 'warning');
                }
            });
        }
    }
});