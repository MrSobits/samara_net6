Ext.define('B4.view.regop.personal_account.privilegedcategory.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paccountprivilegedcategorygrid',

    title: 'Категория льготы',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.regop.personal_account.PrivilegedCategory',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PrivilegedCategory');

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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата начала действия'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateTo',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата окончания действия'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Percent',
                    flex: 1,
                    text: 'Процент льготы',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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

