Ext.define('B4.view.dict.ControlType.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ControlLevel'
    ],

    title: 'Виды контроля',
    store: 'dict.ControlType',
    alias: 'widget.controltypegrid',
    closable: true,

    initComponent: function () {
        var me = this
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Наименование вида контроля (надзора)',
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Level',
                    enumName: 'B4.enums.ControlLevel',
                    flex: 1,
                    filter: true,
                    text: 'Уровень',
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ErvkId',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Идентификатор вида контроля из ЕРВК',
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
                    layout: 'vbox',
                    dock: 'top',
                    padding: 5,
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
                        },
                        {
                            xtype: 'textfield',
                            name: 'ControlTypeGuid',
                            fieldLabel: 'Идентификатор справочника',
                            readOnly: true,
                            maxLength: 36,
                            padding: '5 0',
                            labelWidth: 160,
                            width: 450
                        },
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    dock: 'bottom',
                    displayInfo: true,
                    store: this.store
                }
            ]
        });

        me.callParent(arguments);
    }
});