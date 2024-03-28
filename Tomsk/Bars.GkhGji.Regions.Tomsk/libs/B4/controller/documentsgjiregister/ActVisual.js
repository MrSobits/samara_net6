Ext.define('B4.controller.documentsgjiregister.ActVisual', {
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
        'ActVisual',
        'InspectionGji'
    ],
    stores: ['view.ActVisual'],

    views: ['documentsgjiregister.ActVisualGrid'],

    mainView: 'documentsgjiregister.ActVisualGrid',
    mainViewSelector: '#docsGjiRegisterActVisualGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActVisualGridEditFormAspect',
            gridSelector: '#docsGjiRegisterActVisualGrid',
            modelName: 'ActVisual',
            storeName: 'view.ActVisual'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actVisualButtonExportAspect',
            gridSelector: '#docsGjiRegisterActVisualGrid',
            buttonSelector: '#docsGjiRegisterActVisualGrid #btnExport',
            controllerName: 'ActVisual',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterActVisualStateTransferAspect',
            gridSelector: '#docsGjiRegisterActVisualGrid',
            stateType: 'gji_document_actvisual',
            menuSelector: 'docsGjiRegisterActVisualGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ActVisual');
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
        this.getStore('view.ActVisual').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ActVisual').load();
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