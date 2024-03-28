Ext.define('B4.controller.dict.KnmCharacters', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'dict.KnmCharacter'
    ],

    stores: [
        'dict.KnmCharacters',
        'dict.KindCheckGjiForSelect',
        'dict.KindCheckGjiSelected'
    ],

    views: ['dict.knmcharacters.Grid'],

    mainView: 'dict.knmcharacters.Grid',
    mainViewSelector: 'knmcharactersgrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'knmcharactersgrid',
            permissionPrefix: 'GkhGji.Dict.KnmCharacters'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'knmgkhinlinegridaspect',
            gridSelector: 'knmcharactersgrid',
            storeName: 'dict.KnmCharacters',
            modelName: 'dict.KnmCharacter'
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'kindinspection1Aspect',
            fieldSelector: '#trigfKindChecks',
            storeName: 'dict.KnmCharacters',
            modelName: 'dict.KnmCharacter',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#kindCheckMultiSelectWindow',
            storeSelect: 'dict.KindCheckGjiForSelect',
            storeSelected: 'dict.KindCheckGjiSelected',
            textProperty: 'Name',
            titleSelectWindow: 'Выбор видов проверки',
            titleGridSelect: 'Виды проверки для отбора',
            titleGridSelected: 'Выбранные виды проверки',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            onBeforeLoad: function (store, operation) {
                var me = this,
                    kindCheckIds = [],
                    view = me.controller.getMainView(),
                    kindCheck = view.getSelectionModel().getSelection()[0].getData().KindCheck;

                if (Array.isArray(kindCheck)) {
                    kindCheck.forEach((obj) => kindCheckIds.push(obj.Id));
                }

                // Наименование сущности, связывающей вид КНМ с видом проверки
                operation.params.typeEntity = me.controller.getContextValue(view, 'typeEntity');
                operation.params.kindCheckIds = kindCheckIds;
            },
            triggerOpenForm: function () {
                var me = this,
                    field = me.getSelectField(),
                    grid = me.controller.getMainView(),
                    kindCheck = grid.getSelectionModel().getSelection()[0].data.KindCheck;

                me.getForm().show();
                me.getSelectGrid().getStore().load({
                    callback: function () {
                        if (kindCheck) {
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange().filter(function (rec) {
                                    return kindCheck.map(function (x) {
                                        return x.Id;
                                    }).includes(rec.get('Id'));
                                });

                            field.setValue(kindCheck);
                            records.forEach(function (record) {
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            getRawValue: function (val) {
                if (val) {
                    return val.map(function (item) {
                        return item['Name'];
                    }).join(', ');
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    var grid = asp.controller.getMainView(),
                        rows = grid.getStore().getRange(),
                        editor = grid.down('gridcolumn[dataIndex=KindCheck]').getEditor(),
                        rowId = grid.getSelectionModel().getLastSelected().internalId,
                        kindCheck = records.items.map(item => item.data),
                        row = rows.find(x => x.internalId === rowId);

                    row.set('KindCheck', kindCheck);
                    editor.setValue(kindCheck);
                    editor.setRawValue(asp.getRawValue(kindCheck));
                    return true;
                }
            }
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        B4.Ajax.request({
            url: B4.Url.action('GetEntityType', 'KnmCharacter')
        }).next(function (resp) {
            me.setContextValue(view, 'typeEntity', resp.responseText);
        });

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    },

    updateName: function (currentIds, newIds, newNames) {
        var me = this,
            rows = me.getMainView().getStore().getRange(),
            row = rows.find(x => x.data.KindCheckGjiIds == currentIds);

        row.set('Name', newNames);
        row.set('KindCheckGjiIds', newIds);
    }
});