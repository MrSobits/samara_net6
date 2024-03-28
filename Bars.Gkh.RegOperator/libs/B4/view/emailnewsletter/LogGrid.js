Ext.define('B4.view.emailnewsletter.LogGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Update',

        'B4.store.emailnewsletter.EmailNewsletterLog'
    ],

    title: 'Лог',
    itemId: 'emailNewsletterLogGrid',
    alias: 'widget.emailnewsletterLogGrid',
    minHeight: 410,
    autoScroll: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.emailnewsletter.EmailNewsletterLog');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    text: 'Адресат',
                    dataIndex: 'Destination',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Лог',
                    dataIndex: 'Log',
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