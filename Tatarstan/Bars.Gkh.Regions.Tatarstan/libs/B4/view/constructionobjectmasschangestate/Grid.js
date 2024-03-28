Ext.define('B4.view.constructionobjectmasschangestate.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.form.GridStateColumn'
    ],

    store: 'ConstructionObjectMassChangeState',
    alias: 'widget.constructionobjectmasschangestategrid',

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
                    dataIndex: 'ResettlementProgram',
                    width: 250,
                    text: 'Программа',
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
                    dataIndex: 'MunicipalityName',
                    width: 180,
                    text: 'Муниципальное образование',
                    sortable: false
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    disabled: true
                                },
                                {
                                    xtype: 'button',
                                    action: 'RemoveAll',
                                    text: 'Очистить',
                                    iconCls: 'icon-delete',
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