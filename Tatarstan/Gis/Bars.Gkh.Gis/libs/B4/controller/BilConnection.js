Ext.define('B4.controller.BilConnection', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.BilConnectionGrid',
        'B4.store.BilConnection',
        'B4.model.BilConnection',
        'B4.view.BilConnectionEditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'BilConnectionGrid',
    mainViewSelector: 'bilconnectiongrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'bilconnectiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'bilconnectionGridWindowAspect',
            gridSelector: 'bilconnectiongrid',
            editFormSelector: 'bilconnectioneditwindow',
            modelName: 'BilConnection',
            editWindowView: 'BilConnectionEditWindow'
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('bilconnectiongrid');
        me.bindContext(view);
        view.getStore().load();
    }
});