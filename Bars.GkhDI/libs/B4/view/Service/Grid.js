Ext.define('B4.view.service.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit', 
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TariffIsSetForDi'
    ],

    title: 'Сведения об услугах',
    store: 'service.Base',
    itemId: 'serviceGrid',
    closable: true,
    
    initComponent: function() {
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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProviderName',
                    flex: 1,
                    text: 'Поставщик',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    width: 80,
                    text: 'Тариф',
                    filter: { xtype: 'gkhdecimalfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DateStart',
                    width: 125,
                    text: 'Дата начала действия',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TariffIsSetFor',
                    flex: 1,
                    text: 'Тариф установлен',
                    sortable: false,
                    renderer: function (val, meta, record) {
                        if (record.get('KindServiceDi') == '20' || record.get('KindServiceDi') == '30' || record.get('KindServiceDi') == '40') {
                            return '';
                        }
                        return val == '' ? val : B4.enums.TariffIsSetForDi.displayRenderer(val);
                    }
                },
                {
                    text: 'Группа услуги',
                    dataIndex: 'TypeGroupServiceDi',
                    flex: 1,
                    hidden: true,
                    renderer: function(val) {
                        switch (val) {
                            case 10:
                                return 'Коммунальные';
                            case 20:
                                return 'Жилищные';
                            default :
                                return 'Прочие';
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'percent',
                    width: 100,
                    align: 'center',
                    text: '%',
                    tdCls: 'x-progress-cell'
                },
                {
                    xtype: 'actioncolumn',
                    width: 75,
                    icon: B4.Url.content('content/img/icons/door_in.png'),
                    text: 'Копировать',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'copy', rec);
                    },
                    scope: me
                },                
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [Ext.create('Ext.grid.feature.Grouping', {
                groupHeaderTpl: '{name}'
            })],
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
                    } else if (c > 70 && c <= 99) {
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
                                    itemId: 'serviceAddButton',
                                    text: 'Добавить',
                                    tooltip: 'Добавить',
                                    iconCls: 'icon-add'
                                }
                            ]
                        },
                        {
                            xtype: 'tbspacer',
                            width: 50
                        },
                        {
                            xtype: 'label',
                            itemId: 'lbCountDifficit',
                            width: 300
                        },
                        {
                             xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    itemId: 'btnUnfilledMandatoryServs',
                                    text: 'Какие услуги не добавлены?',
                                    tooltip: 'Какие услуги не добавлены?',
                                    iconCls: 'icon-help'
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