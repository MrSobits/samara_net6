Ext.define('B4.view.report.BuildContractsReestrReportPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'buildContractsReestrReportPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.form.SelectField'
    ],

    initComponent: function() {
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
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап.ремонта',
                    store: 'B4.store.dict.ProgramCr',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            header: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Municipalities',
                    itemId: 'sfMunicipality',
                    textProperty: 'Name',
                    fieldLabel: 'Муниципальные образования',
                    store: 'B4.store.dict.Municipality',
                    selectionMode: 'MULTI',
                    editable: false,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            header: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'checkboxfield',
                    name: 'contractsFilter',
                    itemId: 'cbContractsFilter',
                    fieldLabel: 'Только договора со статусом "Утверждено ГЖИ"',
                    flex: 1
                }
            ]
        });
        me.callParent(arguments);
    }
});