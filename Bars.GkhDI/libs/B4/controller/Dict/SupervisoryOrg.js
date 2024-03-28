Ext.define('B4.controller.dict.SupervisoryOrg', {
    extend: 'B4.base.Controller',
    requires: [
         'B4.aspects.InlineGrid',
         'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: ['dict.SupervisoryOrg'],
    stores: ['dict.SupervisoryOrg'],
    views: ['dict.supervisoryorg.Grid'],

    mixins: {
        context: 'B4.mixins.Context'
    },
    mainView: 'dict.supervisoryorg.Grid',
    mainViewSelector: 'supervisoryOrgGrid',

    refs: [{
        ref: 'mainView',
        selector: 'supervisoryOrgGrid'
    }],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'supervisoryOrgGrid',
            permissionPrefix: 'GkhDi.Dict.SupervisoryOrg'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'supervisoryOrgGridAspect',
            storeName: 'dict.SupervisoryOrg',
            modelName: 'dict.SupervisoryOrg',
            gridSelector: 'supervisoryOrgGrid',
            listeners: {
                beforesave: function (asp, store) {
                    var result = true;
                    store.each(function (record) {
                        if (Ext.isEmpty(record.get('Name'))) {
                            Ext.Msg.alert('Ошибка сохранения', 'Необходимо заполнить наименование');
                            result = false;
                        }
                    });

                    return result;
                }
            }
        }],

    index: function () {
        var view = this.getMainView() || Ext.widget('supervisoryOrgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.SupervisoryOrg').load();
    }
});