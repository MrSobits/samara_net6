Ext.define('B4.controller.documentsgjiregister.Presentation', {
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
        'Presentation',
        'InspectionGji'
    ],
    stores: ['Presentation'],

    views: ['documentsgjiregister.PresentationGrid'],

    mainView: 'documentsgjiregister.PresentationGrid',
    mainViewSelector: '#docsGjiRegisterPresentationGrid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'presentationButtonExportAspect',
            gridSelector: '#docsGjiRegisterPresentationGrid',
            buttonSelector: '#docsGjiRegisterPresentationGrid #btnExport',
            controllerName: 'Presentation',
            actionName: 'Export'
        },        
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrPresentationGridEditFormAspect',
            gridSelector: '#docsGjiRegisterPresentationGrid',
            modelName: 'Presentation',
            storeName: 'Presentation'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterPresentStateTransferAspect',
            gridSelector: '#docsGjiRegisterPresentationGrid',
            stateType: 'gji_document_prot',
            menuSelector: 'docsGjiRegisterPresentationGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Presentation');
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
        this.getStore('Presentation').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('Presentation').load();
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