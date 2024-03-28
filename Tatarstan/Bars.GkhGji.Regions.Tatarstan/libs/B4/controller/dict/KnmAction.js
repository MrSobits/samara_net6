Ext.define('B4.controller.dict.KnmAction', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.KnmAction',
        'dict.ControlType',
        'dict.KnmTypes',
        'dict.KindAction'
    ],

    stores: [
        'dict.KnmAction',
        'dict.ControlType',
        'dict.ControlTypeForSelect',
        'dict.ControlTypeForSelected',
        'dict.KnmTypes',
        'dict.KnmTypesForSelect',
        'dict.KnmTypesForSelected',
        'dict.KindActionForSelect',
        'dict.KindActionForSelected',
    ],

    views: ['dict.knmaction.Grid'],

    mainView: 'dict.knmaction.Grid',
    mainViewSelector: 'knmactiongrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'knmactiongrid',
            permissionPrefix: 'GkhGji.Dict.KnmAction'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'knmgkhinlinegridaspect',
            gridSelector: 'knmactiongrid',
            storeName: 'dict.KnmAction',
            modelName: 'dict.KnmAction'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'controlTypeMultiSelectAspect',
            fieldSelector: '#trigfControlType',
            storeName: 'dict.ControlType',
            modelName: 'dict.ControlType',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#controlTypeMultiSelectWindow',
            storeSelect: 'dict.ControlTypeForSelect',
            storeSelected: 'dict.ControlTypeForSelected',
            textProperty: 'Name',
            titleSelectWindow: 'Выбор видов контроля',
            titleGridSelect: 'Виды контроля для отбора',
            titleGridSelected: 'Выбранные виды контроля',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            triggerOpenForm: function () {
                var me = this,
                    field = me.getSelectField(),
                    grid = me.controller.getMainView(),
                    controlType = grid.getSelectionModel().getSelection()[0].data.ControlType;

                me.getForm().show();
                me.getSelectGrid().getStore().load({
                    callback: function () {
                        if (controlType) {
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange()
                                    .filter((rec) => controlType.map((x) => x.Id).includes(rec.get('Id')));

                            field.setValue(controlType);
                            records.forEach(function (record) {
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            getRawValue: (val) => this.getNewRawValue(val),
            listeners: {
                getdata: function (asp, records) {
                    var grid = asp.controller.getMainView(),
                        rows = grid.getStore().getRange(),
                        editor = grid.down('gridcolumn[dataIndex=ControlType]').getEditor(),
                        rowId = grid.getSelectionModel().getLastSelected().internalId,
                        controlType = records.items.map(item => item.data),
                        row = rows.find(x => x.internalId === rowId);

                    row.set('ControlType', controlType);
                    editor.setValue(controlType);
                    editor.setRawValue(asp.controller.getNewRawValue(controlType));
                    return true;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'knmTypeMultiSelectAspect',
            fieldSelector: '#trigfKnmType',
            storeName: 'dict.KnmTypes',
            modelName: 'dict.KnmTypes',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#knmTypeMultiSelectWindow',
            storeSelect: 'dict.KnmTypesForSelect',
            storeSelected: 'dict.KnmTypesForSelected',
            textProperty: 'Name',
            titleSelectWindow: 'Выбор видов КНМ',
            titleGridSelect: 'Виды КНМ для отбора',
            titleGridSelected: 'Выбранные виды КНМ',
            columnsGridSelect: [
                {
                    header: 'Наименование', xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 1,
                    text: 'Наименование',
                    sortable: false,
                    renderer: (val) => val? val.map((x) => x['Name']).join(', ') : null
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование', xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 1,
                    text: 'Наименование',
                    sortable: false,
                    renderer: (val) => val ? val.map((x) => x['Name']).join(', ') : null
                }
            ],
            triggerOpenForm: function () {
                var me = this,
                    field = me.getSelectField(),
                    grid = me.controller.getMainView(),
                    knmType = grid.getSelectionModel().getSelection()[0].data.KnmType;

                me.getForm().show();
                me.getSelectGrid().getStore().load({
                    callback: function () {
                        if (knmType) {
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange()
                                    .filter((rec) => knmType.map((x) => x.KnmTypesId).includes(rec.get('Id')));

                            field.setValue(knmType);
                            records.forEach(function (record) {
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            getRawValue: (val) => this.getNewRawValue(val),
            listeners: {
                getdata: function (asp, records) {
                    var grid = asp.controller.getMainView(),
                        rows = grid.getStore().getRange(),
                        editor = grid.down('gridcolumn[dataIndex=KnmType]').getEditor(),
                        rowId = grid.getSelectionModel().getLastSelected().internalId,
                        knmType = records.items.map(item => item.data),
                        row = rows.find(x => x.internalId === rowId);

                    row.set('KnmType', knmType);
                    editor.setValue(knmType);
                    editor.setRawValue(asp.controller.getNewRawValue(knmType));
                    return true;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'kindActionMultiSelectAspect',
            fieldSelector: '#trigfKindAction',
            idProperty: 'Value',
            valueProperty: 'Value',
            textProperty: 'Display',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#kindActionMultiSelectWindow',
            storeSelect: 'dict.KindActionForSelect',
            storeSelected: 'dict.KindActionForSelected',
            columnsGridSelect: [
                { 
                    header: 'Наименование', 
                    xtype: 'gridcolumn', 
                    dataIndex: 'Display', 
                    flex: 1, 
                    sortable: false,
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование', 
                    xtype: 'gridcolumn', 
                    dataIndex: 'Display', 
                    flex: 1, 
                    sortable: false,
                }
            ],
            titleSelectWindow: 'Выбор видов мероприятий без взаимодействия с контролируемыми лицами',
            titleGridSelect: 'Виды мероприятия для отбора',
            titleGridSelected: 'Выбранные виды мероприятий',
            getRawValue: (val) => val.map((x) => B4.enums.KindAction.displayRenderer(x)).join(', '),
            triggerOpenForm: function () {
                var me = this,
                    field = me.getSelectField(),
                    grid = me.controller.getMainView(),
                    kindAction = grid.getSelectionModel().getSelection()[0].data.KindAction;

                me.getForm().show();
                me.getSelectedGrid().getStore().removeAll();
                me.getSelectGrid().getStore().load({
                    callback: function () {
                        if (kindAction) {
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange()
                                    .filter(x => kindAction.includes(x.get('Value')));

                            field.setValue(kindAction);
                            records.forEach(function (record) {
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
            },
            listeners: {
                getdata: function (asp, records) {
                    var grid = asp.controller.getMainView(),
                        rows = grid.getStore().getRange(),
                        editor = grid.down('gridcolumn[dataIndex=KindAction]').getEditor(),
                        rowId = grid.getSelectionModel().getLastSelected().internalId,
                        kindAction = records.items.map(item => item.data),
                        row = rows.find(x => x.internalId === rowId);

                    row.set('KindAction', kindAction.map((x) => x.Value));
                    editor.setValue(kindAction);
                    editor.setRawValue(kindAction.map((x) => B4.enums.KindAction.displayRenderer(x.Value)).join(', '));
                    return true;
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'knmactiongrid gridcolumn[dataIndex=ControlType]': {
                click: {
                    fn: function(gridview, td, rowNum, colNum, event, raw) {
                        me.onTrigFieldClick(raw, "ControlType", (field, editor) =>
                        {
                            editor.updateDisplayedText(field.map(x => x.Name).join(', '));
                        });
                    },
                    scope: me
                }
            },
            'knmactiongrid gridcolumn[dataIndex=KnmType]': {
                click: {
                    fn: function(gridview, td, rowNum, colNum, event, raw) {
                        me.onTrigFieldClick(raw, "KnmType", (field, editor) =>
                        {
                            editor.updateDisplayedText(field.map(x => x.Name).join(', '));
                        });
                    },
                    scope: me
                }
            },
            'knmactiongrid gridcolumn[dataIndex=KindAction]': {
                click: {
                    fn: function(gridview, td, rowNum, colNum, event, raw) {
                        me.onTrigFieldClick(raw, "KindAction", (field, editor) => 
                        {
                            editor.updateDisplayedText(field.map(x => B4.enums.KindAction
                                .displayRenderer(x))
                                .join(', ')
                            );
                        });
                    },
                    scope: me
                }
            }
        });

        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        B4.Ajax.request({
            url: B4.Url.action('ListItems', 'GkhConfig'),
            params: {
                parent: 'ErknmIntegrationConfig'
            }
        }).next(function (resp) {
            var res = Ext.JSON.decode(resp.responseText),
                KnmTypeId = res.data.find(x => x.id == 'ErknmIntegrationConfig.KnmActionId').value;
            view.down('textfield[name=KnmTypeId]').setValue(KnmTypeId);
        });

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    getNewRawValue: (val) => val ? val.map((x) => x['Name']).join(', ') : null,

    onTrigFieldClick: function (raw, fieldName, fn){
        var me = this,
            view = me.getMainView(),
            editor = view.down(`gridcolumn[dataIndex=${fieldName}]`).getEditor(),
            field = raw.getData()[fieldName];

        if (field) {
            fn(field, editor)
        }
    }
});