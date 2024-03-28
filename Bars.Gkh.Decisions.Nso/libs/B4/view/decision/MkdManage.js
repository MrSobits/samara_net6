Ext.define('B4.view.decision.MkdManage', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.FileField',
        'B4.form.ComboBox'
    ],

    border: false,
    
    protocolId: 0,
    decisionTypeCode: '',

    initComponent: function () {
        Ext.apply(this, {
            defaults: {
                hideTrigger: (this.saveable === false),
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата начала управления'
                },
                {
                    xtype: 'datefield',
                    name: 'EndDate',
                    fieldLabel: 'Дата окончания управления'
                },
                {
                    xtype: 'b4combobox',
                    allowBlank: false,
                    fieldLabel: 'Принятое решение',
                    name: 'DecisionCode',
                    url: '/decision/getdecisions/?protocolId=' + this.protocolId + '&decisionTypeCode=' + this.decisionTypeCode,
                    emptyItem: { Name: '-' },
                    displayField: 'name',
                    valueField: 'code',
                    fields: ['name', 'code']
                 },
                {
                    xtype: 'textfield',
                    readOnly: true,
                    name: 'ManOrg',
                    fieldLabel: 'Наименование УО'
                },
                {
                    xtype: 'textfield',
                    name: 'AuthorizedName',
                    fieldLabel: 'ФИО уполномоченного'
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Доверенность'
                }
            ]
        });
        this.callParent(arguments);
    }
});