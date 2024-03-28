Ext.define('B4.view.regop.personal_account.action.CancelChargeWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.cancelchargewin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod',
        'B4.view.regop.personal_account.action.CancelChargeGrid'
    ],

    modal: true,
    closable: false,
    width: 800,
    height: 550,
    minHeight: 300,
    title: 'Отмена начислений',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: 0,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.ChargePeriod',
                                    textProperty: 'Name',
                                    editable: false,
                                    allowBlank: false,
                                    trigger2Cls: '',
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
                                    labelWidth: 60,
                                    fieldLabel: 'Период',
                                    flex: 2
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Reason',
                                    fieldLabel: 'Причина',
                                    allowBlank: true,
                                    flex: 3
                                },
                                {
                                    xtype: 'b4filefield',
                                    labelWidth: 160,
                                    name: 'Document',
                                    allowBlank: false,
                                    fieldLabel: 'Документ-основание',
                                    flex: 4
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'cancelchargegrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});