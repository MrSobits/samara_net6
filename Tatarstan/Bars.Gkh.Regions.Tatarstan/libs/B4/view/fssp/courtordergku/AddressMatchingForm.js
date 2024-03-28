Ext.define('B4.view.fssp.courtordergku.AddressMatchingForm', {
    extend: 'B4.form.Window',
    alias: 'widget.addressmatchingform',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.view.fssp.courtordergku.PgmuAddressGrid'
    ],

    title: 'Сопоставление адреса',
    modal: true,
    width: 800,
    height: 500,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    closeAction: 'hide',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            items: [
                {
                    xtype: 'pgmuaddressgrid',
                    flex: 1,
                    border: 0
                }
            ]
        });

        me.callParent(arguments);
    }
});
