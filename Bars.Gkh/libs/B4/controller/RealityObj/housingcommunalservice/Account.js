Ext.define('B4.controller.realityobj.housingcommunalservice.Account', {
    extend: 'B4.controller.MenuItemController',
    params: {},

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.housingcommunalservice.Account'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },


    models: [
        'realityobj.housingcommunalservice.Account',
        'realityobj.housingcommunalservice.AccountCharge',
        'realityobj.housingcommunalservice.AccountMeteringDeviceValue'
    ],

    stores: [
        'realityobj.housingcommunalservice.Account',
        'realityobj.housingcommunalservice.AccountCharge',
        'realityobj.housingcommunalservice.AccountMeteringDeviceValue'
    ],

    views: [
        'realityobj.housingcommunalservice.AccountChargeEditWindow',
        'realityobj.housingcommunalservice.AccountChargeGrid',
        'realityobj.housingcommunalservice.AccountMeteringDeviceValueEditWindow',
        'realityobj.housingcommunalservice.AccountMeteringDeviceValueGrid',
        'realityobj.housingcommunalservice.AccountEditWindow',
        'realityobj.housingcommunalservice.AccountGrid'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    refs: [
        {
            ref: 'mainView',
            selector: 'hseaccountgrid'
        },
        {
            ref: 'accountChargeGrid',
            selector: 'hseaccountchargegrid'
        },
        {
            ref: 'accountMeteringDeviceValueGrid',
            selector: 'hseaccountmeteringdevicevaluegrid'
        },
        {
            ref: 'chargingMonth',
            selector: 'hseaccounteditwindow #AccountChargeMonth'
        },
        {
            ref: 'meteringMonth',
            selector: 'hseaccounteditwindow #AccountMeteringMonth'
        },
        {
            ref: 'meteringYear',
            selector: 'hseaccounteditwindow #AccountMeteringYear'
        }
    ],

    aspects: [
        {
            xtype: 'accountperm',
            name: 'accountPerm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'hseAccountGridWindowAspect',
            gridSelector: 'hseaccountgrid',
            editFormSelector: 'hseaccounteditwindow',
            storeName: 'realityobj.housingcommunalservice.Account',
            modelName: 'realityobj.housingcommunalservice.Account',
            editWindowView: 'realityobj.housingcommunalservice.AccountEditWindow',
            listeners: {
                getdata: function(me, record) {
                    if (!record.data.Id) {
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                },
                beforesetformdata: function(me, record) {
                    me.controller.setCurrentId(record.getId());
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'hseAccountChargeGridWindowAspect',
            gridSelector: 'hseaccountchargegrid',
            editFormSelector: 'hseaccountchargeeditwindow',
            storeName: 'realityobj.housingcommunalservice.AccountCharge',
            modelName: 'realityobj.housingcommunalservice.AccountCharge',
            editWindowView: 'realityobj.housingcommunalservice.AccountChargeEditWindow',
            listeners: {
                getdata: function(me, record) {
                    if (!record.data.Id) {
                        record.data.Account = me.controller.getContextValue(me.controller.getMainComponent(), 'accountId');
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'hseAccountMeteringDeviceValueGridWindowAspect',
            gridSelector: 'hseaccountmeteringdevicevaluegrid',
            editFormSelector: 'hseaccountmeteringdevicevalueeditwindow',
            storeName: 'realityobj.housingcommunalservice.AccountMeteringDeviceValue',
            modelName: 'realityobj.housingcommunalservice.AccountMeteringDeviceValue',
            editWindowView: 'realityobj.housingcommunalservice.AccountMeteringDeviceValueEditWindow',
            listeners: {
                getdata: function(me, record) {
                    if (!record.data.Id) {
                        record.data.Account = me.controller.getContextValue(me.controller.getMainComponent(), 'accountId');
                        record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Create', applyTo: 'b4addbutton', selector: 'hseaccountgrid' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Create', applyTo: 'b4addbutton', selector: 'hseaccountchargegrid' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Create', applyTo: 'b4addbutton', selector: 'hseaccountmeteringdevicevaluegrid' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Edit', applyTo: 'b4savebutton', selector: 'hseaccounteditwindow' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Edit', applyTo: 'b4savebutton', selector: 'hseaccountchargeeditwindow' },
                { name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Edit', applyTo: 'b4savebutton', selector: 'hseaccountmeteringdevicevalueeditwindow' },                
                {
                    name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'hseaccountgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'hseaccountchargegrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HousingComminalService.Account.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'hseaccountmeteringdevicevaluegrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this,
            actions = {};
        
        me.getStore('realityobj.housingcommunalservice.Account').on('beforeload', me.onBeforeLoad, me);
        me.getStore('realityobj.housingcommunalservice.AccountCharge').on('beforeload', me.onBeforeLoadAccountChargeStore, me);
        me.getStore('realityobj.housingcommunalservice.AccountMeteringDeviceValue').on('beforeload', me.onBeforeLoadAccountMeterStore, me);

        actions['hseaccounteditwindow #AccountChargeMonth'] = {
             'change': { fn: me.onChardingDateChange, scope: me }
        };
        
        me.control(actions);

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('hseaccountgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
        
        me.getStore('realityobj.housingcommunalservice.Account').load();
        me.getAspect('accountPerm').setPermissionsByRecord({ getId: function () { return id; } });
    },

    onBeforeLoad: function(store, operation) {
        var me = this;
        operation.params.realityObjectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },

    setCurrentId: function (id) {
        var me = this,
            dfMonth = me.getMeteringMonth(),
            dfYear = me.getMeteringYear(),
            store1 = me.getStore('realityobj.housingcommunalservice.AccountCharge'),
            store2 = me.getStore('realityobj.housingcommunalservice.AccountMeteringDeviceValue'),
            accountChargeGrid = me.getAccountChargeGrid(),
            accountMeteringDeviceValueGrid = me.getAccountMeteringDeviceValueGrid();

        dfMonth.setValue((new Date()).getMonth() + 1);
        dfYear.setValue((new Date()).getFullYear());
        
        me.setContextValue(me.getMainComponent(), 'accountId', id);

        store1.removeAll();
        store2.removeAll();

        if (accountChargeGrid) {
            accountChargeGrid.setDisabled(id == 0);
        }

        if (accountMeteringDeviceValueGrid) {
            accountMeteringDeviceValueGrid.setDisabled(id == 0);
        }

        if (id > 0) {
            store1.load();
            store2.load();
        }
    },

    onBeforeLoadAccountChargeStore: function(store, operation) {
        var me = this,
            dfMonth = me.getChargingMonth();

        if (dfMonth) {
            operation.params.date = dfMonth.getValue();
        }

        operation.params.accountId = me.getContextValue(me.getMainComponent(), 'accountId');
    },

    onBeforeLoadAccountMeterStore: function(store, operation) {
        var me = this,
            dfMonth = me.getMeteringMonth(),
            dfYear = me.getMeteringYear(),
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

        operation.params.accountId = me.getContextValue(me.getMainComponent(), 'accountId');
    },

    onChardingDateChange: function() {
        var me = this,
            store = me.getStore('realityobj.housingcommunalservice.AccountCharge');

        if (store) {
            store.load();
        }
    }
});