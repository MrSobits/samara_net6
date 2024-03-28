Ext.define('B4.controller.dict.TerminationReason', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.terminationreason.Grid'],

    mainView: 'dict.terminationreason.Grid',
    mainViewSelector: 'terminationreasongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'terminationreasongrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.TerminationReason',
            modelName: 'dict.TerminationReason',
            gridSelector: 'terminationreasongrid'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('terminationreasongrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});