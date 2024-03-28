Ext.define('B4.controller.longtermprobject.Loan', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        loader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody'
    },

    stores: ['longtermprobject.Loan'],

    models: ['longtermprobject.Loan'],
    views: [
        'longtermprobject.loan.Grid',
        'longtermprobject.loan.EditWindow'
    ],

    mainView: 'longtermprobject.loan.Grid',
    mainViewSelector: 'longtermprobjectloangrid',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'longtermprobjectloangrid',
            storeName: 'longtermprobject.Loan',
            modelName: 'longtermprobject.Loan',
            editFormSelector: 'longtermprobjectloanwindow',
            editWindowView: 'longtermprobject.loan.EditWindow',
            listeners: {
                beforesave: function (asp, record) {
                    var dateRep = record.get('DateRepayment'),
                        dateIss = record.get('DateIssue');

                    if (dateIss >= dateRep) {
                        B4.QuickMsg.msg('Предупреждение', 'Срок погашения не может быть больше даты выдачи', 'warning');
                        return false;
                    }

                    record.set('Object', asp.controller.params.longTermObjId);
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.LongTermProgramObject.Loan.Create', applyTo: 'b4addbutton', selector: 'longtermprobjectloangrid' },
                { name: 'Ovrhl.LongTermProgramObject.Loan.Edit', applyTo: 'b4savebutton', selector: 'longtermprobjectloanwindow' },
                { name: 'Ovrhl.LongTermProgramObject.Loan.Delete', applyTo: 'b4deletecolumn', selector: 'longtermprobjectloangrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        this.control({
            'longtermprobjectloanwindow b4selectfield[name=ObjectIssued]': { beforeload: { fn: this.onBeforeLoadObjects, scope: this } },
            'longtermprobjectloanwindow datefield[name=DateIssue]': { change: { fn: this.onChangeDate, scope: this } },
            'longtermprobjectloanwindow datefield[name=DateRepayment]': { change: { fn: this.onChangeDate, scope: this } }
        });

        this.getStore('longtermprobject.Loan').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onChangeDate: function(cmp) {
        var window = cmp.up('longtermprobjectloanwindow'),
            dateIss = window.down('datefield[name=DateIssue]').getValue(),
            dateRep = window.down('datefield[name=DateRepayment]').getValue(),
            month;
        
        if (!dateIss || !dateRep) {
            return;
        }

        if (dateRep > dateIss) {
            month = (dateRep.getYear() - dateIss.getYear()) * 12 + dateRep.getMonth() - dateIss.getMonth();
            window.down('numberfield[name=PeriodLoan]').setValue(month);
        } else {
            window.down('numberfield[name=PeriodLoan]').setValue(0);
        }
    },

    onBeforeLoad: function(store, operation) {
        operation.params.objectId = this.params.longTermObjId;
    },

    onBeforeLoadObjects: function (store, operation) {
        operation.params = operation.params || {};

        operation.params.objectId = this.params.longTermObjId;
    },

    onLaunch: function () {
        this.getMainView().getStore().load();
    }
});