Ext.define('B4.controller.gkuinfo.housingcommunalservice.MeteringDeviceValue', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.permission.GkhPermissionAspect'
    ],

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

    mainView: 'realityobj.housingcommunalservice.MeteringDeviceValueGrid',
    mainViewSelector: 'hsemeteringdevicevaluegrid',

    monthSelector: 'hsemeteringdevicevaluegrid #cbMeteringMonth',
    yearSelector: 'hsemeteringdevicevaluegrid #cbMeteringYear',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'hseMeteringDeviceValueGridWindowAspect',
            gridSelector: 'hsemeteringdevicevaluegrid',
            editFormSelector: 'hsemeteringdevicevalueeditwindow',
            storeName: 'realityobj.housingcommunalservice.MeteringDeviceValue',
            modelName: 'realityobj.housingcommunalservice.MeteringDeviceValue',
            editWindowView: 'realityobj.housingcommunalservice.MeteringDeviceValueEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.RealityObject = this.controller.params.realityObjectId;
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
                    name: 'Gkh.RealityObject.Register.HousingComminalService.MeteringDeviceValue.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'hsemeteringdevicevaluegrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function() {
        this.getStore('realityobj.housingcommunalservice.MeteringDeviceValue').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        var dfMonth = Ext.ComponentQuery.query(this.monthSelector)[0],
            dfYear = Ext.ComponentQuery.query(this.yearSelector)[0];

        dfMonth.setValue((new Date()).getMonth() + 1);
        dfYear.setValue((new Date()).getFullYear());

        this.getStore('realityobj.housingcommunalservice.MeteringDeviceValue').load();
    },

    onBeforeLoad: function(store, operation) {
        var dfMonth = Ext.ComponentQuery.query(this.monthSelector)[0],
            dfYear = Ext.ComponentQuery.query(this.yearSelector)[0],
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

        if (this.params) {
            operation.params.realityObjectId = this.params.realityObjectId;
        }
    },

    onDateChange: function() {
        var store = this.getStore('realityobj.housingcommunalservice.MeteringDeviceValue');

        if (store) {
            store.load();
        }
    }
});