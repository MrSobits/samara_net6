Ext.define('B4.controller.calcaccount.PaymentCrSpecAccNotRegop', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.calcaccount.PaymentCrSpecAccNotRegopGrid',
        'B4.view.calcaccount.PaymentCrSpecAccNotRegopEditWindow',
        'B4.store.calcaccount.PaymentCrSpecAccNotRegop',
        'B4.Url',
        'B4.Ajax'
    ],

    mainView: 'calcaccount.PaymentCrSpecAccNotRegopGrid',
    mainViewSelector: 'paymentCrSpecAccNotRegopGrid',

    stores: [
        'calcaccount.PaymentCrSpecAccNotRegop'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'sfPeriod', selector: 'paymentCrSpecAccNotRegopGrid b4selectfield' },
        { ref: 'editWindow', selector: 'paymentCrSpecAccNotRegopEditWindow' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'paymentCrSpecAccNotRegopGridWindowAspect',
            gridSelector: 'paymentCrSpecAccNotRegopGrid',
            editFormSelector: 'paymentCrSpecAccNotRegopEditWindow',
            storeName: 'calcaccount.PaymentCrSpecAccNotRegop',
            modelName: 'calcaccount.PaymentCrSpecAccNotRegop',
            editWindowView: 'calcaccount.PaymentCrSpecAccNotRegopEditWindow',
            onSaveSuccess: function() {
                this.getForm().close();
            },
            editRecord: function (record) {
                var me = this,
                    periodSelField = me.controller.getMainView().down('b4selectfield'),
                    model = me.getModel(record);

                B4.Ajax.request({
                    url: B4.Url.action('GetPaymentCrSpecAccNotRegop', 'SpecialCalcAccount'),
                    params: {
                        id: record ? record.getId() : null,
                        periodId: periodSelField.getValue()
                    }
                }).next(function (response) {
                    if (response) {
                        var resp = Ext.JSON.decode(response.responseText),
                            data = resp.data[0];
                        me.setFormData(new model({
                            Id: data.Id,
                            Period: data.Period,
                            Address: data.Address,
                            InputDate: data.InputDate,
                            AmountIncome: data.AmountIncome,
                            EndYearBalance: data.EndYearBalance,
                            File: data.File
                        }));
                    }
                });

                me.getForm().getForm().isValid();
            }
        }
    ],

    index: function () {
        var me = this,
            grid = me.getMainView(),
            periodSelField;
        
        if (!grid) {
            grid = Ext.widget('paymentCrSpecAccNotRegopGrid');
            periodSelField = grid.down('b4selectfield[name=ChargePeriod]');

            B4.Ajax.request(B4.Url.action('GetCurrentPeriod', 'SpecialCalcAccount')
            ).next(function(response) {
                if (response) {
                    var resp = Ext.JSON.decode(response.responseText);
                    periodSelField.setValue(resp.Id);
                    periodSelField.setRawValue(resp.Name);
                    grid.getStore().filter('periodId', resp.Id);
                }
            });
        }

        me.bindContext(grid);
        me.application.deployView(grid);
    },

    init: function () {
        var me = this;

        me.control({
            'paymentCrSpecAccNotRegopGrid b4updatebutton': {
                click: me.updateGrid
            },
            'paymentCrSpecAccNotRegopGrid b4selectfield[name=ChargePeriod]': {
                change: me.updateGrid
            }
        });

        me.callParent(arguments);
    },

    updateGrid: function (btn) {
        var grid = btn.up('grid'),
            periodId = grid.down('b4selectfield').getValue();

        grid.getStore().filter('periodId', periodId);
    },

    onPaymentCrSpecAccNotRegopStoreBeforeLoad: function (store) {
        var periodVal = this.getSfPeriod().getValue();

        Ext.apply(store.getProxy().extraParams, {
            periodId: periodVal
        });
    }
});