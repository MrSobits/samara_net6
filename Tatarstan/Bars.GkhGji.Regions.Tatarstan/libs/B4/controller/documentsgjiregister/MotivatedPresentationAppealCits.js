Ext.define('B4.controller.documentsgjiregister.MotivatedPresentationAppealCits', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    models: [ 'MotivatedPresentationAppealCits' ],

    stores: ['view.MotivatedPresentationAppealCits'],

    views: ['documentsgjiregister.MotivatedPresentationAppealCitsGrid'],

    mainView: 'documentsgjiregister.MotivatedPresentationAppealCitsGrid',
    mainViewSelector: '#docsGjiRegisterMotivatedPresentationAppealCitsGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegisterMotivPresentAppealCitsGridEditFormAspect',
            gridSelector: '#docsGjiRegisterMotivatedPresentationAppealCitsGrid',
            storeName: 'view.MotivatedPresentationAppealCits',
            modelName: 'MotivatedPresentationAppealCits'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'motivatedPresAppealCitsGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterMotivatedPresentationAppealCitsGrid',
            buttonSelector: '#docsGjiRegisterMotivatedPresentationAppealCitsGrid #btnExport',
            controllerName: 'MotivatedPresentationAppealCits',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterMotivatedPresAppealCitsStateTransferAspect',
            gridSelector: '#docsGjiRegisterMotivatedPresentationAppealCitsGrid',
            stateType: 'gji_document_motivatedpresentation_appealcits',
            menuSelector: 'docsGjiRegisterMotivatedPresentationAppealCitsGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('MotivatedPresentationAppealCits');
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
        this.getStore('view.MotivatedPresentationAppealCits').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.MotivatedPresentationAppealCits').load();
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