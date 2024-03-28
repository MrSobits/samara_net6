Ext.define('B4.controller.RealtyObjTypeWorkCr', {
    extend: 'B4.controller.MenuItemController',

    mixins: {
        context: 'B4.mixins.Context'
    },
    
    models: [
        'RealtyObjTypeWorkCr'
    ],
    
    stores: [
        'RealtyObjTypeWorkCr'
    ],
    
    views: [
        'RealtyObjTypeWorkCrGrid'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'realtyobjtypeworkcrgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    init: function () {
        var me = this,
            store = me.getStore('RealtyObjTypeWorkCr');

        store.on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = this.getMainView() || Ext.widget('realtyobjtypeworkcrgrid');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getStore('RealtyObjTypeWorkCr').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.realityObjectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});