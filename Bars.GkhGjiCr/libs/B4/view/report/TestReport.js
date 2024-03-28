Ext.define('B4.view.report.TestReport', {
    extend: 'Ext.form.Panel',
    itemId: 'testReportPanel',

    layout: {
        type: 'vbox'
    },
    
    border: false,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Municipality'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 200,
                labelAlign: 'right',
                width: 600
            },
            items: [
               {
                    xtype: 'b4selectfield',
                    name: 'Municipality',
                    textProperty: 'Name',
                    fieldLabel: 'Муниципальное образование',
                    store: 'B4.store.dict.Municipality',
                   

                    editable: false,
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name', flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});