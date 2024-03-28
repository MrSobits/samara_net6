Ext.define('B4.view.realityobj.protocol.DecisionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.roprotdecisiongrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.store.PropertyOwnerProtocolsDecision',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.enums.MethodFormFundCr'
    ],

    store: 'PropertyOwnerProtocolsDecision',
    title: 'Повестки ОСС',
    disabled: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    dataIndex: 'Decision',
                    flex: 1,
                    text: 'Повестка ОСС'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Файл',
                    dataIndex: 'DocumentFile',
                    width: 100,
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            //plugins: ['B4.ux.grid.plugin.HeaderFilters'],
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
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});