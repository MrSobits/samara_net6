Ext.define('B4.view.prescription.CancelEditGeneralInfoTab', {
    extend: 'Ext.container.Container',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    requires: [
        'B4.enums.TypeProlongation',
        'B4.store.dict.DecisionMakingAuthorityGji',
        'B4.store.dict.Inspector'
    ],
    alias: 'widget.canceleditgeneralinfotab',
    title: 'Основная информация',
    itemId: 'canceleditgeneralinfotab',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'container',
                padding: '5 0 5 0',
                layout: { xtype: 'hbox' },
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.DecisionMakingAuthorityGji',
                    textProperty: 'Name',
                    name: 'DecisionMakingAuthority',
                    fieldLabel: 'Орган, вынесший решение',
                    editable: false,
                    columns: [
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'B4.store.dict.DecisionMakingAuthorityGji',
                            dock: 'bottom'
                        }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateDecisionCourt',
                    fieldLabel: 'Дата вступления в силу решения суда',
                    format: 'd.m.Y',
                    labelWidth: 130
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'IssuedCancel',
                    fieldLabel: 'ДЛ, вынесшее решение',
                    editable: false,
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: 'B4.store.dict.Inspector',
                            dock: 'bottom'
                        }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateCancel',
                    fieldLabel: 'Дата отмены',
                    format: 'd.m.Y',
                    labelWidth: 130
                },
                {
                    xtype: 'textarea',
                    name: 'Reason',
                    fieldLabel: 'Причина',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    name: 'PetitionNumber',
                    fieldLabel: 'Номер ходатайства',
                    maxLength: 300
                },
                {
                    xtype: 'datefield',
                    name: 'PetitionDate',
                    fieldLabel: 'Дата ходатайства',
                    format: 'd.m.Y',
                    labelWidth: 130
                },
                {
                    xtype: 'textarea',
                    name: 'DescriptionSet',
                    fieldLabel: 'Установлено',
                    maxLength: 2000,
                    flex: 1
                },
                {
                    xtype: 'b4combobox',
                    name: 'Prolongation',
                    fieldLabel: 'Продлено',
                    displayField: 'Display',
                    valueField: 'Value',
                    editable: false,
                    items: B4.enums.TypeProlongation.getItems()
                },
                {
                    xtype: 'datefield',
                    name: 'DateProlongation',
                    fieldLabel: 'Продлить до',
                    format: 'd.m.Y',
                    labelWidth: 130
                }
            ]
        });

        me.callParent(arguments);
    }
});