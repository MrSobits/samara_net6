Ext.define('B4.controller.HouseManaging', {
    extend: 'B4.base.Controller',
    views: ['housemanaging.ViewPanel'],

    requires: [
        'B4.Ajax',
        'B4.Url'
    ],

    models: ['HouseManaging', 'RealityObject'],

    mainView: 'housemanaging.ViewPanel',
    mainViewSelector: 'housemanagingviewpanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this;

        me.control({
            'housemanagingviewpanel b4updatebutton': {
                click: { fn: me.updateInfo, scope: me }
            }
        });

        me.callParent(arguments);
    },

    onLaunch: function () {
        this.updateInfo();
    },

    updateInfo: function () {
        var me = this,
            model = me.getModel('HouseManaging'),
            mainView = me.getMainView();

        me.mask('Загрузка', mainView);
        if (me.params) {
            B4.Ajax.request({
                url: B4.Url.action('GetRealtyObjectHouseManaging', 'DisclosureInfoRealityObj'),
                params: {
                    diRoId: me.params.disclosureInfoRealityObjId,
                    diManorgId: me.params.disclosureInfoId
                },
                timeout:  1000 * 60 // 1 минута
            }).next(function (response) {
                var obj, record;

                try {
                    obj = Ext.decode(response.responseText);
                } catch (e) {
                    obj = {};
                }

                record = new model(obj);
                mainView.loadRecord(record);
                me.unmask();
            }).error(function (e) {
                Ext.msg.alert('Ошибка', e.message || 'При обработке запроса произошла ошибка');
                me.unmask();
            });
        }
    }
});