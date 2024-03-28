Ext.define('B4.controller.documentsgjiregister.ProtocolGji', {
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
        'ProtocolGji',
        'InspectionGji'
    ],
    stores: ['view.ProtocolGji'],

    views: ['documentsgjiregister.ProtocolGrid'],

    mainView: 'documentsgjiregister.ProtocolGrid',
    mainViewSelector: '#docsGjiRegisterProtocolGrid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocolButtonExportAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            buttonSelector: '#docsGjiRegisterProtocolGrid #btnExport',
            controllerName: 'Protocol',
            actionName: 'Export'
        },
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrProtocolGridEditFormAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            modelName: 'ProtocolGji',
            storeName: 'view.ProtocolGji'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterProtocolStateTransferAspect',
            gridSelector: '#docsGjiRegisterProtocolGrid',
            stateType: 'gji_document_prot',
            menuSelector: 'docsGjiRegisterProtocolGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ProtocolGji');
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
        this.getStore('view.ProtocolGji').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ProtocolGji').load();
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