Ext.define('B4.view.administration.risdataexport.DpkrEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.risdataexportdpkreditwindow',

    requires: [
        'B4.mixins.window.ModalMask',
        'B4.view.administration.risdataexport.DpkrPanel'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    minWidth: 720,
    minHeight: 430,
    width: 850,
    height: 430,
    resizable: true,

    title: 'Долгосрочная программа',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'risdataexportdpkrpanel'
                }
            ],
        });

        me.callParent(arguments);
    }
});