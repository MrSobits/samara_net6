Ext.define('B4.view.administration.instructiongroups.RoleGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.instructiongroupsrolegrid',
    requires: [
        'B4.store.Role',
        'B4.ux.grid.column.Delete',
        'B4.store.administration.instruction.InstructionGroupRole'
    ],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.instruction.InstructionGroupRole');

        Ext.applyIf(me, {
            columnLines: true,
            hideHeaders: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Role',
                    flex: 1,
                    renderer: function (data) {
                        if (data)
                            return data.Name;
                        return '-';
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
                    xtype: 'label',
                    text: 'Категория доступна всем. Выберите роли, для которых предназначена эта категория:',
                    dock: 'top',
                    border: 0,
                    style: 'text-align: center'
                },
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