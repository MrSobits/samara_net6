Ext.define('B4.view.realityobj.MapWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 700,
    minHeight: 600,
    bodyPadding: 5,
    itemId: 'realityobjMapWindow',
    title: 'Карта',
    closeAction: 'hide',
    maximizable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'component',
                    renderTpl: new Ext.XTemplate(
                        '<div id="yaMapDiv" style="width: 100%; height: 100%;"></div>'
                    )
                }
            ]
        });

        me.callParent(arguments);
    }
});