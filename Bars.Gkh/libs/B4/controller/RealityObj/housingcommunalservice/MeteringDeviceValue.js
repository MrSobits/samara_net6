Ext.define('B4.controller.realityobj.housingcommunalservice.MeteringDeviceValue', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.realityobj.housingcommunalservice.MeteringDeviceValue'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.housingcommunalservice.MeteringDeviceValue'
    ],
    
    stores: [
        'realityobj.housingcommunalservice.MeteringDeviceValue'
    ],
    
    views: [
        'realityobj.housingcommunalservice.MeteringDeviceValueEditWindow',
        'realityobj.housingcommunalservice.MeteringDeviceValueGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'hsemeteringdevicevaluegrid'
        },
        {
            ref: 'month',
            selector: 'hsemeteringdevicevaluegrid #cbMeteringMonth'
        },
        {
            ref: 'year',
            selector: 'hsemeteringdevicevaluegrid #cbMeteringYear'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'meteringdevicevalue',
            name: 'meteringDeviceValue'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'hseMeteringDeviceValueGridWindowAspect',
            gridSelector: 'hsemeteringdevicevaluegrid',
            editFormSelector: 'hsemeteringdevicevalueeditwindow',
            storeName: 'realityobj.housingcommunalservice.MeteringDeviceValue',
            modelName: 'realityobj.housingcommunalservice.MeteringDeviceValue',
            editWindowView: 'realityobj.housingcommunalservice.MeteringDeviceValueEditWindow',
            listeners: {
                getdata: function (me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Create', applyTo: 'b4addbutton', selector: 'hsemeteringdevicevaluegrid' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Edit', applyTo: 'b4savebutton', selector: 'hsemeteringdevicevalueeditwindow' },
                {
                    name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Delete', applyTo: 'b4deletecolumn', selector: 'hsemeteringdevicevaluegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;

        me.getStore('realityobj.housingcommunalservice.MeteringDeviceValue').on('beforeload', me.onBeforeLoad, me);
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view,
            dfMonth,
            dfYear;

        view = me.getMainView() || Ext.widget('hsemeteringdevicevaluegrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        dfMonth = me.getMonth(),
        dfYear = me.getYear(),

        dfMonth.setValue((new Date()).getMonth() + 1);
        dfYear.setValue((new Date()).getFullYear());

        me.getStore('realityobj.housingcommunalservice.MeteringDeviceValue').load();
        me.getAspect('meteringDeviceValue').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            dfMonth = me.getMonth(),
            dfYear = me.getYear(),
            year;

        if (dfYear) {
            year = dfYear.getValue();
            if (year) {
                operation.params.year = year;

                if (dfMonth) {
                    //если задан год
                    if (operation.params.year) {
                        operation.params.month = dfMonth.getValue();
                    } else {
                        window.dfMonth = dfMonth;
                        dfMonth.setValue(0);
                    }
                }
            }
        }

        
        operation.params.realityObjectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },
    
    onDateChange: function () {
        var me = this,
            store = me.getStore('realityobj.housingcommunalservice.MeteringDeviceValue');
        
        if (store) {
            store.load();
        }
    }
});