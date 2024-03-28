Ext.define('B4.controller.networkoperator.NetworkOperatorRealityObject', {
    extend: 'B4.controller.MenuItemController',
    params: {},
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.Ajax',
        'B4.Url'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['networkoperator.NetworkOperatorRealityObject'],
    stores: [
        'networkoperator.NetworkOperatorRealityObject',
        'dict.TechDecisionForSelect',
        'dict.TechDecisionForSelected'
    ],
    views: [
        'networkoperator.RoGrid',
        'networkoperator.RoEditWindow'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'networkOperatorRoGridWindowAspect',
            gridSelector: 'networkoperatorrogrid',
            editFormSelector: 'networkoperatorroeditwindow',
            storeName: 'networkoperator.NetworkOperatorRealityObject',
            modelName: 'networkoperator.NetworkOperatorRealityObject',
            editWindowView: 'networkoperator.RoEditWindow',
            listeners: {
                beforesetformdata: function (asp, record) {
                    record.set('RealityObject', this.controller.params.realityObjectId);
                },
                aftersetformdata: function (aspect, rec, form) {
                    var networkOperator = rec.get('OperatorName');
                    form.down('b4selectfield').setValue(networkOperator);

                    var techDecisions = rec.get('TechDecisionsTitle');
                    form.down('gkhtriggerfield').setRawValue(techDecisions);
                },
                savesuccess: function (asp, record) {
                    var id = record.getId();
                    var techDecisions = record.get('TechDecisions');
                    B4.Ajax.request(B4.Url.action('SaveTechDecisions', 'NetworkOperatorRealityObject', {
                        recordId: id,
                        techDecisions: techDecisions
                    })).next(function () {
                        asp.controller.getStore('networkoperator.NetworkOperatorRealityObject').load();
                    }).error(function () {
                    });
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'techDecisionsMultiSelectWindowAspect',
            fieldSelector: 'networkoperatorroeditwindow #techDecisionTriggerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#techDecisionsSelectWindow',
            storeSelect: 'dict.TechDecisionForSelect',
            storeSelected: 'dict.TechDecisionForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор технического решения',
            titleGridSelect: 'Технические решения для отбора',
            titleGridSelected: 'Выбранные технические решения',
            onSelectedBeforeLoad: function (store, operation) {
                operation.params['roId'] = this.controller.realityObjectId;
            },
            triggerOpenForm: function () {
                var me = this,
                    field = me.getSelectField();

                me.getForm().show();
                var grid = me.getSelectedGrid();
                if (grid) {
                    grid.getStore().removeAll();
                }

                if (field && field.getValue()) {
                    var selectGrid = me.getSelectGrid();
                    selectGrid.getStore().load({
                        callback: function (data) {
                            var values = field.getValue();
                            var selectionModel = selectGrid.getSelectionModel();
                            var toSelect = new Array();
                            for (var index = 0; index < values.length; index++) {
                                var value = values[index].TechDecision.Id;
                                for (var dataIndex = 0; dataIndex < data.length; dataIndex++) {
                                    if (value == data[dataIndex].data.Id) {
                                        toSelect.push(data[dataIndex]);
                                        break;
                                    }
                                }
                            }

                            selectionModel.select(toSelect);
                        }
                    });
                } else {
                    me.getSelectGrid().getStore().load();
                }
            }
        }
    ],
    refs: [
       { ref: 'mainView', selector: 'networkoperatorrogrid' }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('networkoperatorrogrid');

        me.params.realityObjectId = id;

        me.bindContext(view);
        me.application.deployView(view, 'reality_object_info');

        me.getStore('networkoperator.NetworkOperatorRealityObject').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.roId = this.params.realityObjectId;
        }
    },

    init: function () {
        var me = this;

        me.getStore('networkoperator.NetworkOperatorRealityObject').on('beforeload', me.onBeforeLoad, me);

        me.callParent(arguments);
    }
});