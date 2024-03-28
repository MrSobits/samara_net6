Ext.define('B4.controller.dict.InspectorEdoInteg', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.InspectorEdoInteg'],
    stores: ['dict.InspectorEdoInteg'],

    views: ['dict.inspEdoIntegGrid'],

    mainView: 'dict.inspEdoIntegGrid',
    mainViewSelector: 'inspEdoIntegGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspEdoIntegGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'inspEdoIntegGridAspect',
            storeName: 'dict.InspectorEdoInteg',
            modelName: 'dict.InspectorEdoInteg',
            gridSelector: 'inspEdoIntegGrid',
            listeners: {
                beforesave: function (asp, store) {
                    var modifiedRecs = store.getModifiedRecords();

                    Ext.each(modifiedRecs, function (rec) {
                        var compareId = rec.get('CompareId'),
                            inspId = rec.getId();
                        if (Ext.isEmpty(compareId) || compareId == 0) {
                            rec.phantom = true;
                        }
                        rec.set('Id', compareId);
                        rec.set('Inspector', inspId);
                    });
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('inspEdoIntegGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.InspectorEdoInteg').load();
    }
});