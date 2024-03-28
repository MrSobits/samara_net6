Ext.define('B4.controller.inspectionpreventiveaction.InspectionPreventiveAction', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.TypeObjectAction',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.inspectionpreventiveaction.InspectionPreventiveAction'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'inspectionpreventiveaction.InspectionPreventiveAction'
    ],

    stores: [
        'inspectionpreventiveaction.InspectionPreventiveAction'],

    views: [
        'inspectionpreventiveaction.AddWindow',
        'inspectionpreventiveaction.MainPanel',
        'inspectionpreventiveaction.FilterPanel',
        'inspectionpreventiveaction.Grid',
    ],

    mainView: 'inspectionpreventiveaction.MainPanel',
    mainViewSelector: 'inspectionpreventiveactionmainpanel',

    refs: [
        {
            ref: 'InspectionPreventiveActionFilterPanel',
            selector: 'inspectionpreventiveactionfilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'inspectionpreventiveactionperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'inspectionpreventiveactionGridWindowAspect',
            gridSelector: 'inspectionpreventiveactiongrid',
            modelName: 'inspectionpreventiveaction.InspectionPreventiveAction',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.inspectionpreventiveaction.Navigation',
            editWindowView: 'inspectionpreventiveaction.AddWindow',
            editFormSelector: '#inspectionPreventiveActionAddWindow',
            otherActions: function (actions) {
            },
            closeWindowHandler: function () {
                var me = this,
                    window;
                if (me.editFormSelector) {
                    window = me.componentQuery(me.editFormSelector);
                    if (window) {
                        Ext.Msg.confirm('Внимание', 'Вы действительно хотите закрыть окно? Проверка не будет сохранена', function (result) {
                            if (result == 'yes') {
                                window.close();
                            }
                        });
                    }
                }
            },
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'inspectionPreventiveActionStateTransferAspect',
            gridSelector: 'inspectionpreventiveactiongrid',
            menuSelector: 'inspectionpreventiveactionStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'inspectionPreventiveActionButtonExportAspect',
            gridSelector: 'inspectionpreventiveactiongrid',
            buttonSelector: 'inspectionpreventiveactiongrid #btnExport',
            controllerName: 'InspectionPreventiveAction',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'inspectionpreventiveactiongrid checkbox[name=IsClosed]' : { 'change': { fn: me.updateMainGrid, scope: me } },
            'inspectionpreventiveactionfilterpanel [action=updateGrid]': {
                'click': { fn: me.updateMainGrid, scope: me }
            }
        });

        me.getStore('inspectionpreventiveaction.InspectionPreventiveAction').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
        me.updateMainGrid();
    },

    updateMainGrid: function () {
        this.getMainView().down('inspectionpreventiveactiongrid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            filterPanel = me.getInspectionPreventiveActionFilterPanel(),
            isClosed = me.getMainView().down('inspectionpreventiveactiongrid').down('[name=IsClosed]').getValue();

        if (filterPanel) {
            operation.params.realityObjectId = filterPanel.down('[name=RealityObject]').getValue();
            operation.params.dateStart = filterPanel.down('[name=DateStart]').getValue();
            operation.params.dateEnd = filterPanel.down('[name=DateEnd]').getValue();
        }
        operation.params.isClosed = isClosed;
    }
});