Ext.define('B4.controller.documentsgjiregister.WarningDoc', {
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
    
    models: [
        'WarningDoc',
        'InspectionGji'
    ],
    
    stores: ['view.WarningDoc'],

    views: ['documentsgjiregister.WarningDocGrid'],

    mainView: 'documentsgjiregister.WarningDocGrid',
    mainViewSelector: '#docsGjiRegisterWarningDocGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrWarningDocGridEditFormAspect',
            gridSelector: '#docsGjiRegisterWarningDocGrid',
            storeName: 'view.WarningDoc',
            modelName: 'WarningDoc'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'warningGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterWarningDocGrid',
            buttonSelector: '#docsGjiRegisterWarningDocGrid #btnExport',
            controllerName: 'WarningDoc',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterWarningDocStateTransferAspect',
            gridSelector: '#docsGjiRegisterWarningDocGrid',
            stateType: 'gji_document_warning',
            menuSelector: 'docsGjiRegisterWarningDocGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('WarningDoc');
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
        this.getStore('view.WarningDoc').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.WarningDoc').load();
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