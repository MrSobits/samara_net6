Ext.define('B4.view.regop.chargeperiod.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.ux.button.Update',
        'B4.ux.button.Add'
    ],

    title: 'Периоды начисления',

    alias: 'widget.chargeperiodgrid',

    closable: true,
    
    store: 'regop.ChargePeriod',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columns: [
               {
                   text: 'Наименование',
                   dataIndex: 'Name',
                   flex: 1
               },
               {
                   xtype: 'datecolumn',
                   format: 'd.m.Y',
                   text: 'Дата открытия',
                   dataIndex: 'StartDate',
                   flex: 1
               },
               {
                   xtype: 'datecolumn',
                   format: 'd.m.Y',
                   text: 'Дата закрытия',
                   dataIndex: 'EndDate',
                   flex: 1
               },
               {
                   text: 'Состояние',
                   dataIndex: 'IsClosed',
                   renderer: function(value) {
                       return value == null ? '' : (value ? 'Закрыт' : 'Открыт');
                   },
                   flex: 1
               }
            ],

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
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    action: 'CloseCurrentPeriod',
                                    text: 'Закрыть текущий период',
                                    iconCls: ''
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);

    }
});