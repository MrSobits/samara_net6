Ext.define('B4.view.paysize.RecordGrid', {
    extend: 'Ext.tree.Panel',

    alias: 'widget.paysizerecordgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.paysize.Record'
    ],

    title: 'Муниципальные образования',

    /**
     * Переопределение чтобы не загружался если autoLoad==false
     */
    setRootNode: function () {
        if (this.getStore().autoLoad) {
            this.callParent(arguments);
        }
    },

    rootVisible: false,
    animate: false,
    autoScroll: true,
    useArrows: true,
    containerScroll: true,
    loadMask: true,
    rowLines: true,
    columnLines: true,
    displayField: 'text',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.paysize.Record');

        Ext.apply(me, {
            store: store,
            columns: [
                {
                    xtype: 'treecolumn',
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1
                },
                {
                    text: 'Размер взноса',
                    dataIndex: 'Value',
                    width: 100,
                    //editor: {
                    //    xtype: 'numberfield',
                    //    decimalSeparator: ',',
                    //    minValue: 0,
                    //    allowDecimal: true
                    //},
                    renderer: function(val) {
                        return val >= 0 ? Ext.util.Format.currency(val, null, 2) : '';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Искл. по типу дома',
                    tooltip: "Редактировать",
                    width: 120,
                    scope: me,
                    align: 'center',
                    icon: B4.Url.content('content/img/icons/pencil.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        me.fireEvent('rowaction', me, 'edit', rec);
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1
                })
            ],
            viewConfig: {
                loadMask: true
            },
            loader: {
                autoLoad: false
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
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
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