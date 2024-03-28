Ext.define('B4.controller.documentsgjiregister.MotivatedPresentation', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
    ],

    models: [
        'documentsgjiregister.MotivatedPresentation'
    ],

    stores: [
        'documentsgjiregister.MotivatedPresentation'
    ],

    views: [
        'documentsgjiregister.MotivatedPresentationGrid'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'documentsgjiregister.MotivatedPresentationGrid',
    mainViewSelector: 'docsgjiregistermotivatedpresentationgrid',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'motivatedPresentationGjiButtonExportAspect',
            gridSelector: 'docsgjiregistermotivatedpresentationgrid',
            buttonSelector: 'docsgjiregistermotivatedpresentationgrid #btnExport',
            controllerName: 'MotivatedPresentation',
            actionName: 'Export'
        },
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrMotivatedPresentationGridEditFormAspect',
            gridSelector: 'docsgjiregistermotivatedpresentationgrid',
            modelName: 'documentsgjiregister.MotivatedPresentation',
            storeName: 'documentsgjiregister.MotivatedPresentation'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterMotivatedPresentationAspect',
            gridSelector: 'docsgjiregistermotivatedpresentationgrid',
            stateType: 'gji_document_disp',
            menuSelector: 'docsGjiRegisterActKnmGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('documentsgjiregister.MotivatedPresentation');
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
        this.getStore('documentsgjiregister.MotivatedPresentation').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('documentsgjiregister.MotivatedPresentation').load();
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