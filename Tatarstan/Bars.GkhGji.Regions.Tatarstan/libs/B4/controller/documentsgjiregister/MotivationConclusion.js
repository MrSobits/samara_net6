Ext.define('B4.controller.documentsgjiregister.MotivationConclusion', {
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
        'MotivationConclusion',
        'InspectionGji'
    ],
    
    stores: ['view.MotivationConclusion'],

    views: ['documentsgjiregister.MotivationConclusionGrid'],

    mainView: 'documentsgjiregister.MotivationConclusionGrid',
    mainViewSelector: '#docsGjiRegisterMotivationConclusionGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrMotivationConclusionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterMotivationConclusionGrid',
            storeName: 'view.MotivationConclusion',
            modelName: 'MotivationConclusion'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'warningGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterMotivationConclusionGrid',
            buttonSelector: '#docsGjiRegisterMotivationConclusionGrid #btnExport',
            controllerName: 'MotivationConclusion',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterMotivationConclusionStateTransferAspect',
            gridSelector: '#docsGjiRegisterMotivationConclusionGrid',
            stateType: 'gji_document_motivationconclusion',
            menuSelector: 'docsGjiRegisterMotivationConclusionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('MotivationConclusion');
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
        this.getStore('view.MotivationConclusion').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.MotivationConclusion').load();
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