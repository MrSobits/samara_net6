Ext.define('B4.controller.documentsgjiregister.Protocol197', {
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
        'protocol197.Protocol197',
        'InspectionGji'
    ],
    stores: ['protocol197.Protocol197'],

    views: ['documentsgjiregister.Protocol197Grid'],

    mainView: 'documentsgjiregister.Protocol197Grid',
    mainViewSelector: '#docsGjiRegisterProtocol197Grid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'protocol197ButtonExportAspect',
            gridSelector: '#docsGjiRegisterProtocol197Grid',
            buttonSelector: '#docsGjiRegisterProtocol197Grid #btnExport',
            controllerName: 'Protocol197',
            actionName: 'Export'
        },
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrProtocol197GridEditFormAspect',
            gridSelector: '#docsGjiRegisterProtocol197Grid',
            modelName: 'protocol197.Protocol197',
            storeName: 'protocol197.Protocol197'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterProtocol197StateTransferAspect',
            gridSelector: '#docsGjiRegisterProtocol197Grid',
            stateType: 'gji_document_prot197',
            menuSelector: 'docsGjiRegisterProtocol197GridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('protocol197.Protocol197');
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
        this.getStore('protocol197.Protocol197').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('protocol197.Protocol197').load();
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