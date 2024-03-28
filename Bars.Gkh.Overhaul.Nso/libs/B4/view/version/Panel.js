Ext.define('B4.view.version.Panel', {
    extend: 'Ext.panel.Panel',

    closable: true,
    alias: 'widget.programversionpanel',
    title: 'Версия программы',
    requires: [
        'B4.ux.button.Save',
        'B4.view.version.RecordsGrid',
        'B4.view.version.ActualizeLogGrid',
        'B4.store.version.ProgramVersion',
        'B4.form.SelectField'
    ],

    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    bodyPadding: 5,

    items: [
        {
            xtype: 'b4selectfield',
            name: 'Version',
            columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }],
            store: 'B4.store.version.ProgramVersion',
            fieldLabel: 'Версия программы',
            editable: false,
            allowBlank: false,
            labelAlign: 'right',
            labelWidth: 170
        },
        {
            xtype: 'checkbox',
            boxLabel: 'Основная',
            name: 'IsMain',
            margin: '0 0 5 115',
            boxLabelAlign: 'before'
        },
        {
            xtype: 'tabpanel',
            flex: 1,
            layout: {
                align: 'stretch'
            },
            enableTabScroll: true,
            items: [
                {
                    xtype: 'versionrecordsgrid',
                    flex: 1
                },
                {
                    xtype: 'actualizeloggridgrid',
                    flex: 1
                }
            ]
        }
    ]
});