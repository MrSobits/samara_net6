Ext.define('B4.controller.LongTermPrObject', {
    /*
     */
    extend: 'B4.base.Controller',

    requires: [
       'B4.aspects.GkhGridEditForm',
       'B4.aspects.ButtonDataExport',
       'B4.aspects.permission.RealityObject',
       'B4.aspects.StateContextMenu'
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
        { ref: 'mainPanel', selector: 'longtermprobjectgrid' }
    ],
    aspects: [
        {
            xtype: 'realityobjperm'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'LongTermPrObjectStateTransferAspect',
            gridSelector: 'longtermprobjectgrid',
            menuSelector: 'LongTermPrObjectGridStateMenu',
            stateType: 'gkh_real_obj'
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
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'LongTermPrObjectButtonExportAspect',
            gridSelector: 'longtermprobjectgrid',
            buttonSelector: 'longtermprobjectgrid #btnExport',
            controllerName: 'LongTermPrObject',
            actionName: 'Export'
        }
    ],

    index: function () {
        var view = this.getMainPanel() || Ext.widget('longtermprobjectgrid'),
            store = view.getStore();
        this.bindContext(view);
        this.application.deployView(view);
        
        store.on('beforeload', this.onBeforeLoad, this);
        store.load();
    },
    
    onBeforeLoad: function (store, operation) {
        var mw = this.getMainPanel();

        operation.params.showDemolished = mw.down('#cbShowDemolished').getValue();
        operation.params.showEmergency = mw.down('#cbShowEmergency').getValue();
    },
    
    loadStore: function() {
        this.getMainPanel().getStore().load();
    },

    init: function() {

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