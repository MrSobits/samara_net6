Ext.define('B4.controller.dict.KindStatementEdoInteg', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KindStatementEdoInteg'],
    stores: ['dict.KindStatementEdoInteg'],

    views: ['dict.kindStEdoIntegGrid'],

    mainView: 'dict.kindStEdoIntegGrid',
    mainViewSelector: 'kindStEdoIntegGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'kindStEdoIntegGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'kindStEdoIntegGridAspect',
            storeName: 'dict.KindStatementEdoInteg',
            modelName: 'dict.KindStatementEdoInteg',
            gridSelector: 'kindStEdoIntegGrid',
            listeners: {
                beforesave: function (asp, store) {
                    var modifiedRecs = store.getModifiedRecords();

                    Ext.each(modifiedRecs, function (rec) {
                        var compareId = rec.get('CompareId'),
                             kindStId = rec.getId();
                        if (Ext.isEmpty(compareId) || compareId == 0) {
                            rec.phantom = true;
                        }
                        rec.set('Id', compareId);
                        rec.set('KindStatement', kindStId);
                    });
                }
            }

        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('kindStEdoIntegGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.KindStatementEdoInteg').load();
    }
});