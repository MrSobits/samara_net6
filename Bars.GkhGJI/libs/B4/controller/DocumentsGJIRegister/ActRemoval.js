Ext.define('B4.controller.documentsgjiregister.ActRemoval', {
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
        'ActRemoval',
        'InspectionGji'
    ],
    stores: ['view.ActRemoval'],

    views: ['documentsgjiregister.ActRemovalGrid'],

    mainView: 'documentsgjiregister.ActRemovalGrid',
    mainViewSelector: '#docsGjiRegisterActRemovalGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActRemovalGridEditFormAspect',
            gridSelector: '#docsGjiRegisterActRemovalGrid',
            modelName: 'ActRemoval',
            storeName: 'view.ActRemoval'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actRemovalGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterActRemovalGrid',
            buttonSelector: '#docsGjiRegisterActRemovalGrid #btnExport',
            controllerName: 'ActRemoval',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterActRemovalStateTransferAspect',
            gridSelector: '#docsGjiRegisterActRemovalGrid',
            stateType: 'gji_document_actrem',
            menuSelector: 'docsGjiRegisterActRemovalGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ActRemoval');
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
        this.getStore('view.ActRemoval').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ActRemoval').load();
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