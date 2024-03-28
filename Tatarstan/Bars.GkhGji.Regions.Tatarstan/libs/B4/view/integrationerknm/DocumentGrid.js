Ext.define('B4.view.integrationerknm.DocumentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.integrationerknmgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeDocumentGji',
        'B4.store.integrationerknm.Document'
    ],

    title: 'Интеграция с ЕРКНМ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.integrationerknm.Document');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    name: 'showDocument',
                    width: 20,
                    icon: 'content/img/searchfield-icon.png'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypeDocumentGji',
                    dataIndex: 'DocumentType',
                    text: 'Тип документа',
                    enumItems: [
                        B4.enums.TypeDocumentGji.WarningDoc,
                        B4.enums.TypeDocumentGji.PreventiveAction,
                        B4.enums.TypeDocumentGji.Decision
                    ],
                    filter: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер документа',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LastMethodStartTime',
                    text: 'Дата последнего запроса',
                    format: 'd.m.Y H:i:s',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErknmGuid',
                    text: 'Идентификатор в ЕРКНМ',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ErknmRegistrationDate',
                    text: 'Дата присвоения идентификатора ЕРКНМ',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
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
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в Excel',
                                    itemId: 'btnExport'
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