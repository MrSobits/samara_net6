Ext.define('B4.controller.documentsgjiregister.ActIsolated', {
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
        'ActIsolated',
        'InspectionGji'
    ],
    stores: ['view.ActIsolated'],

    views: ['documentsgjiregister.ActIsolatedGrid'],

    mainView: 'documentsgjiregister.ActIsolatedGrid',
    mainViewSelector: 'docsgjiregisteractisolatedgrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActIsolatedGridEditFormAspect',
            gridSelector: 'docsgjiregisteractisolatedgrid',
            modelName: 'ActIsolated',
            storeName: 'view.ActIsolated'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actIsolatedButtonExportAspect',
            gridSelector: 'docsgjiregisteractisolatedgrid',
            buttonSelector: 'docsgjiregisteractisolatedgrid #btnExport',
            controllerName: 'ActIsolated',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterActIsolatedStateTransferAspect',
            gridSelector: 'docsgjiregisteractisolatedgrid',
            stateType: 'gji_document_actisolated',
            menuSelector: 'docsGjiRegisterActIsolatedGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ActIsolated');
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
        this.getStore('view.ActIsolated').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ActIsolated').load();
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