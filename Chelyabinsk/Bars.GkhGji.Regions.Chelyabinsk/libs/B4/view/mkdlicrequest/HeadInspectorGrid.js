Ext.define('B4.view.mkdlicrequest.HeadInspectorGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.mkdlicrequest.HeadInspector',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    title: 'Вложения',
    store: 'mkdlicrequest.HeadInspector',
    alias: 'widget.mkdLicRequestHeadInspectorGrid',
    columnLines: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'gridcolumn',
                    text: 'ФИО',
                    dataIndex: 'Fio',
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Должность',
                    dataIndex: 'Position',
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Номер телефона',
                    dataIndex: 'Phone',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    text: 'E-mail',
                    dataIndex: 'Email',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
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
                    store: this.store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});