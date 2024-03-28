Ext.define('B4.controller.documentsgjiregister.TaskAction', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    models: ['actionisolated.TaskAction'],

    stores: ['actionisolated.DocumentRegistryTaskAction'],

    views: ['documentsgjiregister.TaskActionGrid'],

    mainView: 'documentsgjiregister.TaskActionGrid',
    mainViewSelector: '#taskActionIsolatedGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrTaskActionGridEditFormAspect',
            gridSelector: '#taskActionIsolatedGrid',
            storeName: 'actionisolated.DocumentRegistryTaskAction',
            modelName: 'actionisolated.TaskAction'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'warningGjiButtonExportAspect',
            gridSelector: '#taskActionIsolatedGrid',
            buttonSelector: '#taskActionIsolatedGrid #btnExport',
            controllerName: 'TaskActionIsolated',
            actionName: 'Export'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegistertaskActionIsolatedStateTransferAspect',
            gridSelector: '#taskActionIsolatedGrid',
            stateType: 'gji_document_task_actionisolated',
            menuSelector: 'docsGjiRegistertaskActionIsolatedGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('actionisolated.TaskAction');
                    
                    model.load(record.getId(), {
                        success: function (rec) {
                            record.set('DocumentNumber', rec.get('DocumentNumber'));
                        },
                        scope: this
                    });
                }
            }
        }
    ],

    init: function () {
        this.getStore('actionisolated.DocumentRegistryTaskAction').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('actionisolated.DocumentRegistryTaskAction').load();
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