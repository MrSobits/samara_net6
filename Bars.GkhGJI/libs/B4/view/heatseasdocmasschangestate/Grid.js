Ext.define('B4.view.heatseasdocmasschangestate.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.HeatingSystem',
        'B4.enums.HeatSeasonDocType',
        'B4.enums.TypeHouse'
    ],

    store: 'HeatSeasDocMassChangeState',
    itemId: 'heatSeasDocMassChangeStateGrid',
    alias: 'widget.heatSeasDocMassChangeStateGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 150,
                    scope: me,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeatingSystem',
                    width: 120,
                    text: 'Система отопления',
                    renderer: function (val) {
                        return B4.enums.HeatingSystem.displayRenderer(val);
                    },
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeDocument',
                    width: 120,
                    text: 'Тип документа',
                    renderer: function (val) {
                        return B4.enums.HeatSeasonDocType.displayRenderer(val);
                    },
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityName',
                    width: 180,
                    text: 'Муниципальное образование',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeHouse',
                    width: 120,
                    text: 'Тип дома',
                    renderer: function (val) {
                        return B4.enums.TypeHouse.displayRenderer(val);
                    },
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManOrgName',
                    flex: 1,
                    text: 'Управляющая организация',
                    sortable: false
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    disabled: true
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});