Ext.require('B4.enums.TypeHouse');
Ext.require('B4.enums.HeatingSystem');
Ext.require('B4.enums.HeatSeasonDocType');
/**
* Контроллер реестра массовой смены статусов документов подготовки к отопительному сезону
*/
Ext.define('B4.controller.HeatSeasDocMassChangeState', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url',
        
        'B4.enums.HeatSeasonDocType',
        'B4.enums.HeatingSystem',
        'B4.enums.TypeHouse'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['heatseason.Document'],

    stores: [
        'HeatSeasDocMassChangeState',
        'heatseason.DocumentForSelect',
        'heatseason.DocumentForSelected'
    ],

    views: [
        'heatseasdocmasschangestate.MainPanel',
        'heatseasdocmasschangestate.Grid',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'heatseasdocmasschangestate.MainPanel',
    mainViewSelector: 'heatSeasDocMassChangeStateMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'heatSeasDocMassChangeStateMainPanel'
        }
    ],

    //идентификатор статуса
    stateId: null,
    //идентификатор периода
    periodId: null,

    aspects: [
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'heatseasdocmasschangestateaspect',
            gridSelector: '#heatSeasDocMassChangeStateGrid',
            storeName: 'HeatSeasDocMassChangeState',
            modelName: 'heatseason.Document',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#heatSeasDocMassStateChangeSelectWindow',
            storeSelect: 'heatseason.DocumentForSelect',
            storeSelected: 'heatseason.DocumentForSelected',
            titleSelectWindow: 'Выбор документов',
            titleGridSelect: 'Документы для отбора',
            titleGridSelected: 'Выбранные документы',
            columnsGridSelect: [
                {
                    header: 'Система отопления',
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatingSystem',
                    flex: 1,
                    renderer: function (val) {
                        return B4.enums.HeatingSystem.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatingSystem.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    header: 'Тип документа',
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDocument',
                    flex: 1,
                    renderer: function (val) { return B4.enums.HeatSeasonDocType.displayRenderer(val); },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.HeatSeasonDocType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valuefield: 'value',
                        displayfield: 'display'
                    }
                },
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeHouse',
                    flex: 1,
                    header: 'Тип дома',
                    renderer: function (val) {
                        return B4.enums.TypeHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.TypeHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    header: 'Управляющая организация',
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrgName',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Тип документа',
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDocument',
                    flex: 1,
                    sortable: false,
                    renderer: function (val) {
                        return B4.enums.HeatSeasonDocType.displayRenderer(val);
                    }
                },
                {
                    header: 'Муниципальное образование',
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
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
            onBeforeLoad: function (store, operation) {
                operation.params.periodId = this.controller.periodId;
                operation.params.stateId = this.controller.stateId;
            },
            otherActions: function (actions) {
                var mainCmpSel = this.controller.mainViewSelector;

                actions[mainCmpSel + ' #btnChangeState'] = { 'click': { fn: this.onClickBtnChange, scope: this} };
                actions[mainCmpSel + ' #sfHeatSeasonPeriod'] = {
                    'change': { fn: this.onChangePeriod, scope: this }
                };
                actions[mainCmpSel + ' #cbCurrentState'] = {
                    'change': { fn: this.onChangeCurrentState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadCurrentState, scope: this }
                };
                actions[mainCmpSel + ' #cbNextState'] = {
                    'change': { fn: this.onChangeNextState, scope: this },
                    'storebeforeload': { fn: this.onBeforeLoadNextState, scope: this }
                };
            },
            listeners: {
                getdata: function (me, records) {
                    var mainStore = this.controller.getStore(this.storeName);

                    Ext.each(records.items, function (rec) {
                        if (mainStore.find('Id', rec.get('Id'), 0, false, false, true) == -1) {
                            mainStore.insert(0, rec);
                        }
                    });
                }
            },
            onClickBtnChange: function(btn) {
            var me = this,
                controller = me.controller,
                store = controller.getStore(me.storeName);

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
                    url: B4.Url.action('MassChangeState', 'HeatSeasonDoc'),
                    metod: 'POST',
                    params: {
                                ids: Ext.JSON.encode(ids),
                                newStateId: controller.getMainComponent().down('#cbNextState').getValue(),
                                oldStateId: controller.getMainComponent().down('#cbCurrentState').getValue()
                            }
            }).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText);

                        if (Ext.isEmpty(obj.message))
                            Ext.Msg.alert('Успешно!', 'Статусы выбранных объектов успешно переведены!');
                        else
                            Ext.Msg.alert('Успешно!', obj.message);
                        store.removeAll();
                        controller.unmask();
                    }).error(function (e) {
                        controller.unmask();
                        Ext.Msg.alert('Неудача!', e.message || e);
                    });
                },
            onChangePeriod: function (field, newValue, oldValue) {
                var controller = this.controller,
                    mainCmp = controller.getMainComponent(),
                    btnAdd = mainCmp.down(this.gridSelector).down('b4addbutton');

                controller.getStore('HeatSeasDocMassChangeState').removeAll();

                if (newValue) {
                    controller.periodId = newValue.Id;
                    mainCmp.down('#cbCurrentState').setDisabled(false);
                    mainCmp.down('#cbNextState').setDisabled(false);
                    if (controller.stateId) {
                        btnAdd.setDisabled(false);
                    }
                } else {
                    controller.periodId = null;
                    mainCmp.down('#cbCurrentState').setDisabled(true);
                    mainCmp.down('#cbCurrentState').setValue(null);
                    mainCmp.down('#cbNextState').setDisabled(true);
                    btnAdd.setDisabled(true);
                }
            },
            onChangeCurrentState: function (field, newValue, oldValue) {
                var controller = this.controller,
                    cbNextState = controller.getMainComponent().down('#cbNextState'),
                    btnAdd = controller.getMainComponent().down(this.gridSelector).down('b4addbutton');
                controller.stateId = newValue;

                if (newValue) {
                    cbNextState.setDisabled(false);
                    if (controller.periodId) {
                        btnAdd.setDisabled(false);
                    }
                } else {
                    cbNextState.setDisabled(true);
                    btnAdd.setDisabled(false);
                }
                cbNextState.setValue(null);
                cbNextState.store.load();
            },
            onChangeNextState: function (field, newValue, oldValue) {
                var btnChange = this.controller.getMainComponent().down('#btnChangeState');
                btnChange.setDisabled(!newValue);
            },
            onBeforeLoadCurrentState: function (field, store, operation) {
                operation.params.typeId = 'gji_heatseason_document';
            },
            onBeforeLoadNextState: function (field, store, operation) {
                operation.params.currentStateId = this.controller.stateId;
            },
            deleteRecord: function (record) {
                this.controller.getStore('HeatSeasDocMassChangeState').remove(record);
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('heatSeasDocMassChangeStateMainPanel');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('HeatSeasDocMassChangeState').removeAll();
    }
});