Ext.define('B4.view.actionisolated.actactionisolated.DefinitionPanel', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.actactionisolateddefinitionpanel',
    
    itemId: 'actActionIsolatedDefinitionPanel',
    store: 'actactionisolated.Definition',
    layout: 'fit',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.enums.ActActionIsolatedDefinitionType'
    ],

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
                    dataIndex: 'Number',
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
                    dataIndex: 'Date',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Official',
                    flex: 1,
                    text: 'ДЛ, вынесшее определение'
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'DefinitionType',
                    enumName: 'B4.enums.ActActionIsolatedDefinitionType',
                    flex: 1,
                    text: 'Тип определения',
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
                            columns: 2,
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