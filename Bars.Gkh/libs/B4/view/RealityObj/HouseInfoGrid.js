Ext.define('B4.view.realityobj.HouseInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realityobjhouseinfogrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.KindRightToObject'
    ],

    title: 'Сведения о помещениях',
    store: 'realityobj.HouseInfo',
    
    closable: true,

    initComponent: function () {
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
                    flex: 2,
                    text: 'Наименование'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumObject',
                    flex: 1,
                    text: '№ объекта'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalArea',
                    flex: 1,
                    text: 'Площадь'
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'UnitMeasure',
                     flex: 1,
                     text: 'Единица измерения'
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindRight',
                    flex: 2,
                    text: 'Вид права (ограничения)</br> в отношении объекта',
                    renderer: function (val) {
                        return B4.enums.KindRightToObject.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumRegistrationOwner',
                    flex: 1,
                    text: '№ зарег. права / ограничения'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRegistration',
                    text: 'Дата регистрации права',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Owner',
                    flex: 1,
                    text: 'Правообладатель'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateRegistrationOwner',
                    text: 'Дата рождения правообладателя',
                    format: 'd.m.Y',
                    width: 100
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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