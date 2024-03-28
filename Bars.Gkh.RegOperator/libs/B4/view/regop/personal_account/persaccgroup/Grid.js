Ext.define('B4.view.regop.personal_account.persaccgroup.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paccountgroupgrid',

    title: 'Группы',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.store.regop.personal_account.PersonalAccountGroup',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging'
    ],

    initComponent: function() {
        var me = this;
        me.store = Ext.create('B4.store.regop.personal_account.PersonalAccountGroup');

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование группы',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me,
                    handler: null,
                    renderer: function (val, btn, rec) {
                        var deleteCol = this.columns[1];
                        if (rec.get('IsSystem') == B4.enums.YesNo.Yes) {
                            deleteCol.disableAction();
                        } else {
                            deleteCol.enableAction();
                        }

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
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Добавить'
                                },
                                {
                                    itemId: 'btnUpdateGroups',
                                    text: 'Обновить',
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});

