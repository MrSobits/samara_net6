Ext.define('B4.view.al.DataSourceGrid', {
    extend: 'Ext.tree.Panel',

    requires: [
        'B4.base.Store',
        'B4.base.Proxy',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',

        'B4.enums.al.OwnerType'
    ],

    alias: 'widget.datasourcegrid',
    title: 'Источники данных',

    closable: true,
    rootVisible: false,
    useArrows: true,

    initComponent: function () {
        var me = this;

        me.store = Ext.create('Ext.data.TreeStore', {
            fields: ['text', 'id', 'type', 'subroot'],
            proxy: {
                type: 'memory'
            },
            root: {
                text: 'Root',
                expanded: true,
                leaf: false,
                children: []
            }
        });

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    renderer: function (value, meta, record) {
                        var col = this;
                        col.icon = !record.get('subroot') ? '' : B4.Url.content('content/img/icons/pencil.png');
                        return value;
                    },
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var col = this,
                            scope = col.origScope;

                        if (!rec.get('subroot')) {
                            return;
                        }

                        if (!scope) {
                            scope = col.up('treepanel');
                        }

                        scope.fireEvent('rowaction', scope, 'edit', rec);
                    }
                },
                {
                    xtype: 'treecolumn',
                    dataIndex: 'text',
                    text: 'Название',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'type',
                    text: 'Тип',
                    enumName: 'B4.enums.al.OwnerType',
                    flex: 1
                },
                {
                    dataIndex: 'Description',
                    text: 'Описание',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    renderer: function (value, meta, record) {
                        var col = this;
                        col.icon = !record.get('subroot') ? '' : B4.Url.content('content/img/icons/delete.png');
                        return value;
                    },
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var col = this,
                            scope = col.origScope;

                        if (!rec.get('subroot')) {
                            return;
                        }

                        if (!scope) {
                            scope = col.up('treepanel');
                        }

                        scope.fireEvent('rowaction', scope, 'delete', rec);
                    }
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
                }
            ]
        });

        me.callParent(arguments);
    }
});