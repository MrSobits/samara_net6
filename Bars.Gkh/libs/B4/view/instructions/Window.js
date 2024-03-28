Ext.define('B4.view.instructions.Window', {
    extend: 'Ext.window.Window',
    requires: [
        'B4.view.instructions.DataView',
        'B4.ux.grid.toolbar.Paging',
        'B4.mixins.window.ModalMask'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    width: 640,
    height: 480,
    constrain: true,
    autoDestroy: true,
    closeAction: 'hide',
    closable: true,
    overflowY: 'auto',
    overflowX: 'hidden',
    itemId: 'instructionsWindow',
    layout: {
        type: 'fit',
        align: 'stretch'
    },
    title: 'Документы',
    items: [
        {
            xtype: 'instructionsview'
        }
    ]
});