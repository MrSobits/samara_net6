Ext.define('B4.view.regop.personal_account.ownerinformation.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.paccountownerinformationeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 450,
    bodyPadding: 5,
    title: 'Документ о собственности',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.Control.GkhDecimalField',
        'B4.store.regop.owner.PersonalAccountOwner',
        'B4.form.FileField'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Сведения о документе собственности',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox',
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата начала документа',
                            name: 'StartDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата окончания документа',
                            name: 'EndDate',
                            format: 'd.m.Y',
                            allowBlank: true
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл',
                            editable: false
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Сведения о собственнике',
                    anchor: '100%',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        xtype: 'container',
                        layout: 'hbox',
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'AreaShare',
                            fieldLabel: 'Доля собственности',
                            allowBlank: false,
                            maxValue: 1
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Owner',
                            fieldLabel: 'ФИО собственника',
                            store: 'B4.store.regop.owner.PersonalAccountOwner',
                            editable: false,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'ФИО абонента',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: {
                                        xtype: 'textfield'
                                    }
                                }
                            ]
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});