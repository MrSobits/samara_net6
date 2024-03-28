Ext.define('B4.controller.realityobj.MeteringDevice', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.MeteringDevice'
    ],

    models: [
        'realityobj.MeteringDevice'
    ],

    stores: [
        'realityobj.MeteringDevice'
    ],
    
    views: [
        'realityobj.MeteringDeviceGrid',
        'realityobj.MeteringDeviceEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjMeteringDeviceGrid'
        }
    ],


    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'realityobjmeteringdeviceperm',
            name: 'realityObjMeteringDevicePerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjMeteringDeviceGridWindowAspect',
            gridSelector: 'realityobjMeteringDeviceGrid',
            editFormSelector: 'realityobjmetdeviceeditwindow',
            storeName: 'realityobj.MeteringDevice',
            modelName: 'realityobj.MeteringDevice',
            editWindowView: 'realityobj.MeteringDeviceEditWindow',
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
        
        me.getStore('realityobj.MeteringDevice').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjMeteringDeviceGrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info')

        me.getStore('realityobj.MeteringDevice').load();
        me.getAspect('realityObjMeteringDevicePerm').setPermissionsByRecord({ getId: function () { return id; } });
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});