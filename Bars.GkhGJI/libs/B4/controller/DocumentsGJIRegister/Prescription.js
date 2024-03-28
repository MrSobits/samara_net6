Ext.define('B4.controller.documentsgjiregister.Prescription', {
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
        'Prescription',
        'InspectionGji'
    ],
    stores: ['view.Prescription'],

    views: ['documentsgjiregister.PrescriptionGrid'],

    mainView: 'documentsgjiregister.PrescriptionGrid',
    mainViewSelector: '#docsGjiRegisterPrescriptionGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegisterPrescriptionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterPrescriptionGrid',
            modelName: 'Prescription',
            storeName: 'view.Prescription',
            otherActions: function (actions) {
                actions[this.gridSelector + ' #showYellow'] = { 'change': { fn: this.onShowYellow, scope: this } };   
                actions[this.gridSelector + ' #showRed'] = { 'change': { fn: this.onshowRed, scope: this } }; 
            },
            onShowYellow: function (cb, checked) {
                debugger;
                var me = this;
                if (me.controller.params) {
                    me.controller.params.filterParams.showYellow = checked;
                }
                var grid = cb.up(me.gridSelector);
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
            onshowRed: function (cb, checked) {
                debugger;
                var me = this;
                if (me.controller.params) {
                    me.controller.params.filterParams.showRed = checked;
                }
                var grid = cb.up(me.gridSelector);
                store = grid.getStore();
                store.filter('showSysFiles', checked);
            },
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'docsGjiRegisterPrescriptionButtonExportAspect',
            gridSelector: '#docsGjiRegisterPrescriptionGrid',
            buttonSelector: '#docsGjiRegisterPrescriptionGrid #btnExport',
            controllerName: 'Prescription',
            actionName: 'Export'
        },
        {
            /* Вешаем аспект смены статуса */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterPrescriptionStateTransferAspect',
            gridSelector: '#docsGjiRegisterPrescriptionGrid',
            stateType: 'gji_document_prescr',
            menuSelector: 'docsGjiRegisterPrescriptionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Prescription.Prescription');
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
        this.getStore('view.Prescription').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.Prescription').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            if (this.params.filterParams.realityObjectId)
                operation.params.realityObjectId = this.params.filterParams.realityObjectId;

            if (this.params.filterParams.showYellow)
                operation.params.showYellow = this.params.filterParams.showYellow;
            else
                operation.params.showYellow = false;
            if (this.params.filterParams.showRed)
                operation.params.showRed = this.params.filterParams.showRed;
            else
                operation.params.showRed = false;

            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
        }
    }
});