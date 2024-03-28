Ext.define('B4.view.objectcrmasschangestate.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.objectCrMassChangeStateGrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.form.GridStateColumn'
    ],

    store: 'ObjectCrMassChangeState',
    itemId: 'objectCrMassChangeStateGrid',
    closable: false,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 200,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramNum',
                    text: '№',
                    width: 50,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramCrName',
                    text: 'Программа',
                    width: 120,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjName',
                    text: 'Адрес',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    width: 140,
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateAcceptCrGji',
                    text: 'Принят ГЖИ',
                    format: 'd.m.Y',
                    width: 100,
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
                                },
                                {
                                    xtype: 'button',
                                    text: 'Очистить',
                                    iconCls: 'icon-delete',
                                    itemId: 'btnClearGrid'
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