Ext.define('B4.view.menu.ManagingOrgRealityObjDataGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.Url',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],
    title: 'Объекты в управлении',
    store: 'menu.ManagingOrgRealityObjDataMenu',
    itemId: 'managingOrgRealityObjDataGrid',
    border: false,

    initComponent: function () {
        var me = this;

        var renderer = function (val, meta, rec) {
            if (!rec.get('HouseAccounting')) {
                meta.style = 'background: silver; line-height: 16px;';
                meta.tdAttr = 'data-qtip="Информация по данному дому не используется при расчете общего процента раскрытия информации"';
            }
            return val;
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AddressName',
                    flex: 2,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    renderer: function(val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaLiving',
                    flex: 0.3,
                    text: 'Жилая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'percent',
                    width: 100,
                    align: 'center',
                    text: '%',
                    tdCls: 'x-progress-cell',
                    renderer: function (value, meta, rec) {
                        return value + '%';
                    }
                },
                {
                    xtype: 'actioncolumn',
                    flex: 0.35,
                    icon: B4.Url.content('content/img/btn.png'),
                    iconCls: 'icon-fill-button',
                    align: 'center',
                    text: 'Действия',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'click', rec);
                    },
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                    itemId: 'btnActionDi',
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