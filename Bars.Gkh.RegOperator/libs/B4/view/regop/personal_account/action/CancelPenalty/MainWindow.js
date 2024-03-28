Ext.define('B4.view.regop.personal_account.action.CancelPenalty.MainWindow', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.cancelpenaltywin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.regop.ChargePeriod',
        'B4.view.regop.personal_account.action.CancelPenalty.AccountsGrid'
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

    accountOperationCode: null,

    initComponent: function() {
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
                                    xtype: 'b4combobox',
                                    storeAutoLoad: true,
                                    editable: false,
                                    name: 'ChargePeriod',
                                    labelWidth: 60,
                                    fieldLabel: 'Период',
                                    flex: 2,
                                    store: Ext.create('B4.store.regop.ChargePeriod', {
                                        autoLoad: true,
                                        sorters: [{
                                            property: 'Id',
                                            direction: 'DESC'
                                        }],
                                        sortOnLoad: true,
                                        remoteSort: false
                                    }),
                                    pageSize: 1
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
                                    fieldLabel: 'Документ-основание',
                                    flex: 4
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'cancelpenaltygrid',
                    flex: 1,
                    accountOperationCode: me.accountOperationCode
                }
            ]
        });

        me.callParent(arguments);
    }
});