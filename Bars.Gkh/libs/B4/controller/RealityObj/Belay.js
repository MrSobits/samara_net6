Ext.define('B4.controller.realityobj.Belay', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.BelayPolicy'
    ],
    
    stores: [
        'realityobj.BelayPolicy'
    ],
    
    views: [
        'realityobj.BelayPolicyGrid',
        'realityobj.BelayPolicyWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjbelaypolicygrid'
        }
    ],
    
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjBelayPolicyWindowAspect',
            gridSelector: 'realityobjbelaypolicygrid',
            editFormSelector: 'realityobjbelaypolicywindow',
            storeName: 'realityobj.BelayPolicy',
            modelName: 'realityobj.BelayPolicy',
            editWindowView: 'realityobj.BelayPolicyWindow'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    init: function () {
        var me = this;
        
        me.getStore('realityobj.BelayPolicy').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjbelaypolicygrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.BelayPolicy').load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});