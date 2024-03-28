Ext.define('B4.view.publicservorg.ContractTempGraph', {
    extend: 'Ext.form.Panel',
    alias: 'widget.publicservorgcontracttepgraphpanel',
    requires: [
        'B4.view.publicservorg.ContractTempGraphGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],

    title: 'Информация о температурном графике',

    bodyPadding: 3,
    closeAction: 'hide',

    closable: false,
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
                    xtype: 'publicservorgcontracttempgraphgrid'
                }
            ]
        });

        me.callParent(arguments);
    }
});
