Ext.define('B4.controller.KindCheckRuleReplace', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'KindCheckRuleReplace'
    ],

    stores: [
        'KindCheckRuleReplace'
    ],

    views: [
        'kindcheckrulereplace.Grid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'kindcheckrulereplacegrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'kindcheckreplaceGridAspect',
            storeName: 'KindCheckRuleReplace',
            modelName: 'KindCheckRuleReplace',
            gridSelector: 'kindcheckrulereplacegrid',
            listeners: {
                beforesave: function(me, store) {
                    var modifRecords = store.getModifiedRecords();

                    Ext.each(modifRecords, function(rec) {
                        rec.phantom = rec.get('Id') ? false : true;
                    });
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('kindcheckrulereplacegrid');

        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('KindCheckRuleReplace').load();
    }
});