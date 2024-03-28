Ext.define('B4.view.fssp.courtordergku.AddressMatchingPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.fsspaddressmatchingpanel',

    title: 'Реестр сопоставления адресов',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.fssp.courtordergku.ImportedAddressMatchingRegistry',
        'B4.view.fssp.courtordergku.PgmuAddressGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    title: 'Адрес домов импорта',
                    xtype: 'importedaddressmatchingregistry',
                    flex: 1
                },
                {
                    title: 'Адрес домов ПГМУ',
                    xtype: 'pgmuaddressgrid',
                    selModel: null,
                    hiddenToolbar: true,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});