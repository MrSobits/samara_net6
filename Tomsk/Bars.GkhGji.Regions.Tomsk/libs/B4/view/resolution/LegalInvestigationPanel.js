Ext.define('B4.view.resolution.LegalInvestigationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.legalinvestigationpanel',
    
    requires: [
        'B4.form.ComboBox',
        'B4.enums.TypeResolutionPetitions',
        'B4.ux.form.field.TabularTextArea'
    ],
    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            autoScroll: true,
            title: 'Рассмотрение дела',
            bodyPadding: 5,
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Ходатайства',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            name: 'HasPetition',
                            displayField: 'Display',
                            store: B4.enums.TypeResolutionPetitions.getStore(),
                            valueField: 'Value',
                            width: 250
                        },
                        {
                            xtype: 'label',
                            text: 'Текст ходатайства:',
                            padding: '0 0 5 0',
                            width: 150
                        },
                        {
                            xtype: 'tabtextarea',
                            name: 'PetitionText',
                            maxLength: 2000,
                            disabled: true,
                            flex: 1,
                            //значение в поле "Текст ходатайства" постановления, и в форме редактирования "Текст ходатайства"
                            //могут быть разными.
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Присутствовали',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'FioAttend',
                            labelWidth: 140,
                            labelAlign: 'right',
                            flex: 1,
                            fieldLabel: 'ФИО Присутствовавших',
                            displayField: 'Display',
                            store: B4.enums.TypeResolutionPetitions.getStore(),
                            valueField: 'Value'
                        },
                        {
                            xtype: 'label',
                            padding: '0 0 5 0',
                            text: 'Пояснения:',
                            width: 150
                        },
                        {
                            xtype: 'tabtextarea',
                            name: 'ExplanationText',
                            maxLength: 2000,
                            flex: 1,
                            //значение в поле "Пояснения" постановления, и в форме редактирования "Пояснения"
                            //могут быть разными.
                            readOnly: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});