Ext.define('B4.controller.documentsgjiregister.Resolution', {
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
        'Resolution',
        'InspectionGji'
    ],
    stores: ['view.Resolution'],

    views: ['documentsgjiregister.ResolutionGrid'],

    mainView: 'documentsgjiregister.ResolutionGrid',
    mainViewSelector: '#docsGjiRegisterResolutionGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrResolutionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            modelName: 'Resolution',
            storeName: 'view.Resolution'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'resolutionButtonExportAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            buttonSelector: '#docsGjiRegisterResolutionGrid #btnExport',
            controllerName: 'Resolution',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterResolutionStateTransferAspect',
            gridSelector: '#docsGjiRegisterResolutionGrid',
            stateType: 'gji_document_resol',
            menuSelector: 'docsGjiRegisterResolutionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Resolution');
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
        this.getStore('view.Resolution').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.Resolution').load();
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