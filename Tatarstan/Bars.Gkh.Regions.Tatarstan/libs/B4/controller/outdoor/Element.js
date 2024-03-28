Ext.define('B4.controller.outdoor.Element', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.outdoor.Element',
        'B4.aspects.permission.outdoor.OutdoorFields'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'outdoor.Element'
    ],

    models: [
        'outdoor.Element'
    ],

    views: [
        'outdoor.element.ElementBeforeCrGrid',
        'outdoor.element.ElementEdit'
    ],

    parentCtrlCls: 'B4.controller.realityobj.realityobjectoutdoor.Navi',

    mainView: 'outdoor.element.ElementBeforeCrGrid',
    mainViewSelector: 'outdoorelementgrid',

    aspects: [
        {
            xtype: 'outdoorelementperm',
            name: 'outdoorElementPerm'
        },
        {
            xtype: 'outdoorfieldsperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'outdoorElementGridWindowAspect',
            gridSelector: 'outdoorelementgrid',
            editFormSelector: 'outdoorelementeditwindow',
            modelName: 'outdoor.Element',
            editWindowView: 'outdoor.element.ElementEdit',

            listeners: {
                beforesetformdata: function(asp, rec, form) {
                    form.down('b4selectfield[name=Element]').setDisabled(rec.data.Id);
                },
                getdata: function(asp, record) {
                    var me = this;
                    if (!record.getId()) {
                        record.data.Outdoor =
                            me.controller.getContextValue(me.controller.getMainView(), 'outdoorId');
                    }
                }
            },
            otherActions: function(actions) {
                var me = this;
                actions[me.editFormSelector + ' b4selectfield[name=Element]'] =
                    { 'change': { fn: me.onChangeElement, scope: me } };
            },
            onChangeElement: function(f, newValue) {
                var me = this,
                    form = me.getForm();

                if (newValue) {
                    form.down('textfield[name=Measure]').setValue(newValue.UnitMeasure);
                }
            }
        }
    ],

    init: function() {
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