Ext.define('B4.view.administration.fieldrequirement.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'Ext.ux.CheckColumn'
    ],

    title: 'Обязательность полей',
    store: 'administration.FieldRequirement',
    alias: 'widget.fieldrequirementgrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectName',
                    flex: 2,
                    text: 'Объект',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FieldName',
                    flex: 2,
                    text: 'Наименование поля',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'checkcolumn',
                    dataIndex: 'Required',
                    flex: 1,
                    text: 'Обязательно для заполнения'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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