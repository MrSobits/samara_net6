Ext.define('B4.controller.documentsgjiregister.VisitSheet', {
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

    models: [ 'VisitSheet' ],

    stores: ['view.VisitSheet'],

    views: ['documentsgjiregister.VisitSheetGrid'],

    mainView: 'documentsgjiregister.VisitSheetGrid',
    mainViewSelector: '#docsGjiRegisterVisitSheetGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrVisitSheetGridEditFormAspect',
            gridSelector: '#docsGjiRegisterVisitSheetGrid',
            storeName: 'view.VisitSheet',
            modelName: 'VisitSheet'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'visitSheetGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterVisitSheetGrid',
            buttonSelector: '#docsGjiRegisterVisitSheetGrid #btnExport',
            controllerName: 'VisitSheet',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterVisitSheetStateTransferAspect',
            gridSelector: '#docsGjiRegisterVisitSheetGrid',
            stateType: 'gji_document_visit_sheet',
            menuSelector: 'docsGjiRegisterVisitSheetGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('VisitSheet');
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
        this.getStore('view.VisitSheet').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.VisitSheet').load();
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