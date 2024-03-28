Ext.define('B4.view.payreg.RequestsEditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        //'B4.form.SelectField',
        //'B4.enums.PayerType',
        //'B4.store.dict.PhysicalPersonDocType',
        //'B4.enums.IdentifierType',
        //'B4.view.payreg.Grid',
        //'B4.enums.GisGmpPaymentsKind', 
        //'B4.enums.GisGmpPaymentsType', 
        //'B4.enums.GisGmpChargeType', 

    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 10,
    itemId: 'payregRequestsEditWindow',
    title: 'Запрос платежей',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                                
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'combobox',
                                //     margin: '10 0 5 0',
                                labelWidth: 90,
                                labelAlign: 'right',
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y H:i',
                                    allowBlank: false,
                                    name: 'GetPaymentsStartDate',
                                    fieldLabel: 'Дата оплат с',
                                    itemId: 'dfGetPaymentsStartDate',
                                    flex: 1,
                                }, 
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y H:i',
                                    allowBlank: false,
                                    name: 'GetPaymentsEndDate',
                                    fieldLabel: 'Дата оплат по',
                                    itemId: 'dfGetPaymentsEndDate',
                                    value: new Date(),
                                    flex: 1,
                                }
                            ]
                },
                {
                    xtype: 'button',
                    text: 'Отправить запрос',
                    tooltip: 'Запросить все платежи за указанные даты',
                    iconCls: 'icon-accept',
                    width: 70,
                    itemId: 'btnSendPaymentsRequest'
                }
            ]
        });

        me.callParent(arguments);
    }
});