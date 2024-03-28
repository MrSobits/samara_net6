Ext.define('B4.controller.outdoor.Image', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.outdoor.Image', 
        'B4.aspects.permission.outdoor.OutdoorFields',
        'B4.aspects.fieldrequirement.outdoor.Image'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'outdoor.Image'
    ],

    models: [
        'outdoor.Image'
    ],

    views: [
        'outdoor.image.ImageGrid',
        'outdoor.image.EditWindow'
    ],

    parentCtrlCls: 'B4.controller.realityobj.realityobjectoutdoor.Navi',

    mainView: 'outdoor.image.ImageGrid',
    mainViewSelector: 'outdoorimagegrid',

    aspects: [
        {
            xtype: 'outdoorimageperm',
            name: 'outdoorImagePerm'
        },
        {
            xtype:'outdoorimagerequirement',
            name: 'outdoorImageRequirementAspect',
            viewSelector: 'outdoorimageeditwindow'
        },
        {
            xtype: 'outdoorfieldsperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'outdoorImageGridWindowAspect',
            gridSelector: 'outdoorimagegrid',
            editFormSelector: 'outdoorimageeditwindow',
            modelName: 'outdoor.Image',
            editWindowView: 'outdoor.image.EditWindow',

            listeners: {
                getdata: function(asp, record) {
                    var me = this;
                    if (!record.getId()) {
                        record.set('Outdoor', me.controller.getContextValue(me.controller.getMainView(), 'outdoorId'));
                    }
                }
            },
            otherActions: function(actions) {
                var me = this;

                actions[me.editFormSelector + ' b4selectfield[name=WorkCr]'] = {
                    'beforeload': { fn: me.onBeforeLoadWork, scope: me }
                };
                actions[me.editFormSelector + ' b4selectfield[name=Period]'] = {
                    'change': { fn: me.onChangePeriod, scope: me }
                };

            },
            onBeforeLoadWork: function (store, operation) {
                var me = this;
                operation = operation || {};
                operation.params = operation.params || {};
                
                operation.params.outdoorId = me.controller.getContextValue(me.controller.getMainComponent(), 'outdoorId');
                operation.params.periodId = me.controller.getContextValue(me.controller.getMainComponent(), 'periodId');
            },
            onChangePeriod: function (field, newValue) {
                var me = this;
                if (newValue) {
                    me.controller.setContextValue(me.controller.getMainComponent(), 'periodId', newValue.Id);
                } else {
                    me.controller.setContextValue(me.controller.getMainComponent(), 'periodId', 0);
                }
            }
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions[me.mainViewSelector] = { 'store.beforeload': { fn: me.onBeforeLoad, scope: me } };

        me.control(actions);
        me.callParent(arguments);
    },

    index: function(id) {
        var me = this,
            view = me.getMainComponent();

        me.bindContext(view);
        me.setContextValue(view, 'outdoorId', id);
        me.application.deployView(view, 'realityobject_outdoor_info');

        view.getStore().load();
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        operation.params.outdoorId = me.getContextValue(me.getMainView(), 'outdoorId');
    }
});