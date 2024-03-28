Ext.define('B4.controller.realityobj.ApartInfo', {
    extend: 'B4.controller.MenuItemController',
    
    requires: [
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.permission.realityobj.ApartInfo'
    ],

    models: [
        'realityobj.ApartInfo'
    ],
    
    stores: [
        'realityobj.ApartInfo'
    ],
    
    views: [
        'realityobj.ApartInfoGrid'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'realityobjapartinfogrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'realityobjapartinfoperm',
            name: 'realityObjApartInfoPerm'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'realityobjApartInfoInlineGridAspect',
            gridSelector: 'realityobjapartinfogrid',
            storeName: 'realityobj.ApartInfo',
            modelName: 'realityobj.ApartInfo',
            listeners: {
                beforesave: function (me, store) {
                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                        }
                    });
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.getStore('realityobj.ApartInfo').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('realityobjapartinfogrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.ApartInfo').load();
        me.getAspect('realityObjApartInfoPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});