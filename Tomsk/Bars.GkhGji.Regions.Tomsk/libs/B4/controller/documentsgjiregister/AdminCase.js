Ext.define('B4.controller.documentsgjiregister.AdminCase', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateContextMenu'
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },
    
    models: [
        'AdminCase',
        'InspectionGji'
    ],
    
    stores: ['view.AdminCase'],

    views: ['documentsgjiregister.AdminCaseGrid'],

    mainView: 'documentsgjiregister.AdminCaseGrid',
    mainViewSelector: '#docsGjiRegisterAdminCaseGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrAdminCaseGridEditFormAspect',
            gridSelector: '#docsGjiRegisterAdminCaseGrid',
            modelName: 'AdminCase',
            storeName: 'view.AdminCase'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'adminCaseButtonExportAspect',
            gridSelector: 'registeradmincasegrid',
            buttonSelector: 'registeradmincasegrid #btnExport',
            controllerName: 'AdministrativeCase',
            actionName: 'Export'
        },
        {
            /*
            * Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterAdminCaseStateTransferAspect',
            gridSelector: 'registeradmincasegrid',
            stateType: 'gji_document_admincase',
            menuSelector: 'docsGjiRegisterActVisualGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('AdminCase');
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
        this.getStore('view.AdminCase').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.AdminCase').load();
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