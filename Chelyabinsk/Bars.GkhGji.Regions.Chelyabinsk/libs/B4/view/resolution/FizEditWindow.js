Ext.define('B4.view.resolution.FizEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    minWidth: 520,
    minHeight: 210,
    height: 300,
    bodyPadding: 5,
    itemId: 'resolutionFizEditWindow',
    title: 'Форма редактирования физ лица',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.PhysicalPersonDocType',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        margin: '5 0 5 0',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                          {
                              xtype: 'b4selectfield',
                              name: 'PhysicalPersonDocType',
                              fieldLabel: 'Вид документа ФЛ',
                              store: 'B4.store.dict.PhysicalPersonDocType',
                              editable: false,
                              flex: 1,
                              itemId: 'dfPhysicalPersonDocType',
                              allowBlank: true,
                              columns: [
                                   { text: 'Код', dataIndex: 'Code', flex: 0.3, filter: { xtype: 'textfield' } },
                                  { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }

                              ]
                          },
                    ]
                },                
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        margin: '5 0 5 0',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                          {
                              xtype: 'textfield',
                              name: 'DocumentSerial',
                              itemId: 'dfDocumentSerial',
                              fieldLabel: 'Серия документа ФЛ',
                              allowBlank: true,
                              disabled: false,
                              flex: 1,
                              editable: true,
                              maxLength: 20
                          },
                       {
                           xtype: 'textfield',
                           name: 'DocumentNumber',
                           itemId: 'dfDocumentNumber',
                           fieldLabel: 'Номер документа ФЛ',
                           allowBlank: true,
                           disabled: false,
                           flex: 1,
                           editable: true,
                           maxLength: 20
                       }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'textfield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            itemId: 'dfIsRF',
                            name: 'IsRF',
                            fieldLabel: 'Гражданин РФ',
                            allowBlank: true,
                            disabled: false,
                            flex: 1,
                            editable: true
                        }
                    ]
                },
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