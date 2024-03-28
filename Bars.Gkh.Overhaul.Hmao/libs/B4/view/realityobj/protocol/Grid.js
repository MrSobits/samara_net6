Ext.define('B4.view.realityobj.protocol.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.roprotocolgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.store.PropertyOwnerProtocols',
        'B4.enums.PropertyOwnerProtocolType'
    ],

    title: 'Протоколы',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.PropertyOwnerProtocols');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'ProtocolTypeId',
                    flex: 1,
                    text: 'Тип протокола'
                },
                {
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер протокола'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата принятия протокола'
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
            ]
        });
        me.callParent(arguments);
    }
});