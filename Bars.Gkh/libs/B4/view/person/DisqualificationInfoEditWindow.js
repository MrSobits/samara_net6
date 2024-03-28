Ext.define('B4.view.person.DisqualificationInfoEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.persondisqualinfoeditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 250,
    maxHeight: 250,
    bodyPadding: 5,
    title: 'Сведения о дисквалификации',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.TypePersonDisqualification'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'combobox',
                            editable: false,
                            fieldLabel: 'Основание',
                            store: B4.enums.TypePersonDisqualification.getStore(),
                            displayField: 'Display',
                            allowBlank: false,
                            flex: 0.7,
                            valueField: 'Value',
                            name: 'TypeDisqualification'
                        },
                        {
                            xtype: 'datefield',
                            name: 'DisqDate',
                            labelWidth: 70,
                            fieldLabel: 'Дата',
                            allowBlank: false,
                            flex: 0.3,
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'EndDisqDate',
                            fieldLabel: 'Дата окончания',
                            flex: 0.3,
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'component',
                            flex: 0.7
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Ходатайство о дисквалификации',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 100,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PetitionNumber',
                                    fieldLabel: 'Номер',
                                    flex: 0.3,
                                    maxLength: 100
                                },
                                {
                                    xtype: 'component',
                                    flex: 0.4
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'PetitionDate',
                                    fieldLabel: 'Дата',
                                    labelWidth: 70,
                                    flex: 0.3,
                                    format: 'd.m.Y'
                                }
                            ]
                       },
                       {
                           xtype: 'b4filefield',
                           name: 'PetitionFile',
                           allowBlank: true,
                           fieldLabel: 'Файл'
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
                            items: [
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});