Ext.define('B4.view.al.ExternalLinkWindow', {
    extend: 'Ext.window.Window',

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.al.ReportPrintFormat'
    ],

    title: 'Формирование внешней ссылки отчета',
    modal: true,
    width: 400,
    height: 130,
    closeAction: 'destroy',
    bodyPadding:5,

    layout: {
        type: 'form'
    },

    items: [
        {
            xtype: 'b4enumcombo',
            enumName: 'B4.enums.al.ReportPrintFormat',
            fieldLabel: 'Формат печати',
            value: B4.enums.al.ReportPrintFormat.xls
        },
        {
            xtype: 'textarea',
            name: 'ExternalLink',
            fieldLabel: 'Внешняя ссылка'
        }
    ],

    initComponent: function() {
        var me = this;
        me.callParent(arguments);
        
        me.down('textarea').setValue(me.getLink());
        me.down('b4enumcombo').on('change', function() { me.down('textarea').setValue(me.getLink()); });
    },

    getLink: function () {
        var me = this;
        return Ext.String.format("http://{0}{1}ExternalReport/Generate?reportId={2}&format={3}&token={4}",
            window.location.host, rootUrl, me.reportId, me.down('b4enumcombo').getValue(), me.token);
    }
});