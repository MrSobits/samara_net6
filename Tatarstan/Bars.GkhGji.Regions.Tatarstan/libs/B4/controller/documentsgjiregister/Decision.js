Ext.define('B4.controller.documentsgjiregister.Decision', {
    extend: 'B4.controller.documentsgjiregister.TatDisposal',

    models: ['Decision'],
    stores: ['view.Decision'],
    views: ['documentsgjiregister.DecisionGrid'],
    mainView: 'documentsgjiregister.DecisionGrid',
    mainViewSelector: '#docsGjiRegisterDecisionGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrDecisionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterDecisionGrid',
            storeName: 'view.Decision',
            modelName: 'Decision'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'disposalGjiButtonExportAspect',
            gridSelector: '#docsGjiRegisterDecisionGrid',
            buttonSelector: '#docsGjiRegisterDecisionGrid #btnExport',
            controllerName: 'TatarstanDecision',
            actionName: 'Export'
        },
        {
            /*
            Вешаем аспект смены статуса
            */
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterDisposalStateTransferAspect',
            gridSelector: '#docsGjiRegisterDecisionGrid',
            stateType: 'gji_document_disp',
            menuSelector: 'docsGjiRegisterDecisionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //Потому что они могли изменится
                    var model = asp.controller.getModel('Decision.Decision');
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
        this.getStore('view.Decision').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('view.Decision').load();
    }
});