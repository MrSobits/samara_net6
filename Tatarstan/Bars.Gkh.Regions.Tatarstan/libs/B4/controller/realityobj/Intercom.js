Ext.define('B4.controller.realityobj.Intercom', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.Intercom',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    views: [
        'realityobj.IntercomGrid',
        'realityobj.IntercomWindow'
    ],

    mainView: 'realityobj.IntercomGrid',
    mainViewSelector: 'intercomgrid',

    aspects: [
        {
            xtype: 'realityobjintercompermissionaspect',
            name: 'realityObjIntercomPermissionAspect'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'intercomWindowAspect',
            gridSelector: 'intercomgrid',
            modelName: 'realityobj.Intercom',
            editFormSelector: 'intercomwindow',
            editWindowView: 'B4.view.realityobj.IntercomWindow',
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
        var me = this,
            actions = {};

        actions[me.mainViewSelector] = {
            'store.beforeload': { fn: me.onBeforeLoad, scope: me },
            'store.load': { fn: me.setPermissions, scope: me }
        };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);


        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
    },

    setPermissions: function (store) {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.getAspect('realityObjIntercomPermissionAspect').setPermissionsByRecord({ getId: function () { return me.getContextValue(view, 'realityObjectId'); } });
    }
});