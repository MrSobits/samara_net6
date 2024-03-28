Ext.define('B4.controller.ConstructionObjectMassChangeState', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['ConstructionObject'],

    stores: [
        'ConstructionObjectMassChangeState',
        'ConstructionObjectForSelect',
        'ConstructionObjectForSelected'
    ],

    views: [
        'constructionobjectmasschangestate.MainPanel',
        'constructionobjectmasschangestate.Grid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'constructionobjectmasschangestate.MainPanel',
    mainViewSelector: 'constructionobjectmasschangestatemainpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'constructionobjectmasschangestatemainpanel'
        }
    ],

    stateId: null,
    resettlementProgramId: null,
    stateTypeId: 'gkh_construct_obj',

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'constructionobjectmasschangestateaspect',
            gridSelector: 'constructionobjectmasschangestategrid',
            storeName: 'ConstructionObjectMassChangeState',
            modelName: 'ConstructionObject',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#constructionObjectMassStateChangeSelectWindow',
            storeSelect: 'ConstructionObjectForSelect',
            storeSelected: 'ConstructionObjectForSelected',
            titleSelectWindow: 'Выбор объектов строительства',
            titleGridSelect: 'Объекты строительства для отбора',
            titleGridSelected: 'Выбранные объекты строительства',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    sortable: false
                },
                {
                    header: 'Адрес',
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    sortable: false
                }
            ],
            onBeforeLoad: function(store, operation) {
                var me = this;
                operation.params.stateId = me.controller.stateId;
                operation.params.resettlementProgramId = me.controller.resettlementProgramId;
            },
            otherActions: function(actions) {
                var me = this,
                    mainViewSelector = me.controller.mainViewSelector;

                actions[mainViewSelector + ' button[action=Execute]'] = {
                    'click': { fn: me.onClickExecute, scope: me }
                };
                actions[mainViewSelector + ' [name=ResettlementProgram]'] = {
                    'change': { fn: me.onChangeResettlementProgram, scope: me }
                };
                actions[mainViewSelector + ' [name=CurrentState]'] = {
                    'change': { fn: me.onChangeCurrentState, scope: me },
                    'storebeforeload': { fn: me.onBeforeLoadCurrentState, scope: me }
                };
                actions[mainViewSelector + ' [name=NextState]'] = {
                    'change': { fn: me.onChangeNextState, scope: me },
                    'storebeforeload': { fn: me.onBeforeLoadNextState, scope: me }
                };
                actions[mainViewSelector + ' button[action=RemoveAll]'] = {
                    'click': { fn: me.onClickRemoveAll, scope: me }
                };
            },
            listeners: {
                getdata: function(asp, records) {
                    var store = asp.controller.getStore(asp.storeName);

                    Ext.each(records.items, function(rec) {
                        if (store.find('Id', rec.get('Id'), 0, false, false, true) == -1) {
                            store.insert(0, rec);
                        }
                    });
                }
            },
            onClickRemoveAll: function() {
                var me = this;
                me.controller.getStore(me.storeName).removeAll();
            },
            onClickExecute: function() {
                var me = this,
                    store = me.controller.getStore(me.storeName),
                    mainView = me.controller.getMainComponent(),
                    ids = [];

                Ext.each(store.data.items, function(item) {
                    ids.push(item.get('Id'));
                });

                if (!ids.length) {
                    Ext.Msg.alert('Ошибка!', 'Не выбрано ни одного объекта');
                    return;
                }

                me.controller.mask('Загрузка', mainView);
                B4.Ajax.request({
                    url: B4.Url.action('MassChangeState', 'ConstructionObject'),
                    method: 'POST',
                    timeout: 5 * (60 * 1000), //5min
                    params: {
                        ids: Ext.JSON.encode(ids),
                        newStateId: mainView.down('[name=NextState]').getValue(),
                        oldStateId: mainView.down('[name=CurrentState]').getValue()
                    }
                }).next(function(response) {
                    var decoded = Ext.JSON.decode(response.responseText);

                    if (Ext.isEmpty(decoded.message)) {
                        Ext.Msg.alert('Успешно', 'Статусы выбранных объектов успешно переведены');
                    } else {
                        Ext.Msg.alert('Успешно', decoded.message);
                    }

                    store.removeAll();
                    me.controller.unmask();
                }).error(function(e) {
                    me.controller.unmask();
                    Ext.Msg.alert('Ошибка', e.message || e);
                });
            },
            onChangeResettlementProgram: function(field, newValue) {
                var me = this,
                    mainView = me.controller.getMainComponent(),
                    btnAdd = mainView.down(me.gridSelector).down('b4addbutton'),
                    btnRemoveAll = mainView.down(me.gridSelector).down('[action=RemoveAll]'),
                    currentStateField = mainView.down('[name=CurrentState]'),
                    nextStateField = mainView.down('[name=NextState]');

                me.controller.getStore('ConstructionObjectMassChangeState').removeAll();

                if (newValue) {
                    me.controller.resettlementProgramId = newValue.Id;

                    currentStateField.setDisabled(false);
                    nextStateField.setDisabled(false);

                    if (me.controller.stateId) {
                        btnAdd.setDisabled(false);
                        btnRemoveAll.setDisabled(false);
                    }
                } else {
                    me.controller.resettlementProgramId = null;

                    currentStateField.setDisabled(true);
                    currentStateField.setValue(null);
                    nextStateField.setDisabled(true);
                    btnAdd.setDisabled(true);
                    btnRemoveAll.setDisabled(true);
                }
            },
            onChangeCurrentState: function(field, newValue) {
                var me = this,
                    mainView = me.controller.getMainComponent(),
                    nextStateField = mainView.down('[name=NextState]'),
                    btnAdd = mainView.down(me.gridSelector).down('b4addbutton'),
                    btnRemoveAll = mainView.down(me.gridSelector).down('[action=RemoveAll]');

                me.controller.stateId = newValue;

                if (newValue) {
                    nextStateField.setDisabled(false);

                    if (me.controller.resettlementProgramId) {
                        btnAdd.setDisabled(false);
                        btnRemoveAll.setDisabled(false);
                    }
                } else {
                    nextStateField.setDisabled(true);
                    btnAdd.setDisabled(false);
                    btnRemoveAll.setDisabled(false);
                }

                nextStateField.setValue(null);
                nextStateField.store.load();
            },
            onChangeNextState: function(field, newValue) {
                var me = this,
                    btnChange = me.controller.getMainComponent().down('[action=Execute]');

                btnChange.setDisabled(!newValue);
            },
            onBeforeLoadCurrentState: function (field, store, operation) {
                var me = this;
                operation.params.typeId = me.controller.stateTypeId;
            },
            onBeforeLoadNextState: function(field, store, operation) {
                var me = this;
                operation.params.currentStateId = me.controller.stateId;
            },
            deleteRecord: function(record) {
                var me = this;
                me.controller.getStore('ConstructionObjectMassChangeState').remove(record);
            }
        }
    ],

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('constructionobjectmasschangestatemainpanel');

        me.bindContext(view);
        me.application.deployView(view);
        me.getStore('ConstructionObjectMassChangeState').removeAll();
    }
});