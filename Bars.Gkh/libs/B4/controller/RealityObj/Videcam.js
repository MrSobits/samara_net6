Ext.define('B4.controller.realityobj.Videcam',
    {
        extend: 'B4.controller.MenuItemController',

        requires: [
            'B4.enums.YesNoNotSet',
            'B4.aspects.GridEditWindow'
        ],

        models: [
            'realityobj.RealityObjectVidecam'
        ],

        stores: [
            'realityobj.RealityObjectVidecam'
        ],

        views: [
            'realityobj.VidecamGrid',
            'realityobj.VidecamEditWindow'
        ],

        mixins: {
            context: 'B4.mixins.Context'
        },

        refs: [
            {
                ref: 'mainView',
                selector: 'realityobjvidecamgrid'
            }
        ],

        parentCtrlCls: 'B4.controller.realityobj.Navi',

        aspects: [
            //{
            //    xtype: 'realityobjantennaperm',
            //    name: 'realityObjAntennaPerm'
            //}
            {
                xtype: 'grideditwindowaspect',
                name: 'realityobjvidecamgridWindowAspect',
                gridSelector: 'realityobjvidecamgrid',
                editFormSelector: '#realityobjVidecamEditWindow',
                storeName: 'realityobj.RealityObjectVidecam',
                modelName: 'realityobj.RealityObjectVidecam',
                editWindowView: 'realityobj.VidecamEditWindow',
                listeners: {
                    getdata: function (asp, record) {
                        var me = this;
                        
                        if (!record.data.Id) {
                            record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                        }
                    }
                }              
            }
        ],

        init: function () {
            var me = this;

            me.getStore('realityobj.RealityObjectVidecam').on('beforeload', me.onBeforeLoad, me);
            me.callParent(arguments);
        },

        index: function (id) {
            var me = this,
                view = me.getMainView() || Ext.widget('realityobjvidecamgrid');

            me.bindContext(view);
            me.setContextValue(view, 'realityObjectId', id);
            me.application.deployView(view, 'reality_object_info');

            me.getStore('realityobj.RealityObjectVidecam').load();
            //me.getAspect('realityObjAntennaPerm').setPermissionsByRecord({ getId: function () { return id; } });
        },

        onBeforeLoad: function (store, operation) {
            var me = this;
            operation.params.objectId = me.getContextValue(me.getMainView(), 'realityObjectId');
        }
    });