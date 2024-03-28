Ext.define('B4.controller.dict.InspectionBaseType', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.enums.TypeDocumentGji',
        'B4.view.Control.GkhTriggerField',
        'Ext.ux.CheckColumn'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    views: ['dict.inspectionbasetype.Grid', 'SelectWindow.MultiSelectWindow'],
    
    stores: [
        'dict.InspectionBaseType',
        'dict.KindCheckGji'
    ],

    mainView: 'dict.inspectionbasetype.Grid',
    mainViewSelector: 'inspectionbasetypegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'inspectionbasetypegrid'
        }
    ],
    
    defaultColumns: [],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'inspectionbasetypegrid',
            permissionPrefix: 'GkhGji.Dict.InspectionBaseType'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'inlineGridAspect',
            storeName: 'dict.InspectionBaseType',
            modelName: 'dict.InspectionBaseType',
            gridSelector: 'inspectionbasetypegrid',
            listeners: {
                'beforesave': function (asp, store) {
                    var modifiedRecords = store.getModifiedRecords(),
                        validate = true,
                        requiredFields,
                        openValuesForErknm = asp.controller.getMainView().down('[name=OpenValuesForErknm]').value;
                    
                    if(openValuesForErknm){
                        Ext.each(modifiedRecords, function(rec) {
                            if (!rec.get('ErknmCode')) {
                                requiredFields = 'Код в ЕРКНМ';
                                validate = false;
                            }
                        });
                    } else {
                        Ext.each(modifiedRecords, function(rec) {
                            if (!rec.get('Name') || !rec.get('Code') || !rec.get('InspectionKindId')) {
                                requiredFields = 'Наименование, Код, Вид проверки';
                                validate = false;
                            }
                        });
                    }
                    
                    if (!validate) {
                        Ext.Msg.alert('Ошибка сохранения', 'Не заполнено одно или несколько обязательных полей: ' + requiredFields);
                    }

                    return validate;
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'kindCheckMultiselectwindowaspect',
            fieldSelector: '#kindCheckEditor',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#kindCheckSelectWindow',
            storeSelect: 'dict.KindCheckGji',
            storeSelected: 'dict.KindCheckGji',
            columnsGridSelect: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1
                }
            ],
            columnsGridSelected: [
                {
                    header: 'Наименование',
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    sortable: false
                }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранные записи',
            triggerOpenForm: function () {
                var me = this,
                    editor = me.getSelectField(),
                    kindChecks = me.controller.getAspect('inlineGridAspect').getGrid().getSelectionModel().getSelection()[0].data.KindCheck;

                me.getForm().show();
                me.getSelectGrid().getStore().load({
                    callback: function(){
                        if(kindChecks){
                            var selectGrid = me.getSelectGrid(),
                                records = selectGrid.getStore().getRange().filter(function (rec) {
                                    return kindChecks.map(function(x){
                                        return x.Id;
                                    }).includes(rec.get('Id'));
                                });

                            editor.setValue(kindChecks);
                            records.forEach(function(record){
                                selectGrid.getSelectionModel().select(record);
                            });
                        }
                    }
                });
                me.getSelectedGrid().getStore().removeAll();
            },
            listeners: {
                getdata: function (me, records) {
                    var selectedRow = me.controller.getAspect('inlineGridAspect').getGrid().getSelectionModel().getSelection()[0];

                    selectedRow.set('KindCheck', records.getRange().map(function(x){return x.data;}));
                    return true;
                }
            }
        }
    ],
    
    init: function() {
        var me = this;

        me.control({
            'inspectionbasetypegrid [name=OpenValuesForErknm]': {
                change: {
                    fn: me.reconfigureGrid,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget(me.mainViewSelector);
        
        me.defaultColumns = view.initialConfig.columns;
        
        view.getStore().on('beforeload', me.onStoreBeforeload, me);
        
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    onStoreBeforeload: function (store, operation) {
        var me = this;
        
        operation.params.openValuesForErknm = me.getMainView().down('[name=OpenValuesForErknm]').value;
    },

    reconfigureGrid: function(checkBox, value)
    {
        var me = this,
            grid = me.getMainView(),
            store = grid.getStore(),
            columns = [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 2,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 1,
                    text: 'Вид проверки',
                    renderer: function (data) {
                        return data ? Ext.Array.map(data, function (value) { return value.Name; }).join() : data;
                    },
                    editor: {
                        xtype: 'gkhtriggerfield',
                        itemId: 'kindCheckEditor',
                        editable: false,
                        onTrigger2Click: function () {
                            var me = this;

                            me.fireEvent('triggerClear', this);
                            me.setValue([]);
                            me.updateDisplayedText();
                        },
                        listeners: {
                            focus: function(cmp){
                                var documentTypes = me.getMainView().getSelectionModel().getSelection()[0].data.KindCheck,
                                    display = Ext.Array.map(documentTypes, function (value) {
                                        return value.Name;
                                    }).join();

                                cmp.updateDisplayedText(display);
                            }
                        }
                    }
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'HasTextField',
                    flex: 1,
                    text: 'Наличие текстового поля'
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'HasDate',
                    flex: 1,
                    text: 'Наличие даты'
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'HasRiskIndicator',
                    flex: 1,
                    text: 'Наличие индикатора риска'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErknmCode',
                    flex: 1,
                    text: 'Код в ЕРКНМ',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 10
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: grid
                }
            ];
        
        if(value === true){
            grid.reconfigure(null, columns);
        } else {
            grid.reconfigure(null, me.defaultColumns);
        }

        store.sorters.clear();
        store.load();
    }
});