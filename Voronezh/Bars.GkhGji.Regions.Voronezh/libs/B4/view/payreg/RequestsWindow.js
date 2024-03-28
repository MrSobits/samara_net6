Ext.define('B4.view.payreg.RequestsWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.PayerType',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.enums.IdentifierType',
        'B4.view.payreg.RequestsGrid',
        'B4.enums.GisGmpPaymentsKind', 
        'B4.enums.GisGmpPaymentsType', 
        'B4.enums.GisGmpChargeType', 
        'B4.enums.TypeDocumentGji'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 1500,
    height: 700,
    bodyPadding: 10,
    itemId: 'payregRequestsWindow',
    title: 'Запросы платежей',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'payregrequestsgrid',
                    //flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    itemId: 'btnClose'
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