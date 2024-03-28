Ext.define('B4.controller.StructElements', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: ['StructElements'],

    views: ['structelements.ViewPanel'],

    mainView: 'structelements.ViewPanel',
    mainViewSelector: '#disinfostructelementspanel',

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'passportstatepermission'
        }
    ],

    onLaunch: function () {
        var me = this,
            model = me.getModel('StructElements'),
            mainView = me.getMainView();

        me.mask('Загрузка', mainView);
        if (me.params) {
            B4.Ajax.request({
                url: B4.Url.action('GetRealtyObjectStructElements', 'DisclosureInfoRealityObj'),
                params: {
                    diRoId: me.params.disclosureInfoRealityObjId
                },
                timeout: 9999999
            }).next(function (response) {
                var obj, record;

                try {
                    obj = Ext.decode(response.responseText);
                } catch (e) {
                    obj = {};
                }

                record = new model(obj);
                mainView.loadRecord(record);
                mainView.down('#disinfofacadegrid').store.loadData(obj.Facades);
                mainView.down('#disinforoofinggrid').store.loadData(obj.RoofTypes);
                me.unmask();
            }).error(function (e) {
                Ext.msg.alert('Ошибка', e.message || 'При обработке запроса произошла ошибка');
                me.unmask();
            });
        }
    }

});