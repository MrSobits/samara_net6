Ext.define('B4.controller.documentsgjiregister.ResolutionRospotrebnadzor', {
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
    stores: ['view.ResolutionRospotrebnadzor'],

    views: ['documentsgjiregister.ResolutionRospotrebnadzorGrid'],

    mainView: 'documentsgjiregister.ResolutionRospotrebnadzorGrid',
    mainViewSelector: '#docsGjiRegisterResolutionRospotrebnadzorGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrResolutionRospotrebnadzorGridEditFormAspect',
            gridSelector: '#docsGjiRegisterResolutionRospotrebnadzorGrid',
            modelName: 'ResolutionRospotrebnadzor',
            storeName: 'view.ResolutionRospotrebnadzor'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'resolutionButtonExportAspect',
            gridSelector: '#docsGjiRegisterResolutionRospotrebnadzorGrid',
            buttonSelector: '#docsGjiRegisterResolutionRospotrebnadzorGrid #btnExport',
            controllerName: 'ResolutionRospotrebnadzor',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterResolutionRospotrebnadzorStateTransferAspect',
            gridSelector: '#docsGjiRegisterResolutionRospotrebnadzorGrid',
            stateType: 'gji_document_resol_rosp',
            menuSelector: 'docsGjiRegisterResolutionRospotrebnadzorGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ResolutionRospotrebnadzor');
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
        this.getStore('view.ResolutionRospotrebnadzor').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ResolutionRospotrebnadzor').load();
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