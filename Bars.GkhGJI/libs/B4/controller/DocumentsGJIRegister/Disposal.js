﻿Ext.define('B4.controller.documentsgjiregister.Disposal', {
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
        'Disposal',
        'InspectionGji'
    ],
    
    stores: ['view.Disposal'],

    views: ['documentsgjiregister.DisposalGrid'],

    mainView: 'documentsgjiregister.DisposalGrid',
    mainViewSelector: '#docsGjiRegisterDisposalGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrDisposalGridEditFormAspect',
            gridSelector: '#docsGjiRegisterDisposalGrid',
            storeName: 'view.Disposal',
            modelName: 'Disposal'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'disposalGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterDisposalGrid',
            buttonSelector: '#docsGjiRegisterDisposalGrid #btnExport',
            controllerName: 'Disposal',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterDisposalStateTransferAspect',
            gridSelector: '#docsGjiRegisterDisposalGrid',
            stateType: 'gji_document_disp',
            menuSelector: 'docsGjiRegisterDisposalGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Disposal.Disposal');
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
        this.getStore('view.Disposal').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.Disposal').load();
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