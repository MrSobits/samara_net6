Ext.define('B4.controller.bankstatement.PaymentOrderOut', {
    /*
    * Контроллер раздела исходящих банковских выписок
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: ['B4.aspects.GridEditWindow'],

    models: ['bankstatement.PaymentOrderOut'],
    stores: ['bankstatement.PaymentOrderOut'],
    views: ['bankstatement.PaymentOrderOutGrid',
             'bankstatement.PaymentOrderOutEditWindow'],

    mainView: 'bankstatement.PaymentOrderOutGrid',
    mainViewSelector: '#paymentOrderOutGrid',

    aspects: [
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования исходящих банковских выписок
            */
            xtype: 'grideditwindowaspect',
            name: 'paymentOrderOutGridWindowAspect',
            gridSelector: '#paymentOrderOutGrid',
            editFormSelector: '#paymentOrderOutEditWindow',
            storeName: 'bankstatement.PaymentOrderOut',
            modelName: 'bankstatement.PaymentOrderOut',
            editWindowView: 'bankstatement.PaymentOrderOutEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (this.controller.params && !record.data.Id) {
                        record.data.BankStatement = this.controller.params.get('Id');
                    }
                }
            },
            otherActions: function (actions) {
                actions[this.editFormSelector + ' #chkbxRepeatSend'] = { 'change': { fn: this.onChangeRepeatSend, scope: this } };
            },
            onChangeRepeatSend: function (checkbox) {
                var editForm = this.getForm();
                if (checkbox.checked) {
                    editForm.down('#dcmfRedirectFunds').enable();
                } else {
                    editForm.down('#dcmfRedirectFunds').disable();
                }
            }
        }
    ],

    init: function () {
        this.getStore('bankstatement.PaymentOrderOut').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('bankstatement.PaymentOrderOut').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.bankStatementId = this.params.get('Id');
        }
    }
});