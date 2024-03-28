Ext.define('B4.view.integrations.ExternalRis', {
    extend: 'Ext.ux.IFrame',
    alias: 'widget.ris-externalris-frame',

    requires: [],
    title: 'РИС',
    closable: true,

    tbar: [],
    src: B4.Url.action('ToExternalRis', 'ExternalRis'),

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});