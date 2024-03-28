Ext.define('B4.view.protocolrso.DefinitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeDefinitionProtocolProsecutor'
    ],

    alias: 'widget.protocolRSODefinitionGrid',
    title: 'Определения',
    store: 'protocolrso.Definition',
    itemId: 'protocolRSODefinitionGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер документа'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExecutionDate',
                    text: 'Дата исполнения',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssuedDefinition',
                    flex: 1,
                    text: 'ДЛ, вынесшее определение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDefinition',
                    flex: 1,
                    text: 'Тип определения',
                    renderer: function (val) { return B4.enums.TypeDefinitionProtocolProsecutor.displayRenderer(val); }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
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