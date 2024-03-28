Ext.define('B4.view.dict.qualifytest.QualifyTestQuestionsEditWindow', {
    extend: 'B4.form.Window',
    itemId: 'qualifyTestQuestionsEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.dict.qualifytest.QualifyTestQuestionsAnswersGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    title: 'Форма редактирования вопроса',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                        {
                            xtype: 'container',
                            title: 'Работы по содержанию и ремонту МКД',
                            flex: 1,
                            layout:
                            {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Code',
                                    fieldLabel: 'Номер вопроса'
                                },
                                {
                                    xtype: 'combobox',
                                    name: 'IsActual',
                                    fieldLabel: 'Актуальный',
                                    displayField: 'Display',
                                    store: B4.enums.YesNoNotSet.getStore(),
                                    valueField: 'Value',
                                    allowBlank: false,
                                    editable: false
                                },
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'Question',
                            fieldLabel: 'Текст вопроса'
                        },
                        {
                          xtype: 'tabpanel',
                          border: false,
                          flex: 1,
                          defaults: {
                              border: false
                          },
                          items: [
                             {
                                  xtype: 'qtestdictanswersgrid',
                                 flex: 1
                             }
                          ]
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
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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