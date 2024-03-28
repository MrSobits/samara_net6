Ext.define('B4.controller.bankstatement.PaymentOrderIn', {
    /*
    * Контроллер раздела входящих банковских выписок
    */
    extend: 'B4.base.Controller',
    params: null,
    requires: ['B4.aspects.GridEditWindow'],

    models: ['bankstatement.PaymentOrderIn'],
    stores: ['bankstatement.PaymentOrderIn'],
    views: ['bankstatement.PaymentOrderInGrid', 
'bankstatement.PaymentOrderInEditWindow'],

    mainView: 'bankstatement.PaymentOrderInGrid',
    mainViewSelector: '#paymentOrderInGrid',

    aspects: [
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования входящих банковских выписок
            */
            xtype: 'grideditwindowaspect',
            name: 'paymentOrderInGridWindowAspect',
            gridSelector: '#paymentOrderInGrid',
            editFormSelector: '#paymentOrderInEditWindow',
            storeName: 'bankstatement.PaymentOrderIn',
            modelName: 'bankstatement.PaymentOrderIn',
            editWindowView: 'bankstatement.PaymentOrderInEditWindow',
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
        this.getStore('bankstatement.PaymentOrderIn').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('bankstatement.PaymentOrderIn').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.bankStatementId = this.params.get('Id');
        }
    }
});