Ext.define('B4.controller.documentsgjiregister.PreventiveVisit', {
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
        'PreventiveVisit',
        'InspectionGji'
    ],

    stores: ['PreventiveVisit'],

    views: ['documentsgjiregister.PreventiveVisitGrid'],

    mainView: 'documentsgjiregister.PreventiveVisitGrid',
    mainViewSelector: '#docsGjiRegisterPreventiveVisitGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegisterPreventiveVisitGridEditFormAspect',
            gridSelector: '#docsGjiRegisterPreventiveVisitGrid',
            storeName: 'PreventiveVisit',
            modelName: 'PreventiveVisit'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'preventiveVisitButtonExportAspect',
            gridSelector: '#docsGjiRegisterPreventiveVisitGrid',
            buttonSelector: '#docsGjiRegisterPreventiveVisitGrid #btnExport',
            controllerName: 'PreventiveVisit',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterPreventiveVisitStateTransferAspect',
            gridSelector: '#docsGjiRegisterPreventiveVisitGrid',
            stateType: 'gji_document_prevvisit',
            menuSelector: 'docsGjiRegisterPreventiveVisitGridStateMenu'
            //listeners: {
            //    transfersuccess: function (asp, record) {
            //        //После успешной смены статуса запрашиваем по Id актуальные данные записи
            //        //Потому что они могли изменится
            //        var model = asp.controller.getModel('Disposal.Disposal');
            //        model.load(record.getId(), {
            //            success: function (rec) {
            //                record.set('DocumentNumber', rec.get('DocumentNumber'));
            //            },
            //            scope: this
            //        });
            //    }
            //}
        }
    ],

    init: function () {
        this.getStore('PreventiveVisit').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('PreventiveVisit').load();
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