Ext.define('B4.view.realityobj.CouncillorsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjcouncillorsgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeCouncillors'
    ],

    title: 'Члены совета МКД',
    store: 'realityobj.Councillors',
    itemId: 'councillorsGrid',
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    },
                    flex: 1,
                    text: 'ФИО'
                },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'Post',
                      flex: 1,
                      text: 'Должность',
                      editor:
                        {
                            xtype: 'combobox', editable: false,
                            store: B4.enums.TypeCouncillors.getStore(),
                            displayField: 'Display',
                            valueField: 'Value'
                        },
                      renderer: function (val) {
                          return B4.enums.TypeCouncillors.displayRenderer(val);
                      }
                  },

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Phone',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 50
                    },
                    flex: 1,
                    text: 'Телефон'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Email',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 100
                    },
                    flex: 1,
                    text: 'E-mail'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
                plugins: [
                    Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                    Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
                
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
                            columns: 3,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    itemId: 'councilSaveButton',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});