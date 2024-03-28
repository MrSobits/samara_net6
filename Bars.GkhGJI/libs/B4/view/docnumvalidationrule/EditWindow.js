Ext.define('B4.view.docnumvalidationrule.EditWindow', {
    extend: 'B4.form.Window',

    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'docnumvalidationruleeditwindow',
    title: 'Правило проставления номера документа',
    closeAction: 'hide',
    trackResetOnLoad: true,
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.TypeDocumentGji'
    ],
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    itemId: 'cbRule',
                    name: 'RuleId',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false,
                    fields: ['Id', 'Name'],
                    fieldLabel: 'Правило проставления',
                    url: '/DocNumValidationRule/ListRules',
                    queryMode: 'local',
                    triggerAction: 'all',
                    displayField: 'Name',
                    valueField: 'Id'
                },
                {
                    xtype: 'combobox',
                    name: 'TypeDocumentGji',
                    store: B4.enums.TypeDocumentGji.getStore(),
                    itemId: 'cbTypeDocumentGji',
                    labelAlign: 'right',
                    allowBlank: false,
                    editable: false,
                    fieldLabel: 'Тип документа',
                    displayField: 'Display',
                    valueField: 'Value'
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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
