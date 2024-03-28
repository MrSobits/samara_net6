Ext.define('B4.controller.dict.RevenueFormEdoInteg', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.RevenueFormEdoInteg'],
    stores: ['dict.RevenueFormEdoInteg'],

    views: ['dict.revFormEdoIntegGrid'],

    mainView: 'dict.revFormEdoIntegGrid',
    mainViewSelector: 'revFormEdoIntegGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'revFormEdoIntegGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'revenueFormGjiEdoIntegGridAspect',
            storeName: 'dict.RevenueFormEdoInteg',
            modelName: 'dict.RevenueFormEdoInteg',
            gridSelector: 'revFormEdoIntegGrid',
            listeners: {
                beforesave: function (asp, store) {
                    var modifiedRecs = store.getModifiedRecords();

                    Ext.each(modifiedRecs, function (rec) {
                        var compareId = rec.get('CompareId'),
                            revenueFormId = rec.getId();
                        if (Ext.isEmpty(compareId) || compareId == 0) {
                            rec.phantom = true;
                        }
                        rec.set('Id', compareId);
                        rec.set('RevenueForm', revenueFormId);
                    });
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('revFormEdoIntegGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.RevenueFormEdoInteg').load();
    }
});