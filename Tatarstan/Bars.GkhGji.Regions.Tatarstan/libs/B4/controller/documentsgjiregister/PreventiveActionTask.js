Ext.define('B4.controller.documentsgjiregister.PreventiveActionTask', {
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

    models: ['preventiveaction.PreventiveActionTask'],

    stores: ['preventiveaction.PreventiveActionTask'],

    views: ['documentsgjiregister.PreventiveActionTaskGrid'],

    mainView: 'documentsgjiregister.PreventiveActionTaskGrid',
    mainViewSelector: '#docsGjiRegisterPreventiveActionTaskGrid',

    aspects: [
        {
            xtype: 'gjidocumentregisteraspect',
            name: 'docsGjiRegistrPreventiveActionTaskGridEditFormAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionTaskGrid',
            storeName: 'preventiveaction.PreventiveActionTask',
            modelName: 'preventiveaction.PreventiveActionTask'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'PreventiveActionTaskButtonExportAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionTaskGrid',
            buttonSelector: '#docsGjiRegisterPreventiveActionTaskGrid #btnExport',
            controllerName: 'PreventiveActionTask',
            actionName: 'Export'
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'docsGjiRegisterPreventiveActionTaskStateTransferAspect',
            gridSelector: '#docsGjiRegisterPreventiveActionTaskGrid',
            stateType: 'gji_document_preventive_action_task',
            menuSelector: 'docsGjiRegisterPreventiveActionTaskGridStateMenu',
            listeners: {
                transfersuccess: function (asp, record) {
                    var model = asp.controller.getModel('preventiveaction.PreventiveActionTask');

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
        var me = this;
        
        me.getStore('preventiveaction.PreventiveActionTask').on('beforeload', me.onBeforeLoad, me);

        this.callParent(arguments);
    },

    onLaunch: function () {
        var me = this,
            roFilter = me.getMainView().up('#docsGjiRegisterNavigationPanel').down('#sfRealityObject');
        
        if(roFilter){
            roFilter.hide();
        }
        
        me.getStore('preventiveaction.PreventiveActionTask').load();
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