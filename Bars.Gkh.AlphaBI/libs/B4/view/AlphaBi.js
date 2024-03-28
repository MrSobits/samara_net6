Ext.define('B4.view.AlphaBi', {
    extend: 'Ext.ux.IFrame',
    alias: 'widget.rms-alphabi-frame',

    requires: [],
    title: 'OLAP аналитика',
    closable: true,

    tbar: [],
    src: B4.Url.action('ToAlphaBI', 'AlphaBi'),

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});