Ext.define('B4.view.report.UploadingAtDataForFundPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'uploadingAtDataForFundPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            }
        });

        me.callParent(arguments);
    }
});
