Ext.define('B4.view.gisaddressmatching.GisGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.enums.TypeAddressMatched',
        'B4.form.EnumCombo',
        'B4.store.gisaddressmatching.GisAddress'
    ],

    alias: 'widget.gisgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create("B4.store.gisaddressmatching.GisAddress");

        Ext.apply(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    tooltip: 'Сопоставить адрес вручную',
                    align: 'center',
                    width: 20,
                    icon: B4.Url.content('content/img/icons/database_edit.png'),
                    handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this;
                        var scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'match', rec);
                    },
                    scope: me
                },
                {
                    text: 'Регион',
                    dataIndex: 'RegionName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Населенный пункт',
                    dataIndex: 'CityName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Улица',
                    dataIndex: 'StreetName',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Номер дома',
                    dataIndex: 'Number',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Поставщик',
                    dataIndex: 'Supplier',
                    flex: 2,
                    filter: {
                        xtype: 'textfield'
                    }
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record) {
                    var matched = record.get('TypeAddressMatched');
                    if (matched) {
                        switch (matched) {
                        case 20:
// адрес сопоставлен, но не найден
                            return 'back-coralyellow-noimportant';
                        case 30:
// адрес сопоставлен и найден, но не найден дом в ЖФ
                            return 'back-coralgreen-noimportant';
                        case 40:
// адрес сопоставлен и найден
                            return '';
                        }
                    }

                    return 'back-coralred-noimportant';
                }
            },
            dockedItems: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    name: 'billingAddrInfo'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Присвоить муниципальный район',
                                    iconCls: 'icon-add',
                                    name: 'addMoButton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Присвоить тип дома',
                                    iconCls: 'icon-add',
                                    name: 'addTypeButton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Фильтр адресов',
                                    dataIndex: 'TypeAddressMatched',
                                    name: 'cmbTypeAddressMatched',
                                    enumName: 'B4.enums.TypeAddressMatched',
                                    editable: false,
                                    emptyText: '-',
                                    emptyItem: { Display: '-' },
                                    width: 500,
                                    tpl: Ext.create('Ext.XTemplate',
                                        '<tpl for=".">',
                                        '<div class="x-boundlist-item {[this.getClass(values)]}">{Display}</div>',
                                        '</tpl>',
                                        {
                                            getClass: function(rec) {
                                                switch (rec.Value) {
                                                case 10:
                                                    return 'back-coralred-noimportant';
                                                case 20:
                                                    return 'back-coralyellow-noimportant';
                                                case 30:
                                                    return 'back-coralgreen-noimportant';
                                                default:
                                                case 40:
                                                    return '';
                                                }
                                            }
                                        }
                                    )
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