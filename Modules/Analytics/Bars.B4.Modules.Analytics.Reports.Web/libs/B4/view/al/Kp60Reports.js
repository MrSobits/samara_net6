Ext.define('B4.view.al.Kp60Reports', {
    extend: 'Ext.ux.IFrame',
    alias: 'widget.kp60reportsframe',

    requires: [],
    title: 'Отчеты КП60',
    closable: true,

    tbar: [],
    src: B4.Url.action('ToKp60Report', 'Kp60Report'),

    initComponent: function () {
        var me = this;
        me.callParent(arguments);
    }
});