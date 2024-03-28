Ext.define('B4.controller.dict.PenaltiesWithDeferred', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: [
        'dict.PenaltiesWithDeferred'
    ],
    stores: [
        'dict.PenaltiesWithDeferred'
    ],
    views: [
        'dict.penaltieswithdeferred.Grid',
        'dict.penaltieswithdeferred.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.penaltieswithdeferred.Grid',
    mainViewSelector: 'penaltiesdeferredGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'penaltiesdeferredGrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentpenaltiesGridWindowAspect',
            gridSelector: 'penaltiesdeferredGrid',
            editFormSelector: 'penaltieswithdeferrededitwindow',
            storeName: 'dict.PenaltiesWithDeferred',
            modelName: 'dict.PenaltiesWithDeferred',
            editWindowView: 'dict.penaltieswithdeferred.EditWindow'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('penaltiesdeferredGrid');
        me.bindContext(view);
        me.getStore('dict.PenaltiesWithDeferred').load();
    }
});