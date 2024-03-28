Ext.define('B4.controller.inspectionactionisolated.InspectionActionIsolated', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.inspectionactionisolated.InspectionActionIsolated',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.TypeObjectAction',
        'B4.aspects.ButtonDataExport'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'inspectionactionisolated.InspectionActionIsolated'
    ],

    stores: [
        'inspectionactionisolated.InspectionActionIsolated'],

    views: [
        'inspectionactionisolated.MainPanel',
        'inspectionactionisolated.FilterPanel',
        'inspectionactionisolated.Grid',
        'inspectionactionisolated.AddWindow'
    ],

    mainView: 'inspectionactionisolated.MainPanel',
    mainViewSelector: 'inspectionactionisolatedmainpanel',

    refs: [
        {
            ref: 'InspectionActionIsolatedFilterPanel',
            selector: 'inspectionactionisolatedfilterpanel'
        }
    ],

    aspects: [
        {
            xtype: 'inspectionactionisolatedperm'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'inspectionactionisolatedGridWindowAspect',
            gridSelector: 'inspectionactionisolatedgrid',
            modelName: 'inspectionactionisolated.InspectionActionIsolated',
            deleteWithRelatedEntities: true,
            controllerEditName: 'B4.controller.inspectionactionisolated.Navigation',
            editWindowView: 'inspectionactionisolated.AddWindow',
            editFormSelector: '#inspectionactionisolatedaddwindow',
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
            name: 'inspectionactionisolatedStateTransferAspect',
            gridSelector: 'inspectionactionisolatedgrid',
            menuSelector: 'inspectionactionisolatedStateMenu',
            stateType: 'gji_inspection'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'inspectionActionIsolatedButtonExportAspect',
            gridSelector: 'inspectionactionisolatedgrid',
            buttonSelector: 'inspectionactionisolatedgrid #btnExport',
            controllerName: 'InspectionActionIsolated',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'inspectionactionisolatedgrid': { 'inspectionactionisolated.beforeload': { fn: me.onBeforeLoad, scope: me } },
            'inspectionactionisolatedgrid checkbox[name=IsClosed]' : { 'change': { fn: me.updateMainGrid, scope: me } },
            'inspectionactionisolatedfilterpanel [action=updateGrid]': {
                'click': { fn: me.updateMainGrid, scope: me }
            }
        });

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
        this.getMainView().down('inspectionactionisolatedgrid').getStore().load();
    },

    onBeforeLoad: function (store, operation) {
        var me = this,
            filterPanel = me.getInspectionActionIsolatedFilterPanel(),
            isClosed = me.getMainView().down('inspectionactionisolatedgrid').down('[name=IsClosed]').getValue();

        if (filterPanel) {
            operation.params.realityObjectId = filterPanel.down('[name=RealityObject]').getValue();
            operation.params.dateStart = filterPanel.down('[name=DateStart]').getValue();
            operation.params.dateEnd = filterPanel.down('[name=DateEnd]').getValue();
        }
        operation.params.isClosed = isClosed;
    }
});