Ext.define('B4.view.administration.notify.PermissionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.notifypermissiongrid',

    requires: [
        'B4.store.administration.notify.Permission',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Доступ',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.notify.Permission');

        Ext.apply(me, {
            columnLines: true,
            hideHeaders: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Role',
                    flex: 1,
                    renderer: function (role, cls, record) {
                        return record.get('RoleName');
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    border: false,
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        },
                        {
                            xtype: 'label',
                            name: 'Hint',
                            padding: 5,
                            text: 'Сообщение получат все пользователи. Выберите роли, для которых предназначено это сообщение',
                            border: false,
                            style: 'font-size: 12px;'
                        },
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