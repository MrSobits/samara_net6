Ext.define('B4.ux.grid.EntityHistoryFieldWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.entityhistoryfieldwindow',

    requires: [
        'B4.ux.grid.EntityHistoryFieldGrid'
    ],

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',

    width: 640,
    minWidth: 640,
    minHeight: 480,
    maxHeight: 480,
    bodyPadding: 2,
    title: 'Детализация изменений',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'entityhistoryfieldgrid',
                    flex: 1
                }
            ]
        });
        me.callParent(arguments);
    }
});