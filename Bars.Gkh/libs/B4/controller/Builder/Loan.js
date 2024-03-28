Ext.define('B4.controller.builder.Loan', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.GkhInlineGrid'
    ],

    models: [
        'builder.LoanRepayment'
    ],
    stores: [
        'builder.Loan',
        'builder.LoanRepayment'
    ],
    views: [
        'builder.LoanEditWindow',
        'builder.LoanGrid',
        'builder.LoanRepaymentGrid'
    ],
    
    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
            name: 'builderLoanRepaymentInlineGridAspect',
            gridSelector: '#builderLoanRepaymentGrid',
            storeName: 'builder.LoanRepayment',
            modelName: 'builder.LoanRepayment',
            saveButtonSelector: '#builderLoanRepaymentGrid #builderLoanRepaymentSaveButton',
            listeners: {
                beforeaddrecord: function (asp, record) {
                    record.data.BuilderLoan = this.controller.builderLoanId;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'builderLoanGridWindowAspect',
            gridSelector: '#builderLoanGrid',
            editFormSelector: '#builderLoanEditWindow',
            storeName: 'builder.Loan',
            modelName: 'builder.Loan',
            editWindowView: 'builder.LoanEditWindow',
            onSaveSuccess: function (asp, record) {
                asp.controller.setCurrentId(record.getId());
            },
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.Builder = this.controller.params.get('Id');
                    }
                },
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record.getId());
                }
            }
        }
    ],

    params: null,
    mainView: 'builder.LoanGrid',
    mainViewSelector: '#builderLoanGrid',

    //Селектор окна котоырй потом используется при октрытии
    builderLoanEditWindow: '#builderLoanEditWindow',

    init: function () {
        this.getStore('builder.Loan').on('beforeload', this.onBeforeLoad, this, 'BuilderLoan');
        this.getStore('builder.LoanRepayment').on('beforeload', this.onBeforeLoad, this, 'BuilderLoanRepayment');

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('builder.Loan').load();
    },

    onBeforeLoad: function (store, operation, type) {
        if (type == 'BuilderLoan') {
            if (this.params) {
                operation.params.builderId = this.params.get('Id');
            }
        }
        if (type == 'BuilderLoanRepayment') {
            operation.params.builderLoanId = this.builderLoanId;
        }
    },

    setCurrentId: function (id) {
        this.builderLoanId = id;

        var editWindow = Ext.ComponentQuery.query(this.builderLoanEditWindow)[0];

        var store = this.getStore('builder.LoanRepayment');
        store.removeAll();

        if (id > 0) {
            editWindow.down('#builderLoanRepaymentGrid').setDisabled(false);
            store.load();
        } else {
            editWindow.down('#builderLoanRepaymentGrid').setDisabled(true);
        }
    }
});