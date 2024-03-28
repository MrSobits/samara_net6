Ext.define('B4.controller.Lifts', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    views: ['lifts.ViewPanel'],

    mainView: 'lifts.ViewPanel',
    mainViewSelector: '#disinfoliftspanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    init: function () {
        var me = this;

        me.control({
            'disinfoliftspanel b4updatebutton': {
                click: { fn: me.updateGrid, scope: me }
            }
        });

        me.callParent(arguments);
    },
    
    onLaunch: function () {
        this.updateGrid();
    },

    updateGrid: function () {
        var me = this,
            mainView = me.getMainView();

        me.mask('Загрузка', mainView);
        if (me.params) {
            B4.Ajax.request({
                url: B4.Url.action('GetRealtyObjectLifts', 'DisclosureInfoRealityObj'),
                params: {
                    diRoId: me.params.disclosureInfoRealityObjId
                },
                timeout: 9999999
            }).next(function (response) {
                var obj;

                try {
                    obj = Ext.decode(response.responseText);
                } catch (e) {
                    obj = {};
                }

                mainView.down('#disinfoliftgrid').store.loadData(obj);
                me.unmask();
            }).error(function (e) {
                Ext.msg.alert('Ошибка', e.message || 'При обработке запроса произошла ошибка');
                me.unmask();
            });
        }
    }

});