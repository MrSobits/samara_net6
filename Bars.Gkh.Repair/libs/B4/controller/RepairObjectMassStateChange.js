/**
* Контроллер массовой смены статусов объектов ТР
*/
Ext.define('B4.controller.RepairObjectMassStateChange', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        mixins: 'B4.mixins.Context'
    },

    models: ['RepairObject'],

    stores: [
        'RepairObject'
    ],

    views: [
        'repairobjectmassstatechange.MainPanel',
        'repairobjectmassstatechange.Grid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'repairobjectmassstatechange.MainPanel',
    mainViewSelector: 'massRepairChangeStateMainPanel',

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            gridSelector: 'repairObjectMassChangeStateGrid',
            storeName: 'RepairObject',
            modelName: 'RepairObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#RepairObjectMassStateChangeMultiSelectWindow',
            storeSelect: 'RepairObject',
            storeSelected: 'RepairObject',
            titleSelectWindow: 'Выбор объектов ТР',
            titleGridSelect: 'Объекты ТР для отбора',
            titleGridSelected: 'Выбранные объекты ТР',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObjName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, sortable: false },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RealityObjName', flex: 1, sortable: false }
            ],
            onBeforeLoad: function(store, operation) {
                operation.params.programId = this.programId;
                operation.params.stateId = this.controller.stateId;
            },
            otherActions: function(actions) {
                var mainCmpSel = this.controller.mainViewSelector;

                actions[mainCmpSel + ' [name = btnChangeState]'] = { 'click': { fn: this.onClickBtnChange, scope: this } };
                actions[mainCmpSel + ' [name = sfProgram]'] = {
                    'change': { fn: this.onChangeProgram, scope: this },
                    'beforeload': { fn: this.onBeforeLoadProgram, scope: this }
                };
                actions[mainCmpSel + ' [name = cbCurrentState]'] = {
                    'change': { fn: this.onChangeCurrentState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadCurrentState, scope: this }
                };
                actions[mainCmpSel + ' [name = cbNextState]'] = {
                    'change': { fn: this.onChangeNextState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadNextState, scope: this }
                };
                actions[mainCmpSel + ' [name = btnClearGrid]'] = { 'click': { fn: this.onClickBtnClearGrid, scope: this } };
            },
            listeners: {
                getdata: function(me, records) {
                    var mainStore = this.controller.getStore(this.storeName);

                    Ext.each(records.items, function(rec) {
                        if (mainStore.find('Id', rec.get('Id'), 0, false, false, true) == -1) {
                            mainStore.insert(0, rec);
                        }
                    });
                }
            },
            rowAction: function (grid, action, record) {
                if(action.toLowerCase()=='delete') {
                    grid.getStore().remove(record);
                }
            },
            onBeforeLoadProgram: function (store, operation) {
                
                operation = operation || {};
                operation.params = operation.params || {};

                operation.params.onlyFull = true;
            },
            onClickBtnClearGrid: function (btn) {
                this.controller.getStore(this.storeName).removeAll();
            },
            onClickBtnChange: function(btn) {
                var me = this,
                    controller = me.controller,
                    store = controller.getStore(this.storeName);

                var ids = [];

                Ext.each(store.data.items, function(item) {
                    ids.push(item.get('Id'));
                });

                if (ids.length == 0) {
                    Ext.Msg.alert('Ошибка!', 'Не выбрано ни одного объекта');
                    return;
                }

                controller.mask('Загрузка', controller.getMainComponent());
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('MassStateChange', 'RepairObject'),
                    params: {
                        ids: Ext.JSON.encode(ids),
                        newStateId: controller.getMainComponent().down('[name = cbNextState]').getValue()
                    }
                }).next(function(response) {
                    Ext.Msg.alert('Успешно!', 'Статусы выбранных объектов успешно переведены!');
                    store.removeAll();
                    controller.unmask();
                }).error(function (response) {
                    me.controller.unmask();
                    var resp = Ext.decode(response.responseText);
                    Ext.Msg.alert('Ошибка!', resp.message);
                });
            },
            onChangeProgram: function(field, newValue, oldValue) {
                var mainCmp = this.controller.getMainComponent(),
                    btnAdd = mainCmp.down(this.gridSelector).down('b4addbutton');
                this.programId = null;
                if (newValue) {
                    this.programId = newValue.Id;
                    mainCmp.down('[name = cbCurrentState]').setDisabled(false);
                    mainCmp.down('[name = cbNextState]').setDisabled(false);
                    if (this.controller.stateId) {
                        btnAdd.setDisabled(false);
                    }
                } else {
                    mainCmp.down('[name = cbCurrentState]').setDisabled(true);
                    mainCmp.down('[name = cbCurrentState]').setValue(null);
                    mainCmp.down('[name = cbNextState]').setDisabled(true);
                    btnAdd.setDisabled(true);
                }
            },
            onChangeCurrentState: function(field, newValue, oldValue) {
                var controller = this.controller,
                    cbNextState = controller.getMainComponent().down('[name = cbNextState]'),
                    btnAdd = controller.getMainComponent().down(this.gridSelector).down('b4addbutton');
                controller.stateId = newValue;

                if (newValue) {
                    cbNextState.setDisabled(false);
                    if (this.controller.stateId) {
                        btnAdd.setDisabled(false);
                    }
                } else {
                    cbNextState.setDisabled(true);
                    btnAdd.setDisabled(false);
                }
                cbNextState.setValue(null);
                cbNextState.store.load();
            },
            onChangeNextState: function(field, newValue, oldValue) {
                var btnChange = this.controller.getMainComponent().down('[name = btnChangeState]');
                btnChange.setDisabled(!newValue);
            },
            onBeforeLoadCurrentState: function(field, store, operation) {
                operation.params.typeId = 'repair_object';
            },
            onBeforeLoadNextState: function (field, store, operation) {
                operation.params.currentStateId = this.controller.stateId;
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('massRepairChangeStateMainPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('RepairObject').removeAll();
    }
});