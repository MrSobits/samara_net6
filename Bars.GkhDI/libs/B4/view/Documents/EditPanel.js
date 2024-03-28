Ext.define('B4.view.documents.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'anchor',
    width: 800,
    minWidth: 800,
    bodyPadding: 5,
    itemId: 'documentsEditPanel',
    title: 'Документы',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
    'B4.ux.button.Save',
    'B4.form.FileField'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    idProperty: 'helpButton',
                                    text: 'Справка',
                                    tooltip: 'Справка',
                                    iconCls: 'icon-help'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'copyDocsButton',
                                    text: 'Добавить копированием',
                                    tooltip: 'Добавить копированием',
                                    iconCls: 'icon-folder-page'
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
              {
                  xtype: 'fieldset',
                  itemId: 'fsProjectContract',
                  defaults: {
                      anchor: '100%',
                      labelAlign: 'right',
                      labelWidth: 200
                  },
                  title: 'Проект договора управления с собственником помещений',
                  items: [
                    {
                        xtype: 'container',
                        anchor: '100%',
                        layout: 'column',
                        defaults: {
                            xtype: 'container',
                            layout: 'anchor',
                            columnWidth: 0.5,
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 200
                            }
                        },
                        items: [
                            {
                                items: [
                                {
                                    xtype: 'b4filefield',
                                    fieldLabel: 'Файл',
                                    name: 'FileProjectContract'
                                }

                                ]
                            },
                            {
                                items: [
                                {
                                    xtype: 'checkbox',
                                    name: 'NotAvailable',
                                    boxLabel: 'Не имеется',
                                    padding: '0 0 0 10'
                                }]
                            }
                        ]
                    },
                    {
                        xtype: 'textarea',
                        name: 'DescriptionProjectContract',
                        fieldLabel: 'Описание',
                        maxLength: 2000
                    }]
              },
              {
                  xtype: 'fieldset',
                  defaults: {
                      anchor: '100%',
                      labelAlign: 'right',
                      labelWidth: 200
                  },
                  title: 'Перечень и качество коммунальных услуг',
                  items: [
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileCommunalService'
                        },
                        {
                            xtype: 'textarea',
                            name: 'DescriptionCommunalCost',
                            fieldLabel: 'Описание (стоимость услуг)',
                            maxLength: 1500
                        },
                        {
                            xtype: 'textarea',
                            name: 'DescriptionCommunalTariff',
                            fieldLabel: 'Описание (тарифы)',
                            maxLength: 1500
                        }
                   ]
              },
              {
                  xtype: 'fieldset',
                  defaults: {
                      anchor: '100%',
                      labelAlign: 'right',
                      labelWidth: 200
                  },
                  title: 'Базовый перечень показателей качества содержания, эксплуатации и технического обслуживания жилых зданий',
                  items: [
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileServiceApartment'
                        }
                   ]
              }
            ]
        });
        me.callParent(arguments);
    }
});