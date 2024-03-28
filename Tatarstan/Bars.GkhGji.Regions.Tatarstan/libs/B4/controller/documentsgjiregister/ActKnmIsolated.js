Ext.define('B4.controller.documentsgjiregister.ActKnmIsolated', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateContextMenu',
        'B4.aspects.GkhCtxButtonDataExport'
    ],

    models: [
        'documentsgjiregister.ActKnmIsolated'
    ],

    stores: [
        'documentsgjiregister.ActKnmIsolated'
    ],

    views: [
        'documentsgjiregister.ActKnmIsolatedGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'documentsgjiregister.ActKnmIsolatedGrid',
    mainViewSelector: '#docsGjiRegisterActKnmIsolatedGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActKnmIsolatedGridEditFormAspect',
            gridSelector: '#docsGjiRegisterActKnmIsolatedGrid',
            modelName: 'documentsgjiregister.ActKnmIsolated',
            storeName: 'documentsgjiregister.ActKnmIsolated'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterDisposalStateTransferAspect',
            gridSelector: '#docsGjiRegisterActKnmIsolatedGrid',
            stateType: 'gji_document_disp',
            menuSelector: 'docsGjiRegisterActKnmGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('documentsgjiregister.ActKnmIsolated');
                    model.load(record.getId(), {
                        success: function (rec) {
                            record.set('DocumentNumber', rec.get('DocumentNumber'));
                        },
                        scope: this
                    });
                }
            }
        },
        {
            xtype: 'gkhctxbuttondataexportaspect',
            name: 'ActKnmIsolatedGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterActKnmIsolatedGrid',
            buttonSelector: '#docsGjiRegisterActKnmIsolatedGrid #btnExport',
            controllerName: 'ActActionIsolated',
            actionName: 'Export'
        },
    ],

    init: function () {
        this.getStore('documentsgjiregister.ActKnmIsolated').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('documentsgjiregister.ActKnmIsolated').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {

            if (this.params.filterParams.realityObjectId)
                operation.params.realityObjectId = this.params.filterParams.realityObjectId;

            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
        }
    }
});