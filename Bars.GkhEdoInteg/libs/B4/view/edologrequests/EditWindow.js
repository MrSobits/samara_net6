Ext.define('B4.view.edologrequests.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 550,
    height: 600,
    bodyPadding: 0,
    itemId: 'edoLogRequestsEditWindow',
    title: 'Загруженные обращения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'edologReqsAppCitsGrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});