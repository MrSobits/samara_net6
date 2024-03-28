Ext.define('B4.view.actcheck.actioneditwindowbaseitem.ViolationGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.actcheckactionviolationgrid',
    title: 'Нарушения',

    // Приписка к itemId
    itemIdInnerMessage: '',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            itemId: 'actCheckActionViolationGrid' + me.itemIdInnerMessage,
            store: 'actcheck.ActionViolation' + me.itemIdInnerMessage,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Violation',
                    text: 'Нарушение',
                    flex: 1,
                    renderer: function (value) {
                        return value?.Name;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContrPersResponse',
                    text: 'Пояснение контролируемого лица',
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
                                    xtype: 'button',
                                    itemId: 'addButton',
                                    iconCls: 'icon-add',
                                    text: 'Добавить'
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