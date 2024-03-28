Ext.define('B4.view.inspectiongji.RiskPrevGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.inspectiongjiriskprevgrid',

    requires: [
        'B4.grid.SelectFieldEditor',
        'B4.store.dict.BaseDict',
        'B4.store.inspectiongji.Risk',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    border: false,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.inspectiongji.Risk'),
            riskCategoryStore = Ext.create('B4.store.dict.BaseDict', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'RiskCategory'
                }
            }),
            getInspectionId = function() {
                return me.up('formwindow').inspectionId;
            };

        store.on('beforeload', function(_store, operation) {
            Ext.applyIf(operation.params, {
                inspectionId: getInspectionId()
            });
        }, me);

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RiskCategory',
                    flex: 1,
                    text: 'Категория',
                    allowBlank: false,
                    renderer: function(val) {
                        if (val && val.Name) {
                            return val.Name;
                        }

                        return '';
                    },
                    editor: {
                        xtype: 'b4selectfieldeditor',
                        store: riskCategoryStore,
                        modalWindow: true,
                        textProperty: 'Name',
                        editable: false,
                        isGetOnlyIdProperty: false,
                        columns: [
                            {
                                text: 'Наименование',
                                dataIndex: 'Name',
                                flex: 1,
                                filter: { xtype: 'textfield' }
                            }
                        ]
                    },
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 100,
                    allowBlank: false,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 100,
                    allowBlank: false,
                    editor: {
                        xtype: 'datefield',
                        format: 'd.m.Y'
                    },
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?',
                            function(result) {
                                if (result === 'yes') {
                                    store.remove(rec);
                                }
                            }, me);
                    },
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    handler: function() {
                                        var rec = store.model.create(),
                                            plugin = me.getPlugin('cellEditing');

                                        rec.set('Inspection', getInspectionId());

                                        store.insert(0, rec);
                                        plugin.startEditByPosition({ row: 0, column: 0 });
                                    }
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        store.load();
                                    }
                                },
                                {
                                    xtype: 'b4savebutton',
                                    handler: function () {
                                        var modifiedRecs = store.getModifiedRecords(),
                                            removedRecs = store.getRemovedRecords(),
                                            result = true;

                                        if (modifiedRecs.length > 0 || removedRecs.length > 0) {
                                            Ext.each(modifiedRecs, function (rec) {
                                                return result =
                                                    !Ext.isEmpty(rec.get('RiskCategory')) &&
                                                    !Ext.isEmpty(rec.get('StartDate')) &&
                                                    !Ext.isEmpty(rec.get('EndDate'));
                                            });

                                            if (!result) {
                                                Ext.Msg.alert('Предупреждение', 'Необходимо заполнить все поля');
                                                return;
                                            }

                                            me.mask('Сохранение', me);
                                            store.sync({
                                                callback: function () {
                                                    me.unmask();
                                                    me.up('formwindow').saved = true;
                                                    store.load();
                                                },
                                                failure: function (result) {
                                                    me.unmask();
                                                    if (result && result.exceptions[0] && result.exceptions[0].response) {
                                                        Ext.Msg.alert('Ошибка!', Ext.JSON.decode(result.exceptions[0].response.responseText).message);
                                                    }
                                                },
                                                scope: me
                                            });
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});