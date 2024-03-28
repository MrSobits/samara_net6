Ext.define('B4.view.publicservorg.contractqualitylevel.Panel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.contractservicecontractqualitylevelpanel',

    requires: [
        'B4.view.publicservorg.contractqualitylevel.Grid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    bodyPadding: 3,
    closeAction: 'hide',

    closable: false,
    title: 'Показатели качества',
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    trackResetOnLoad: true,
    header: true,
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'contractservicequalitylevelgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});