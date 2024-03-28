Ext.define('B4.view.logentity.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ActionKind'
    ],

    alias: 'widget.logentitygrid',

    initComponent: function () {
        var me = this,
            store = Ext.create("B4.store.LogEntity");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                { text: 'Объект', dataIndex: 'EntityName', flex: 2, sortable: false },
                {
                    text: 'Наименование объекта', dataIndex: 'EntityDescription', flex: 2,
                    filter: { xtype: 'textfield' }
                },
                { text: 'Id объекта', dataIndex: 'EntityId', flex: 2 },
                {
                    xtype: 'datecolumn',
                    text: 'Дата/время',
                    dataIndex: 'EntityDateChange',
                    format: 'd.m.Y H:i:s',
                    flex: 2
                },
                {
                    text: 'Логин', dataIndex: 'UserLogin', flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'IP', dataIndex: 'Ip', flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Тип события', dataIndex: 'TypeAction', flex: 2,
                    filter: { xtype: 'textfield' }
                },
                { text: 'Событие', dataIndex: 'EntityTypeChange', flex: 2, renderer: function (val) { return B4.enums.ActionKind.displayRenderer(val); } }
            ],
            
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                { xtype: 'b4updatebutton' }
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