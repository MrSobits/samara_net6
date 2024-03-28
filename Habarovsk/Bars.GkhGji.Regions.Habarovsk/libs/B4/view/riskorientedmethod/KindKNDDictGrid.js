Ext.define('B4.view.riskorientedmethod.KindKNDDictGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.enums.KindKND',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Справочник КНД',
    store: 'riskorientedmethod.KindKNDDict',
    alias: 'widget.kindknddictgrid',
    closable: true,

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                 {
                     xtype: 'b4editcolumn',
                     scope: me
                 },
                 {
                   xtype: 'gridcolumn',
                   dataIndex: 'KindKND',
                   flex: 1,
                   text: 'Вид КНД',
                   renderer: function (val) {
                       return B4.enums.KindKND.displayRenderer(val);
                   },
                   editor: {
                       xtype: 'b4combobox',
                       valueField: 'Value',
                       displayField: 'Display',
                       items: B4.enums.KindKND.getItems(),
                       editable: false
                   }
               },
                  {
                      xtype: 'datecolumn',
                      dataIndex: 'DateFrom',
                      text: 'Дата начала действия',
                      format: 'd.m.Y',
                      flex: 0.5,
                      width: 80
                   
                  },
                    {
                        xtype: 'datecolumn',
                        dataIndex: 'DateTo',
                        text: 'Дата окончания действия',
                        format: 'd.m.Y',
                        flex: 0.5,
                        width: 80
                      
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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