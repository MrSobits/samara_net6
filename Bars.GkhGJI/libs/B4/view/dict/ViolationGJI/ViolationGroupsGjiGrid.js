Ext.define('B4.view.dict.violationgji.ViolationGroupsGjiGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update'
    ],

    title: 'Группы нарушений',
    store: 'dict.ViolationFeatureGroupsGji',
    alias: 'widget.violationGroupsGjiGrid',
    itemId: 'violationGroupsGjiGrid',
    closable: false,
    stateful: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    width: 70,
                    filter: { xtype: 'textfield' },
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FullName',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    text: 'Наименование'
                }
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
                            items: [
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