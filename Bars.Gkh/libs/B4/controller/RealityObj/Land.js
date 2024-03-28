Ext.define('B4.controller.realityobj.Land', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.Land'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.Land'
    ],

    stores: [
        'realityobj.Land'
    ],

    views: [
        'realityobj.LandGrid',
        'realityobj.LandEditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjlandgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'realityobjlandperm',
            name: 'realityObjLandPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjLandGridWindowAspect',
            gridSelector: 'realityobjlandgrid',
            editFormSelector: 'realityobjlandeditwindow',
            storeName: 'realityobj.Land',
            modelName: 'realityobj.Land',
            editWindowView: 'realityobj.LandEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            }
        }
    ],

    init: function () {
        var me = this;
        
        me.getStore('realityobj.Land').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjlandgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.Land').load();
        me.getAspect('realityObjLandPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});