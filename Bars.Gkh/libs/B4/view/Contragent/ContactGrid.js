Ext.define('B4.view.contragent.ContactGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Контакты',
    store: 'contragent.Contact',
    alias: 'widget.contragentContactGrid',
    closable: true,

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
                    dataIndex: 'FullName',
                    flex: 1,
                    text: 'ФИО'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStartWork',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEndWork',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Position',
                    flex: 1,
                    text: 'Должность'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    flex: 1,
                    text: 'Телефон'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Email',
                    flex: 1,
                    text: 'E-mail'
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
                            columns: 3,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Заполнить из реестра должностных лиц',
                                    textAlign: 'left',
                                    itemId: 'btnFillDL'
                                },
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