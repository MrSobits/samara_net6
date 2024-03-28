Ext.define('B4.view.report.Form1StateHousingInspectionPanel', {
    extend: 'Ext.form.Panel',
    itemId: 'form1StateHousingInspectionPanel',
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
                     fieldLabel: 'Программа',
                     store: 'B4.store.dict.ProgramCr',


                     editable: false,
                     allowBlank: false,
                     columns: [
                        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                     ]
                 },
                 {
                     xtype: 'datefield',
                     name: 'ReportDate',
                     itemId: 'dfReportDate',
                     fieldLabel: 'Дата Отчета',
                     format: 'd.m.Y',
                     value: new Date(),
                     allowBlank: false
                 },
                 {
                     xtype: 'datefield',
                     name: 'DateStart',
                     itemId: 'dfDateStart',
                     fieldLabel: 'Дата начала',
                     format: 'd.m.Y',
                     allowBlank: false
                 },
                 {
                     xtype: 'datefield',
                     name: 'DateEnd',
                     itemId: 'dfDateEnd',
                     fieldLabel: 'Дата окончания',
                     format: 'd.m.Y',
                     allowBlank: false
                 },
                 {
                     xtype: 'gkhtriggerfield',
                     name: 'Municipalities',
                     itemId: 'tfMunicipality',
                     fieldLabel: 'Муниципальные образования',
                     emptyText: 'Все МО'
                 }
            ]
        });

        me.callParent(arguments);
    }
});