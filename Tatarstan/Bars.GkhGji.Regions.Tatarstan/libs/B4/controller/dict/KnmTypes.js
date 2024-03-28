Ext.define('B4.controller.dict.KnmTypes', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['dict.KnmTypes'],
    stores: [
        'dict.TypeSurveyGji',
        'dict.KnmTypes',
        'dict.KindCheckGjiForSelect',
        'dict.KindCheckGjiSelected'
    ],

    views: [
        'SelectWindow.MultiSelectWindow',
        'dict.knmtypes.Grid'
    ],

    mainView: 'dict.knmtypes.Grid',
    mainViewSelector: 'knmtypesgrid',

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            name: 'knmtypesinlinegridaspect',
            gridSelector: 'knmtypesgrid',
            permissionPrefix: 'GkhGji.Dict.KnmTypes'
        },
        {
            xtype: 'gkhinlinegridaspect',
            gridSelector: 'knmtypesgrid',
            storeName: 'dict.KnmTypes',
            modelName: 'dict.KnmTypes',
            listeners: {
                'beforesave': function (asp, store) {
                    var me = this,
                        modifiedRecords = store.getModifiedRecords(),
                        validName = true;

                    Ext.each(modifiedRecords, function (rec) {
                        if (validName && !me.validate(rec, 'KindCheck') || !me.validate(rec, 'ErvkId')) {
                            validName = false;
                            return false;
                        }
                    });

                    if (!validName) {
                        Ext.Msg.alert('Ошибка сохранения!', 'Для каждой записи необходимо заполнить все поля!');
                        return false;
                    }

                    return true;
                }
            },

            validate: function (rec, field) {
                return !Ext.isEmpty(rec.get(field));
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'kindcheckmultiselect',
            fieldSelector: '#tfKindCheck',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#kindCheckSelectWindow',
            storeSelect: 'dict.KindCheckGjiForSelect',
            storeSelected: 'dict.KindCheckGjiSelected',
            columnsGridSelect: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            textProperty: 'Name',
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
                    callback: function(){
                        if(kindCheck){
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange().filter(function (rec) {
                                    return kindCheck.map(function(x){
                                        return x.Id;
                                    }).includes(rec.get('Id'));
                                });

                            field.setValue(kindCheck);
                            records.forEach(function(record){
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            getRawValue: function(val) {
                if(val) {
                    return val.map(function(item) {
                        return item['Name'];
                    }).join(', ');
                }
            },
            listeners: {
                getdata: function (asp, records) {
                    if (records.length > 0) {
                        var grid = asp.controller.getMainView(),
                            rows = grid.getStore().getRange(),
                            editor = grid.down('gridcolumn[dataIndex=KindCheck]').getEditor(), 
                            rowId = grid.getSelectionModel().getLastSelected().internalId, 
                            kindCheck = records.items.map(item=>item.data),
                            row = rows.find(x => x.internalId === rowId);
                        
                        row.set('KindCheck', kindCheck);
                        editor.setValue(kindCheck);
                        editor.setRawValue(asp.getRawValue(kindCheck));
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать один или несколько видов проверок');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],
    
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
                KnmTypeId = res.data.find(x=>x.id == 'ErknmIntegrationConfig.KnmTypeId').value;
            view.down('textfield[name=KnmTypeId]').setValue(KnmTypeId);
        });

        B4.Ajax.request({
            url: B4.Url.action('GetEntityType', 'KnmTypes')
        }).next(function (resp) {
            me.setContextValue(view, 'typeEntity', resp.responseText);
        });
        
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});