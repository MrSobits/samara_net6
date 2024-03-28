Ext.define('B4.view.actcheck.actioneditwindowbaseitem.RemarkGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.actcheckactionremarkgrid',
    title: 'Замечания',

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            itemId: 'actCheckActionRemarkGrid' + me.itemIdInnerMessage,
            store: 'actcheck.ActionRemark' + me.itemIdInnerMessage,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Remark',
                    text: 'Замечание',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MemberFio',
                    text: 'ФИО участника',
                    flex: 1,
                    editor: {
                        xtype: 'textfield',
                        maxLength: 500
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
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 2,
                    pluginId: 'cellEditing'
                })
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'saveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
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