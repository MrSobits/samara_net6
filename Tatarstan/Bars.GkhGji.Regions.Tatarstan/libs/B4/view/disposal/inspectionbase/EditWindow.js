Ext.define('B4.view.disposal.inspectionbase.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.disposalinspectionbaseeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.form.SelectField',
        'B4.store.disposal.DecisionInspBaseType',
        'B4.store.dict.ControlTypeRiskIndicators'
    ],
    
    width: 800,
    bodyPadding: 5,
    title: 'Основание проведения',
    layout: { type: 'vbox', align: 'stretch' },
    closeAction: 'hide',

    initComponent: function () {
        var me = this,
            inspBaseTypeStore = Ext.create('B4.store.disposal.DecisionInspBaseType'),
            riskIndicatorStore = Ext.create('B4.store.dict.ControlTypeRiskIndicators');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    store: inspBaseTypeStore,
                    name: 'InspectionBaseType',
                    fieldLabel: 'Основание проведения',
                    textProperty: 'Name',
                    editable: false,
                    allowBlank: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }]
                },
                {
                    xtype: 'textfield',
                    name: 'OtherInspBaseType',
                    fieldLabel: 'Иное основание проведения',
                    maxLength: 1000,
                },
                {
                    xtype: 'datefield',
                    name: 'FoundationDate',
                    fieldLabel: 'Дата основания',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4selectfield',
                    store: riskIndicatorStore,
                    name: 'RiskIndicator',
                    fieldLabel: 'Индикатор риска',
                    textProperty: 'Name',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
