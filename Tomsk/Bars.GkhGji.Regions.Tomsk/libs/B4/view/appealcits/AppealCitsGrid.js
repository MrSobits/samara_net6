/*
* Грид обращений граждан. Для добавления в проверку
*/
Ext.define('B4.view.appealcits.AppealCitsGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.appealCitsBaseStatGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete'
    ],
    
    title: 'Обращения',
    itemId: 'appealCitsBaseStatGrid',
    closable: false,
    store: 'appealcits.AppealCitsBaseStatement',
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер обращения',
                    sortable: false
                    
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    flex: 1,
                    text: 'Номер гжи',
                    sortable: false

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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});