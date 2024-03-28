Ext.define('B4.controller.documentsgjiregister.PreventiveAction', {
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

    models: ['preventiveaction.PreventiveAction'],

    stores: ['preventiveaction.DocumentRegistryPreventiveAction'],

    views: ['documentsgjiregister.PreventiveActionGrid'],

    mainView: 'documentsgjiregister.PreventiveActionGrid',
    mainViewSelector: '#docsGjiRegisterPreventiveActionGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegisterPreventiveActionGridEditFormAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionGrid',
            storeName: 'preventiveaction.DocumentRegistryPreventiveAction',
            modelName: 'preventiveaction.PreventiveAction'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'docsGjiRegisterPreventiveActionButtonExportAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionGrid',
            buttonSelector: '#docsGjiRegisterPreventiveActionGrid #btnExport',
            controllerName: 'PreventiveAction',
            actionName: 'Export'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterPreventiveActionStateTransferAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionGrid',
            stateType: 'gji_document_preventive_action',
            menuSelector: 'docsGjiRegisterPreventiveActionGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('preventiveaction.PreventiveAction');

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
        this.getStore('preventiveaction.DocumentRegistryPreventiveAction').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            mainView = me.getMainView(),
            navigationPanel = mainView.up('#docsGjiRegisterNavigationPanel'),
            sfRealityObject = navigationPanel.down('#sfRealityObject');

        if (sfRealityObject) {
            sfRealityObject.hide();
        }

        this.getStore('preventiveaction.DocumentRegistryPreventiveAction').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            if (this.params.filterParams.dateStart)
                operation.params.dateStart = this.params.filterParams.dateStart;

            if (this.params.filterParams.dateEnd)
                operation.params.dateEnd = this.params.filterParams.dateEnd;
        }
    }
});