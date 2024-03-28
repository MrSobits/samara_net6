Ext.define('B4.view.emailnewsletter.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.button.Update',
        'B4.ux.button.Add',

        'B4.store.emailnewsletter.EmailNewsletter'
    ],

    title: 'Реестр рассылок на электронную почту',

    alias: 'widget.emailnewslettergrid',

    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.emailnewsletter.EmailNewsletter');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    text: 'Дата отправки',
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'SendDate',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Тема',
                    dataIndex: 'Header',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Отправитель',
                    dataIndex: 'Sender',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'Success',
                    text: 'Отправлено',
                    flex: 1,
                    renderer: function (val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
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
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ]
        });

        me.callParent(arguments);
    }
});