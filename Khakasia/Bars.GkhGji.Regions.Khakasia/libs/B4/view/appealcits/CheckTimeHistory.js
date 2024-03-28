Ext.define('B4.view.appealcits.CheckTimeHistory', {
    extend: 'Ext.window.Window',
    alias: 'widget.appealCitsCheckTimeHistory',
    requires: [
        'B4.store.appealcits.CheckTimeChange',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    modal: true,

    width: 500,
    height: 350,
    layout: 'fit',
    title: 'История изменений контролного срока',
    itemId: 'checkTimeHistory',
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.appealcits.CheckTimeChange');
            
        Ext.apply(me, {
            store: store,
            items: {
                xtype: 'grid',
                border: 0,
                store: store,
                columns: [
                    {
                        text: 'Дата изменения',
                        dataIndex: 'CreateDate',
                        xtype: 'datecolumn',
                        format: 'd.m.Y H:i:s',
                        flex: 1
                    },
                    {
                        text: 'Пользователь',
                        dataIndex: 'UserName',
                        flex: 1
                    },
                    {
                        text: 'Старый срок',
                        dataIndex: 'OldValue',
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    {
                        text: 'Новый срок',
                        dataIndex: 'NewValue',
                        xtype: 'datecolumn',
                        format: 'd.m.Y',
                        flex: 1
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
                                columns: 1,
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
                ]
            }
        });

        me.callParent(arguments);
    }
});