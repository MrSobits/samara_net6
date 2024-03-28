/**
* Контроллер реестра массовой смены статусов объектов кр
*/
Ext.define('B4.controller.ObjectCrMassChangeState', {
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

    models: ['ObjectCr'],

    stores: [
        'ObjectCrMassChangeState',
        'objectcr.ForSelect',
        'objectcr.ForSelected'
    ],

    views: [
        'objectcrmasschangestate.MainPanel',
        'objectcrmasschangestate.Grid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'objectcrmasschangestate.MainPanel',
    mainViewSelector: 'massChangeStateMainPanel',

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            gridSelector: '#objectCrMassChangeStateGrid',
            storeName: 'ObjectCrMassChangeState',
            modelName: 'ObjectCr',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#objCrMassStateChangeMultiSelectWindow',
            storeSelect: 'objectcr.ForSelect',
            storeSelected: 'objectcr.ForSelected',
            titleSelectWindow: 'Выбор объектов КР',
            titleGridSelect: 'Объекты КР для отбора',
            titleGridSelected: 'Выбранные объекты КР',
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

                actions[mainCmpSel + ' #btnChangeState'] = { 'click': { fn: this.onClickBtnChange, scope: this } };
                actions[mainCmpSel + ' #sfProgramCr'] = {
                    'change': { fn: this.onChangeProgram, scope: this },
                    'beforeload': { fn: this.onBeforeLoadProgram, scope: this }
                };
                actions[mainCmpSel + ' #cbCurrentState'] = {
                    'change': { fn: this.onChangeCurrentState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadCurrentState, scope: this }
                };
                actions[mainCmpSel + ' #cbNextState'] = {
                    'change': { fn: this.onChangeNextState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadNextState, scope: this }
                };
                actions[mainCmpSel + ' #btnClearGrid'] = { 'click': { fn: this.onClickBtnClearGrid, scope: this } };
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
                B4.Ajax.request(B4.Url.action('MassChangeState', 'ObjectCr', {
                    ids: Ext.JSON.encode(ids),
                    newStateId: controller.getMainComponent().down('#cbNextState').getValue()
                })).next(function(response) {
                    Ext.Msg.alert('Успешно!', 'Статусы выбранных объектов успешно переведены!');
                    store.removeAll();
                    controller.unmask();
                }).error(function() {
                    controller.unmask();
                });
            },
            onChangeProgram: function(field, newValue, oldValue) {
                var mainCmp = this.controller.getMainComponent(),
                    btnAdd = mainCmp.down(this.gridSelector).down('b4addbutton');
                this.programId = null;
                if (newValue) {
                    this.programId = newValue.Id;
                    mainCmp.down('#cbCurrentState').setDisabled(false);
                    mainCmp.down('#cbNextState').setDisabled(false);
                    if (this.controller.stateId) {
                        btnAdd.setDisabled(false);
                    }
                } else {
                    mainCmp.down('#cbCurrentState').setDisabled(true);
                    mainCmp.down('#cbCurrentState').setValue(null);
                    mainCmp.down('#cbNextState').setDisabled(true);
                    btnAdd.setDisabled(true);
                }
            },
            onChangeCurrentState: function(field, newValue, oldValue) {
                var controller = this.controller,
                    cbNextState = controller.getMainComponent().down('#cbNextState'),
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
                var btnChange = this.controller.getMainComponent().down('#btnChangeState');
                btnChange.setDisabled(!newValue);
            },
            onBeforeLoadCurrentState: function(field, store, operation) {
                operation.params.typeId = 'cr_object';
            },
            onBeforeLoadNextState: function (field, store, operation) {
                operation.params.currentStateId = this.controller.stateId;
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('massChangeStateMainPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ObjectCrMassChangeState').removeAll();
    }
});