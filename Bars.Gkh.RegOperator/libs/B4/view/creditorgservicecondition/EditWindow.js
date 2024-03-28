Ext.define('B4.view.creditorgservicecondition.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.Panel',
        'B4.store.CreditOrg'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.creditOrgServiceCondEditWindow',
    title: 'Добавить/редактировать запись',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 250,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'CreditOrg',
                    textProperty: 'Name',
                    fieldLabel: 'Наименование',
                    store: 'B4.store.CreditOrg',
                    padding: '5px 0 5px 0',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'numberfield',
                    name: 'CashServiceSize',
                    fieldLabel: 'Размер расчётно-кассового обслуживания',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'CashServiceDateFrom',
                    fieldLabel: 'Дата с',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'CashServiceDateTo',
                    fieldLabel: 'Дата по'
                },
                {
                    xtype: 'numberfield',
                    name: 'OpeningAccPay',
                    fieldLabel: 'Плата за открытие счета',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'OpeningAccDateFrom',
                    fieldLabel: 'Дата с',
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    format: 'd.m.Y',
                    name: 'OpeningAccDateTo',
                    fieldLabel: 'Дата по'
                }
            ],
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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