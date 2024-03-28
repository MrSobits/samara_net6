Ext.define('B4.view.resolpros.DefinitionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeResolProsDefinition'
    ],

    alias: 'widget.resolprosdefinitiongrid',
    title: 'Определения',
    store: 'resolpros.Definition',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата определения',
                    format: 'd.m.Y',
                    width: 120
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ExecutionDate',
                    text: 'Дата рассмотрения',
                    format: 'd.m.Y',
                    width: 120
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IssuedDefinition',
                    flex: 1,
                    text: 'ДЛ, вынесшее определение'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeResolProsDefinition',
                    flex: 1,
                    text: 'Тип документа',
                    renderer: function(val) {
                         return B4.enums.TypeResolProsDefinition.displayRenderer(val);
                    }
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