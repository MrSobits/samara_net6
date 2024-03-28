Ext.define('B4.view.dict.normativedoc.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhIntField',
        'B4.enums.NormativeDocCategory'
    ],

    title: 'Нормативные документы',
    store: 'dict.NormativeDoc',
    alias: 'widget.normativeDocGrid',
    closable: true,

    // необходимо для того чтобы неработали восстановления для грида посколкьу колонки показываются и скрываются динамически
    // не ставьте пожалута сохранение этого грида в local storage потмоуч тов зависимости от прав в контролелере скрывабются или показываются колонки этого грида если по ставите сохранение то будет падать
    provideStateId: Ext.emptyFn,
    stateful: false,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1.7,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FullName',
                    flex: 1.7,
                    text: 'Полное наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 1000
                    },
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 0.3,
                    text: 'Код',
                    editor: {
                        xtype: 'gkhintfield',
                        operand: CondExpr.operands.eq,
                        minValue: 0
                    },
                    filter: {
                        xtype: 'gkhintfield',
                        operand: CondExpr.operands
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.NormativeDocCategory',
                    dataIndex: 'Category',
                    text: 'Категория',
                    flex: 0.7,
                    filter: true
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    text: 'Действует с',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    flex: 0.4
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateTo',
                    text: 'Действует по',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    flex: 0.4
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});