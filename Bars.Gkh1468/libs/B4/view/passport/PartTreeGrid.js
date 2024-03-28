Ext.define('B4.view.passport.PartTreeGrid', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.parttreegrid',

    requires: [
       'B4.ux.button.Add',
       'B4.ux.button.Update',
        'Ext.tree.plugin.TreeViewDragDrop',
        'Ext.grid.plugin.CellEditing',
        'Ext.grid.property.Grid',
        'B4.store.passport.PartTreeStore'
    ],
    controllerName: 'Part',

    width: 300,
    rootVisible: false,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            store: Ext.create('B4.store.passport.PartTreeStore'),
            plugins: [
                {
                    ptype: 'cellediting',
                    clicksToEdit: 2
                }
            ],
            viewConfig: {
                plugins: {
                    ptype: 'treeviewdragdrop',
                    ddGroup: 'StructPartDD',
                    allowContainerDrops: false
                },
                listeners: {
                    beforedrop: function (node, data, overModel, dropPos, dropFn) {
                        if (dropPos === 'append') {
                            return false;
                        }
                    },
                    drop: function (node, data, overModel, dropPosition) {
                        me.fireEvent('nodesreordered', me, data.records[0].parentNode);
                    }
                }
            },
            tbar: [
                {
                    xtype: 'b4addbutton',
                    menu: {
                        items: [
                            {
                                text: 'Раздел',
                                cmd: 'addpart'
                            },
                            {
                                text: 'Подраздел',
                                cmd: 'addsubpart'
                            }
                        ]
                    }
                },
                {
                    text: 'Обновить',
                    xtype: 'b4updatebutton'
                },
                {
                    text: 'Удалить',
                    cmd: 'removepart',
                    iconCls: 'icon-delete'
                }
            ],
            columns: [
                {
                    xtype: 'treecolumn',
                    dataIndex: 'Name',
                    editor: 'textfield',
                    text: 'Название',
                    flex: 1,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    editor: 'textfield',
                    menuDisabled: true,
                    sortable: false,
                    draggable: false
                }
            ]
        });

        me.callParent(arguments);
    }
});