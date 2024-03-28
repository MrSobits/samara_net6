Ext.define('B4.view.administration.instructiongroups.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.instructionGroupsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.administration.instruction.InstructionGroups'
    ],

    title: 'Документация',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.instruction.InstructionGroups');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisplayName',
                    flex: 1,
                    text: 'Наименование категории документов'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Role',
                    flex: 1,
                    text: 'Доступно ролям'
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
                            columns: 2,
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