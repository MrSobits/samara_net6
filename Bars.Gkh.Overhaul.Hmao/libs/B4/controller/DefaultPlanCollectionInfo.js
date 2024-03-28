Ext.define('B4.controller.DefaultPlanCollectionInfo', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['subsidy.DefaultPlanCollectionInfo'],
    
    views: ['subsidy.DefaultPlanCollectionInfoGrid'],

    stores: ['subsidy.DefaultPlanCollectionInfo'],

    mainView: 'subsidy.DefaultPlanCollectionInfoGrid',

    aspects: [
            {
                xtype: 'inlinegridaspect',
                name: 'defaultPlanCollectionInfoAspect',
                storeName: 'subsidy.DefaultPlanCollectionInfo',
                modelName: 'subsidy.DefaultPlanCollectionInfo',
                gridSelector: 'defaultplancollectioninfogrid'
            }
    ],

    refs: [
        { ref: 'mainView', selector: 'defaultplancollectioninfogrid' }
    ],

    index: function () {
        var me = this,
            view = me.getMainView();

        if (!view) {
            view = Ext.widget('defaultplancollectioninfogrid');

            me.bindContext(view);
            me.application.deployView(view);
        }

        view.getStore().load();
    }
});