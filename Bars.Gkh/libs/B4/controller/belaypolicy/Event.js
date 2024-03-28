Ext.define('B4.controller.belaypolicy.Event', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GridEditWindow'],

    models: ['belaypolicy.Event'],
    stores: ['belaypolicy.Event'],
    views: [
        'belaypolicy.EventEditWindow',
        'belaypolicy.EventGrid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'eventGridWindowAspect',
            gridSelector: '#belayPolicyEventGrid',
            editFormSelector: '#belayPolicyEventEditWindow',
            storeName: 'belaypolicy.Event',
            modelName: 'belaypolicy.Event',
            editWindowView: 'belaypolicy.EventEditWindow',
            listeners: {
                getdata: function(asp, record) {
                    if (!record.get('Id')) {
                        record.set('BelayPolicy', this.controller.params.get('Id'));
                    }
                }
            }
        }
    ],

    mainView: 'belaypolicy.EventGrid',
    mainViewSelector: '#belayPolicyEventGrid',

    init: function() {
        this.getStore('belaypolicy.Event').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function() {
        this.getStore('belaypolicy.Event').load();
    },

    onBeforeLoad: function(store, operation) {
        if (this.params) {
            operation.params.belayPolicyId = this.params.get('Id');
        }
    }
});