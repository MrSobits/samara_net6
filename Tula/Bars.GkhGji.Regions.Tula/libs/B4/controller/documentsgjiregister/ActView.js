Ext.define('B4.controller.documentsgjiregister.ActView', {
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
        'ActCheck',
        'InspectionGji'
    ],
    stores: ['view.ActView'],

    views: ['documentsgjiregister.ActViewGrid'],

    mainView: 'documentsgjiregister.ActViewGrid',
    mainViewSelector: 'docsgjiregisteractviewgrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrActCheckGridEditFormAspect',
            gridSelector: 'docsgjiregisteractviewgrid',
            modelName: 'ActCheck',
            storeName: 'view.ActView'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'actCheckButtonExportAspect',
            gridSelector: 'docsgjiregisteractviewgrid',
            buttonSelector: 'docsgjiregisteractviewgrid #btnExport',
            controllerName: 'ActCheck',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterActCheckStateTransferAspect',
            gridSelector: 'docsgjiregisteractviewgrid',
            stateType: 'gji_document_actcheck',
            menuSelector: 'docsGjiRegisterActCheckGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('ActCheck');
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
        this.getStore('view.ActView').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.ActView').load();
    },
    
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            
            if (this.params.filterParams.realityObjectId)
                operation.params.realityObjectId = this.params.filterParams.realityObjectId;

            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
            
            operation.params.isActView = true;
        }
    }
});