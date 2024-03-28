Ext.define('B4.view.dict.violationfeaturegji.ViolationGroupsTree', {
    extend: 'Ext.tree.Panel',

    alias: 'widget.violationgroupstree',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.store.dict.FeatureViolGjiTree'
    ],

    title: 'Группы нарушений',

    rootVisible: false,
    useArrows: true,
    rowLines: true,
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.dict.FeatureViolGjiTree');

        me.addEvents(
            'endcelledit'
        );

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'treecolumn',
                    text: 'Наименование',
                    dataIndex: 'Name',
                    tooltip: 'Редактировать по двойному клику',
                    flex: 1,
                    editor: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    width: 100,
                    editor: {
                        xtype: 'textfield'
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 2,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function(editor, e) {
                            var rec = e.record;
                            me.fireEvent('endcelledit', rec, 'update', rec.raw.Parent, this);
                        }
                    }
                })
            ],
            viewConfig: {
                markDirty: false,
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4addbutton',
                                    menu: {
                                        xtype: 'menu',
                                        items: [
                                            {
                                                text: 'Добавить группу',
                                                action: 'addgroup',
                                                iconCls: 'icon-add'
                                            },
                                            {
                                                text: 'Добавить подгруппу',
                                                action: 'addsubgroup',
                                                iconCls: 'icon-add'
                                            }
                                        ]
                                    }
                                },
                                {
                                    xtype: 'button',
                                    action: 'delete',
                                    text: 'Удалить',
                                    iconCls: 'icon-cross'
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