Ext.define('B4.controller.realityobj.HouseInfo', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.HouseInfo'
    ],

    models: [
        'realityobj.HouseInfo'
    ],
    
    stores: [
        'realityobj.HouseInfo'
    ],

    views: [
        'realityobj.HouseInfoGrid',
        'realityobj.HouseInfoEditWindow'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjhouseinfogrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'realityobjhouseinfoperm',
            name: 'realityObjHouseInfoPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjHouseInfoGridWindowAspect',
            gridSelector: 'realityobjhouseinfogrid',
            editFormSelector: 'realityobjhouseinfoeditwindow',
            storeName: 'realityobj.HouseInfo',
            modelName: 'realityobj.HouseInfo',
            editWindowView: 'realityobj.HouseInfoEditWindow',
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
        
        me.getStore('realityobj.HouseInfo').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjhouseinfogrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getStore('realityobj.HouseInfo').load();
        me.getAspect('realityObjHouseInfoPerm').setPermissionsByRecord({ getId: function () { return id; } });

    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    }
});