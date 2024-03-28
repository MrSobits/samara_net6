Ext.define('B4.controller.belaypolicy.Payment', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GridEditWindow'],

    models: ['belaypolicy.Payment'],
    stores: ['belaypolicy.Payment'],
    views: [
        'belaypolicy.PaymentEditWindow',
        'belaypolicy.PaymentGrid'
    ],

    aspects: [{
        xtype: 'grideditwindowaspect',
        name: 'belayorgPaymentGridWindowAspect',
        gridSelector: '#belayPolicyPaymentGrid',
        editFormSelector: '#belayPolicyPaymentEditWindow',
        storeName: 'belaypolicy.Payment',
        modelName: 'belaypolicy.Payment',
        editWindowView: 'belaypolicy.PaymentEditWindow',
        listeners: {
            getdata: function (asp, record) {
                if (!record.get('Id')) {
                    record.set('BelayPolicy', this.controller.params.get('Id'));
                }
            }
        }
    }],

    mainView: 'belaypolicy.PaymentGrid',
    mainViewSelector: '#belayPolicyPaymentGrid',

    init: function () {
        this.getStore('belaypolicy.Payment').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('belaypolicy.Payment').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.belayPolicyId = this.params.get('Id');
        }
    }
});