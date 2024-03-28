Ext.define('B4.controller.realityobj.Housekeeper',
    {
        extend: 'B4.controller.MenuItemController',

        requires: [
            'B4.enums.YesNoNotSet',       
            'B4.aspects.GridEditWindow'
        ],

        models: [
            'realityobj.RealityObjectHousekeeper'
        ],

        stores: [
            'realityobj.RealityObjectHousekeeper'
        ],

        views: [
            'realityobj.HousekeeperGrid',
            'realityobj.HousekeeperEditWindow'
        ],

        mixins: {
            context: 'B4.mixins.Context'
        },

        refs: [
            {
                ref: 'mainView',
                selector: 'realityobjhousekeepergrid'
            }
        ],

        parentCtrlCls: 'B4.controller.realityobj.Navi',

        aspects: [
            {
                xtype: 'grideditwindowaspect',
                name: 'realityobjtHousekeeperGridWindowAspect',
                gridSelector: 'realityobjhousekeepergrid',
                editFormSelector: '#realityobjHousekeeperEditWindow',
                storeName: 'realityobj.RealityObjectHousekeeper',
                modelName: 'realityobj.RealityObjectHousekeeper',
                editWindowView: 'realityobj.HousekeeperEditWindow',
                listeners: {
                    getdata: function (asp, record) {
                        var me = this;
                        debugger;
                        if (!record.data.Id) {
                            record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                        }
                    }
                }
            }
        ],

        init: function () {
            var me = this;
            me.getStore('realityobj.RealityObjectHousekeeper').on('beforeload', me.onBeforeLoad, me);
            me.callParent(arguments);
        },

        index: function (id) {
            var me = this,
                view = me.getMainView() || Ext.widget('realityobjhousekeepergrid');

            me.bindContext(view);
            me.setContextValue(view, 'realityObjectId', id);
            me.application.deployView(view, 'reality_object_info');

            me.getStore('realityobj.RealityObjectHousekeeper').load();
        },

        onBeforeLoad: function (store, operation) {
            var me = this;
            operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
        }
    });