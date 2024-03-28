Ext.define('B4.view.gisGkh.PremisesGrid', {
    //extend: 'Ext.tree.Panel',
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gisgkhpremisesgrid',

    requires: [
        //'B4.view.wizard.preparedata.Wizard',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.realty.RoomType',
        'B4.ux.grid.filter.YesNo'
    ],

    store: 'gisGkh.PremisesGridStore',
    title: 'Помещения в ГИС ЖКХ',
    closable: false,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PremisesNum',
                    flex: 1,
                    text: '№ квартиры/помещения',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EntranceNum',
                    flex: 1,
                    text: 'Подъезд',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TotalArea',
                    flex: 1,
                    text: 'Общая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'GrossArea',
                    flex: 1,
                    text: 'Жилая площадь',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.realty.RoomType',
                    dataIndex: 'RoomType',
                    text: 'Тип помещения',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CadastralNumber',
                    flex: 1,
                    text: 'Кадастровый номер',
                    filter: {
                        xtype: 'textfield',
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'Matched',
                    flex: 1,
                    text: 'Сопоставлено',
                    trueText: 'Сопоставлено',
                    falseText: 'Не сопоставлено',
                    filterable: true,
                    filter: { xtype: 'b4dgridfilteryesno', operator: 'eq' }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: true,
                //    showClearAllButton: true,
                //    pluginId: 'headerFilter'
                //},
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record) {
                    var matched = record.get('Matched');
                    if (matched) {
                        return 'back-coralgreen';
                    }
                    return '';
                }
            },
            dockedItems: [
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
