Ext.define('B4.view.regop.personal_account.action.PersonalAccountSplit.AccountsGrid', {
    extend: 'Ext.grid.Panel',

    alias: 'widget.personalaccountsplitaccountsgrid',

    requires: [
        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.column.Delete',
        'B4.view.Control.GkhDecimalField',
        'B4.ux.grid.column.AreaShare',
        'B4.form.field.AreaShareField',
        'B4.model.regop.personal_account.SplitAccountInfo'
    ],

    columnLines: true,
    cls: 'x-large-head',

    _decimalStyle: '<div style="font-size: 11px; line-height: 13px;">{0}</div>',
    _textStyle: '<div style="font-size: 11px; line-height: 13px; text-align: right">{0}</div>',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                    model: 'B4.model.regop.personal_account.SplitAccountInfo'
                });

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonalAccountNum',
                    text: 'Номер ЛС',
                    flex: 1,
                    maxWidth: 100,
                    renderer: function(val) {
                        return val || 'Новый ЛС';
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'OpenDate',
                    text: 'Дата открытия',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerType',
                    text: 'Тип абонента',
                    flex: 1,
                    maxWidth: 200,
                    renderer: function(val) { return B4.enums.regop.PersonalAccountOwnerType.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerName',
                    text: 'ФИО/Наименование абонента',
                    flex: 1,
                    summaryRenderer: function(val, summaryData) {
                        return Ext.String.format(me._textStyle, 'Итого:');
                    }
                },
                {
                    xtype: 'areasharecolumn',
                    dataIndex: 'AreaShare',
                    text: 'Доля собственности текущая',
                    width: 90,
                    summaryType: 'sum',
                    summaryRenderer: function (val, summaryData, dataIndex) {
                        return val ? Ext.String.format(me._decimalStyle, Ext.util.Format.currency(val, null, Gkh.config.RegOperator.GeneralConfig.AreaShareConfig.DecimalsAreaShare)) : '';
                    }
                },
                {
                    xtype: 'areasharecolumn',
                    dataIndex: 'NewAreaShare',
                    text: 'Доля собственности новая',
                    width: 90,
                    summaryType: 'sum',
                    editor: { xtype: 'areasharefield' },
                    summaryRenderer: function (val, summaryData, dataIndex) {
                        return val ? Ext.String.format(me._decimalStyle, Ext.util.Format.currency(val, null, Gkh.config.RegOperator.GeneralConfig.AreaShareConfig.DecimalsAreaShare)) : '';
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    renderer: function (val, meta, rec) {
                        if (rec.getId()) {
                            meta.style += 'display:none;';
                        }
                    }
                }
            ],
            plugins: [Ext.create('Ext.grid.plugin.CellEditing', {
                clicksToEdit: 1,
                pluginId: 'cellEditing'
            })],
            dockedItems:[
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    text: 'Добавить ЛС'
                                }
                            ]
                        }
                    ]
                }
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function(record, rowIndex, rowParams, store) {
                    return record.getId() ? 'back-coralred' : null;
                }
            },
            features: [{
                ftype: 'summary'
            }]
        });

        me.callParent(arguments);
    }
});