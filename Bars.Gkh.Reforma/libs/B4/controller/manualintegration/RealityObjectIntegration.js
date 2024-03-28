Ext.define('B4.controller.manualintegration.RealityObjectIntegration',
{
    extend: 'B4.base.Controller',

    requires: [
        'B4.Url',
        'B4.Ajax',
        'B4.aspects.GkhGridMultiSelectWindow'
    ],

    stores: [
        'manualintegration.RefRealityObject',
        'manualintegration.RefRealityObjectSelected'
    ],
    views: [
        'SelectWindow.MultiSelectWindow',
        'manualintegration.RobjectEditPanel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'manualintegration.RobjectEditPanel',
    mainViewSelector: '#manualintegrationRobjectPanel',

    aspects: [],
    onLaunch: function () {
        var window = this.getMainView().up();

        this.getMainView().storeSelect.load();

        window.setWidth(1000);
        window.center();
    },
    init: function() {
        var me = this,
            actions = {};

        actions['#manualintegrationRobjectPanel #btnSelectAll'] = { 'click': { fn: this.btnIntegrateClick, scope: this } };
        actions['#manualintegrationRobjectPanel #btnIntegrate'] = { 'click': { fn: this.btnIntegrateClick, scope: this } };

        actions['#realityObjIntegrationGrid'] = {
            'select': { fn: me.onRowSelect, scope: me },
            'deselect': { fn: me.onRowDeselect, scope: me }
        };
        actions['#manualintegrationRobjectPanel'] = {
            'storeSelect.beforeload': {
                fn: function(store, operation) {
                    operation.params.disclosureInfoId = me.params.disclosureInfoId;
                },
                scope: this
            },
            'selectedgridrowactionhandler':
            {
                fn: this.onRowDelete,
                scope: this
            }
        };

        me.control(actions);
        me.callParent(arguments);
    },
    btnClearClick: function () {
        this.getStore('manualintegration.RefRealityObjectSelected').removeAll();
    },
    btnIntegrateClick: function (btn) {
        var me = this,
            storeData = me.getMainView().storeSelected.data.items,
            refRoIds = Ext.JSON.encode(Ext.Array.map(storeData, function (el) { return el.get('Id') })),
            window = me.getMainView().up();

        if (btn.action === 'integrateAll') {
            Ext.Msg.confirm('Внимание',
                'Вы действительно хотите провести интеграцию с системой Реформа ЖКХ по всем домам, находящимся в управлении данной управляющей организации?',
                function(btnId) {
                    if (btnId === 'yes') {
                        me.doIntegration(refRoIds, window);
                    } else {
                        window.close();
                    }
                });
        } else if (storeData.length === 0 && btn.itemId === 'btnIntegrate') {
            Ext.Msg.alert('Ошибка!', 'Выберите хотя бы один дом');
            return;
        } else {
            me.doIntegration(refRoIds, window);
        }
    },
    doIntegration: function (refRoIds, window) {
        me.controller.mask('Загрузка', me.controller.getMainView());
        window.close();

        B4.Ajax.request(B4.Url.action('ScheduleRobjectIntegrationTask', 'ManualIntegration',
                {
                    disclosureInfoId: me.controller.disclosureInfoId,
                    refRoIds: refRoIds,
                    manOrgId: me.controller.manOrgId
                }))
            .next(function() {
                Ext.Msg.alert('Выборочная интеграция', 'Задача успешно поставлена в очередь');
                me.controller.unmask();
                window.close();
            })
            .error(function(response) {
                Ext.Msg.alert('Ошибка!', response.message || response);
                me.controller.unmask();
                window.close();
            });
    },
    onRowDelete: function (action, rec) {
        var me = this,
            grid = me.getMainView().down('#realityObjIntegrationGrid'),
            selectionModel = grid.getSelectionModel();

        selectionModel.deselect(rec);
    },
    onRowSelect: function (rowModel, record) {
        var me = this,
             view = me.getMainView(),
            storeSelected;

        if (view) {
            storeSelected = view.storeSelected;

            if (storeSelected.find('Id', record.get('Id'), 0, false, false, true) === -1) {
                storeSelected.add(record);
            }
        }
    },
    onRowDeselect: function (rowModel, record) {
        var me = this,
            view = me.getMainView(),
            storeSelected;

        if (view) {
            storeSelected = view.storeSelected;
            storeSelected.removeAt(storeSelected.find('Id', record.get('Id'), 0, false, false, true));
        }
    }
});