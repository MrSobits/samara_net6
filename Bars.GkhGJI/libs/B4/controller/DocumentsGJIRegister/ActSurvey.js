Ext.define('B4.controller.documentsgjiregister.ActSurvey', {
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
        'ActSurvey',
        'InspectionGji'
    ],
    stores: ['view.ActSurvey'],

    views: ['documentsgjiregister.ActSurveyGrid'],

    mainView: 'documentsgjiregister.ActSurveyGrid',
    mainViewSelector: '#docsGjiRegisterActSurveyGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActSurveyGridEditFormAspect',
            gridSelector: '#docsGjiRegisterActSurveyGrid',
            modelName: 'ActSurvey',
            storeName: 'view.ActSurvey'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'builderButtonExportAspect',
            gridSelector: '#docsGjiRegisterActSurveyGrid',
            buttonSelector: '#docsGjiRegisterActSurveyGrid #btnExport',
            controllerName: 'ActSurvey',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterActSurveyStateTransferAspect',
            gridSelector: '#docsGjiRegisterActSurveyGrid',
            stateType: 'gji_document_actsur',
            menuSelector: 'docsGjiRegisterActSurveyGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ActSurvey');
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
        this.getStore('view.ActSurvey').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ActSurvey').load();
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