Ext.define('B4.view.report.CompareWithBaseProgrammPanel', {
    extend: 'Ext.form.Panel',
    title: '',
    itemId: 'compareWithBaseProgrammPanel',
    layout: {
        type: 'vbox'
    },
    border: false,

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.view.dict.programcr.Grid'
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
                    name: 'ProgramCr',
                    itemId: 'sfProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа 1',
                    store: 'B4.store.dict.ProgramCr',
                   

                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Name',
                            flex: 1,
                            text: 'Наименование',
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCrAdd',
                    itemId: 'sfProgramCrAdd',
                    textProperty: 'Name',
                    fieldLabel: 'Программа 2',
                    store: 'B4.store.dict.ProgramCr',
                   

                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'Name',
                            flex: 1,
                            text: 'Наименование',
                            filter: { xtype: 'textfield' }
                        }
                    ],
                    editable: false,
                    allowBlank: false
                },
                {
                    xype: 'datefield',
                    xtype: 'datefield',
                    name: 'ReportDate',
                    itemId: 'dfReportDate',
                    fieldLabel: 'Дата отчета',
                    format: 'd.m.Y',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    itemId: 'tfMunicipality',
                    fieldLabel: 'Муниципальные образования',
                    emptyText: 'Все МО'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'FinancialSources',
                    itemId: 'tfFinancial',
                    fieldLabel: 'Разрезы финансирования',
                    emptyText: 'Все разрезы'
                }
            ]
        });

        me.callParent(arguments);
    }
});