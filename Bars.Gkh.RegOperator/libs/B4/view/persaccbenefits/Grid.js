Ext.define('B4.view.persaccbenefits.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',     
        'B4.enums.ContragentState',
        'B4.store.regop.personal_account.PersonalAccountBenefits',
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod',
        'B4.view.Control.GkhButtonImport',
        'B4.grid.feature.Summary'
    ],

    title: 'Информация по начисленным льготам',
    alias: 'widget.persaccbenefitsgrid',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.PersonalAccountBenefits');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    text: 'Период',
                    dataIndex: 'Period',
                    xtype: 'gridcolumn',
                    flex: 1,
                    filter: { xtype: 'textfield' },
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersAccNum',
                    flex: 1,
                    text: 'Лицевой счет',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Owner',
                    flex: 1,
                    text: 'Абонент',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BenefitsName',
                    flex: 1,
                    text: 'Наименование льготной категории',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BenefitsDateStart',
                    text: 'Дата начала действия',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'BenefitsDateEnd',
                    text: 'Дата окончания действия',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    width: 100,
                    text: 'Сумма начисленной льготы',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (val) {
                        val = val || 0;
                        return Ext.util.Format.currency(val);
                    }
                },
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            features: [
                {
                    ftype: 'b4_summary'
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonimport'
                                },
                                {
                                     xtype: 'b4selectfield',
                                     store: 'B4.store.regop.ChargePeriod',
                                     selectionMode: 'MULTI',
                                     textProperty: 'Name',
                                     editable: false,
                                     columns: [
                                         {
                                             text: 'Наименование',
                                             dataIndex: 'Name',
                                             flex: 1,
                                             filter: { xtype: 'textfield' }
                                         },
                                         {
                                             text: 'Дата открытия',
                                             xtype: 'datecolumn',
                                             format: 'd.m.Y',
                                             dataIndex: 'StartDate',
                                             flex: 1,
                                             filter: { xtype: 'datefield' }
                                         },
                                         {
                                             text: 'Дата закрытия',
                                             xtype: 'datecolumn',
                                             format: 'd.m.Y',
                                             dataIndex: 'EndDate',
                                             flex: 1,
                                             filter: { xtype: 'datefield' }
                                         },
                                         {
                                             text: 'Состояние',
                                             dataIndex: 'IsClosed',
                                             flex: 1,
                                             renderer: function (value) {
                                                 return value ? 'Закрыт' : 'Открыт';
                                             }
                                         }
                                     ],
                                     name: 'ChargePeriod',
                                     labelAlign: 'right',
                                     fieldLabel: 'Период начисления',
                                     labelWidth: 175,
                                     width: 550,
                                     onSelectAll: function () {
                                         var me = this,
                                             oldValue = me.value;

                                         me.updateDisplayedText('Выбраны все');
                                         me.value = 'All';
                                         me.selectWindow.hide();

                                         me.fireEvent('change', me, me.value, oldValue);
                                     }
                                 }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});