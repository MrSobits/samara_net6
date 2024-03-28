Ext.define('B4.view.administration.ExportableTypesGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.exportabletypesgrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Close',

        'B4.form.SelectField',
        'B4.store.administration.ExportableType'
    ],

    initComponent: function () {
        var me = this;
        me.store = Ext.create('B4.store.administration.ExportableType');

        Ext.applyIf(me, {
            columnLines: true,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),

            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Наименование',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 100
                    },
                    filter: { xtype: 'textfield' }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
                    dock: 'bottom'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'RunIntegration',
                                    text: 'Выполнить',
                                    typeIntegration: 'manual',
                                    iconCls: 'icon-add'
                                },
                                {
                                    xtype: 'button',
                                    action: 'RunIntegration',
                                    text: 'Выполнить полную интеграцию',
                                    typeIntegration: 'full',
                                    iconCls: 'icon-add'
                                }
                            ]
                        },
                        {
                            xtype: 'checkbox',
                            name: 'exportDependencies',
                            margin: '0 0 0 10',
                            boxLabel: 'Выгружать зависимые секции'
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    handler: function (button) {
                                        button.up('window').close();
                                    }
                                }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

