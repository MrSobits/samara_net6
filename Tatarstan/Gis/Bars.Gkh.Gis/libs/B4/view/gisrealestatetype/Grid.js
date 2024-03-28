Ext.define('B4.view.gisrealestatetype.Grid', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.gisrealestatetypegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.gisrealestate.realestatetype.RealEstateType'
    ],

    title: 'Типы домов',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.gisrealestate.realestatetype.RealEstateTypeGrouped');

        Ext.apply(me, {
            store: store,
            rootVisible: false,
            cls: 'no-tree-icons',
            rowLines: true,
            columns: [
                {
                    xtype: 'treecolumn',
                    width: 20
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    items: [
                        {
                            getClass: function (v, meta, record) {
                                if (record.get('Id') == 'new') {
                                    return 'icon-add';
                                } else {
                                    if (record.get('allowEditor')) {
                                        return 'icon-disk-black';
                                    } else {
                                        return 'icon-pencil';
                                    }
                                }
                            },
                            handler: function (gridview, row, column, btn, meta, record) {
                                me.fireEvent('actionClick', me, record);
                            }
                        }
                    ]
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' },
                    getEditor: function (record) {
                        if (!record.get('allowEditor') && record.get('Id') != 'new') return null;

                        return Ext.create('Ext.grid.CellEditor', {
                            field: Ext.create('Ext.form.field.Text', {
                                allowBlank: false
                            })
                        });
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            viewConfig: {
                getRowClass: function (record) {
                    if (record.get('Entity') == 'RealEstateTypeGroup') {
                        return 'font-blue-noimportant';
                    }
                }
            },
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1
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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    name: 'AddGroup',
                                    text: 'Добавить группу',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'button',
                                    name: 'AddType',
                                    text: 'Добавить тип дома',
                                    iconCls: 'icon-add'
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