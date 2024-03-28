Ext.define('B4.view.administration.risdataexport.DpkrWorksEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.risdataexportdpkrworksEditWindow',

    requires: [
        'B4.mixins.window.ModalMask',
        'B4.view.administration.risdataexport.DpkrWorksPanel'
    ],

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'fit',
    minWidth: 720,
    minHeight: 430,
    width: 850,
    height: 430,
    resizable: true,

    title: 'Работы долгосрочной программы капитального ремонта',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'risdataexportdpkrworkspanel'
                }
            ],
        });

        me.callParent(arguments);
    }
});