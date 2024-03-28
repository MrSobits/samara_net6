Ext.define('B4.controller.LongTermPrObject', {
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.GkhGridEditForm',
       'B4.aspects.permission.RealityObject'
    ],

    models: ['LongTermPrObject'],
    stores: ['LongTermPrObject'],

    views: [
        'longtermprobject.Grid',
        'longtermprobject.AddWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        mixins: 'B4.mixins.Context'
    },

    mainView: 'longtermprobject.Grid',
    mainViewSelector: 'longtermprobjectgrid',
    refs: [
        {
            ref: 'mainView',
            selector: 'longtermprobjectgrid'
        }
    ],
    aspects: [
        {
            xtype: 'realityobjperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'LongTermPrObjectGridWindowAspect',
            gridSelector: 'longtermprobjectgrid',
            editFormSelector: '#LongTermPrObjectAddWindow',
            storeName: 'LongTermPrObject',
            modelName: 'LongTermPrObject',
            editWindowView: 'longtermprobject.AddWindow',
            controllerEditName: 'B4.controller.longtermprobject.Navigation'
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('longtermprobjectgrid'),
            store = view.getStore();
        this.bindContext(view);
        this.application.deployView(view);

        store.on('beforeload', this.onBeforeLoad, this);
        store.load();
    },
    
    onBeforeLoad: function (store, operation) {
        var mainView = this.getMainView();
        operation.params.showDemolished = mainView.down('#cbShowDemolished').getValue();
        operation.params.showEmergency = mainView.down('#cbShowEmergency').getValue();
    },
    
    loadStore: function () {
        this.getMainView().getStore().load();
    },

    init: function () {

        this.control({
            'longtermprobjectgrid #cbShowDemolished': {
                change: {
                    fn: this.loadStore,
                    scope: this
                }
            },
            'longtermprobjectgrid #cbShowEmergency': {
                change: {
                    fn: this.loadStore,
                    scope: this
                }
            }
        });

        this.callParent(arguments);
    }
});