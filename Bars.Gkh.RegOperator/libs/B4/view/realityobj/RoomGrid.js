Ext.define('B4.view.realityobj.RoomGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjroomgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.realty.RoomType',
        'B4.enums.RoomOwnershipType',
        'B4.form.ComboBox'
    ],

    title: 'Сведения о помещениях',
    store: 'realityobj.Room',
    closable: true,

    initComponent: function () {
        var me = this,
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            });

        Ext.applyIf(me, {
            selModel: selModel,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomNum',
                    flex: 1,
                    text: '№ квартиры/помещения',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ChamberNum',
                    flex: 1,
                    text: '№ комнаты',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    dataIndex: 'Entrance',
                    flex: 1,
                    text: 'Номер подъезда',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val ? val : '-';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
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
                    dataIndex: 'Type',
                    flex: 1,
                    text: 'Тип помещения',
                    renderer: function (val) {
                        return B4.enums.realty.RoomType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.realty.RoomType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnershipType',
                    flex: 1,
                    text: 'Тип собственности',
                    renderer: function (val) {
                        return B4.enums.RoomOwnershipType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.RoomOwnershipType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountsNum',
                    flex: 1,
                    text: 'Количество абонентов',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Notation',
                //    flex: 1,
                //    text: 'Примечание',
                //    filter: { xtype: 'textfield' }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CadastralNumber',
                    flex: 1,
                    text: 'Кадастровый номер',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    action: 'selectentrance',
                                    text: 'Определить подъезд'
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