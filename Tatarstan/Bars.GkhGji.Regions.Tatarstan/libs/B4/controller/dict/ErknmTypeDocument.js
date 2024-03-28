Ext.define('B4.controller.dict.ErknmTypeDocument', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    models: [
        'dict.ErknmTypeDocument'
    ],
    stores: [
        'dict.ErknmTypeDocument',
        'dict.KindCheckGji', 
        'dict.KindCheckGjiSelected', 
        'dict.KindCheckGjiForSelect' 
    ],
    views: [
        'dict.erknmtypedocument.Grid'
    ],
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.erknmtypedocument.Grid',
    mainViewSelector: 'erknmtypedocumentgrid',
    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            name: 'typedocinlinegridaspect',
            gridSelector: 'erknmtypedocumentgrid',
            permissionPrefix: 'GkhGji.Dict.ErknmTypeDocument'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'erknmtypedocumentgrid',
            storeName: 'dict.ErknmTypeDocument',
            modelName: 'dict.ErknmTypeDocument',
            requiredFields: true
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'typedocumentserknmkindcheckMultiSelectWindowAspect',
            fieldSelector: '#trigfKindCheck',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#typedocumentserknmkindcheckSelectWindow',
            storeSelect: 'dict.KindCheckGjiForSelect',
            storeSelected: 'dict.KindCheckGjiSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор видов проверки',
            titleGridSelect: 'Виды проверки для отбора',
            titleGridSelected: 'Выбранные виды проверки',
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
                    editor.setRawValue(asp.controller.getNewRawValue(kindCheck));
                    return true;
                }
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
                                records = selectGrid.getStore().getRange()
                                    .filter((rec) => kindCheck.map((x) => x.Id).includes(rec.get('Id')));

                            field.setValue(kindCheck);
                            records.forEach(function (record) {
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            getRawValue: (val) => this.getNewRawValue(val),
        },
    ],

    getNewRawValue: (val) => val ? val.map((x) => x['Name']).join(', ') : null,

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});