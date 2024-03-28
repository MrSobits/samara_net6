Ext.define('B4.controller.realityobj.MeteringDevicesChecks', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.MeteringDevicesChecksPermission',
        'B4.aspects.fieldrequirement.MeteringDevicesChecks'
    ],

    models: ['realityobj.MeteringDevicesChecks'],
    stores: ['realityobj.MeteringDevicesChecks'],
    views:  ['realityobj.meteringdeviceschecks.Grid', 'realityobj.meteringdeviceschecks.EditWindow' ],

    mainView: 'realityobj.meteringdeviceschecks.Grid',
    mainViewSelector: 'meteringDevicesChecksGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'meteringDevicesChecksGrid'
        }
    ],

    aspects: [
        {
            xtype: 'meteringdeviceschecksperm'
        },
        {
            xtype: 'meteringdeviceschecksfieldrequirement'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'meteringDevicesChecksGridAspect',
            gridSelector: 'meteringDevicesChecksGrid',
            editFormSelector: 'meteringdeviceschecksEditWindow',
            storeName: 'realityobj.MeteringDevicesChecks',
            modelName: 'realityobj.MeteringDevicesChecks',
            editWindowView: 'realityobj.meteringdeviceschecks.EditWindow',
            otherActions: function (actions) {
                var me = this;
                actions['meteringdeviceschecksEditWindow b4selectfield[name=MeteringDevice]'] = { 'beforeload': { fn: me.onBeforeLoad, scope: me } };
            },
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            },
            updateGrid: function() {
                var me = this;
                me.getGrid().getStore().load();
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.objectId = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'meteringDevicesChecksGrid': {
                'afterrender': {
                    fn: function(grid) {
                        grid.getStore().on('beforeload', me.onBeforeLoad, me);
                    },
                    scope: me
                }
            }
        });
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('meteringDevicesChecksGrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        view.getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this;
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    }
});
