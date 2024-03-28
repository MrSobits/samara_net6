Ext.define('B4.view.menu.ManagingOrgDataGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.Url'
    ],
    title: 'Сведения об управляющей организации',
    store: null,
    itemId: 'managingOrgDataGrid',
    sortableColumns: false,
    border: false,

    initComponent: function (config) {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'text',
                    flex: 1,
                    text: 'Раздел'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'percent',
                    width: 100,
                    align: 'center',
                    text: '%',
                    tdCls: 'x-progress-cell',
                    renderer: function (value) {
                        if (value != '-') {
                            var value = value.slice(0, -2);
                            return value + '%';                         
                        }
                        return value;
                    }
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'controller',
                    align: 'center',
                    width: 130,
                    icon: B4.Url.content('content/img/btn.png'),
                    iconCls: 'icon-fill-button',
                    text: 'Действия',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'click', rec);
                    },
                    scope: me
                }
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var c = parseFloat(record.get('percent'));
                    if (c == isNaN) {
                        return '';
                    }

                    if (c == 100) {
                        return 'x-percent-100';
                    }
                    
                    if (c <= 10) {
                        return 'x-percent-10';
                    } else if (c > 10 && c <= 20) {
                        return 'x-percent-20';
                    } else if (c > 20 && c <= 40) {
                        return 'x-percent-30';
                    } else if (c > 40 && c <= 70) {
                        return 'x-percent-70';
                    } else if (c > 70) {
                        return 'x-percent-90';
                    };
                    return '';
                }
            },
            dockedItems: [
               {
                   xtype: 'toolbar',
                   dock: 'top',
                   items: [
                       {
                           xtype: 'buttongroup',
                           columns: 1,
                           items: [
                               {
                                   xtype: 'button',
                                   text: 'Действия',
                                   iconCls: 'icon-arrow-nw-ne-sw-se',
                                   itemId: 'btnManorgActionDi',
                                   menu: []
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