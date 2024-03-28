Ext.define('B4.view.regop.personal_account.action.BanRecalcWin', {
    extend: 'B4.view.regop.personal_account.action.BaseAccountWindow',

    alias: 'widget.banrecalcwin',

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.MonthPicker',
        'B4.view.regop.personal_account.action.BanRecalcGrid'
    ],

    modal: true,
    closable: false,
    width: 800,
    height: 550,
    minHeight: 300,
    title: 'Запрет перерасчета',
    closeAction: 'destroy',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    minDate: null,
    accountOperationCode: 'BanRecalcOperation',

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
                                labelAlign: 'right',
                                labelWidth: 200,
                                width: 360
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    name: 'Reason',
                                    maxLength: 200,
                                    fieldLabel: 'Причина',
                                    flex: 3
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Документ-основание',
                                    allowBlank: false,
                                    labelWidth: 180
                                }
                                
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                width: 360
                            },
                            items: [
                                {
                                    xtype: 'b4monthpicker',
                                    fieldLabel: 'Период начала действия',
                                    name: 'DateStart',
                                    allowBlank: false,
                                    editable: false,
                                    flex: 3,
                                    minValue: me.minDate
                                },
                                {
                                    xtype: 'b4monthpicker',
                                    fieldLabel: 'Период окончания действия',
                                    name: 'DateEnd',
                                    editable: false,
                                    allowBlank: false,
                                    labelWidth: 180
                                }
                                
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 10 0',
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 200,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'IsCharge',
                                    fieldLabel: 'Запрет перерасчета начислений'
                                },
                                {
                                    xtype: 'checkbox',
                                    name: 'IsPenalty',
                                    fieldLabel: 'Запрет перерасчета пени',
                                    labelWidth: 203
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'banrecalcgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});