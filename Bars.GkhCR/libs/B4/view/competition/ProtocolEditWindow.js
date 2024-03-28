Ext.define('B4.view.competition.ProtocolEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 320,
    maxHeight: 320,
    bodyPadding: 5,
    alias: 'widget.competitionprotocoleditwindow',
    title: 'Протокол',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.TypeCompetitionProtocol'
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
                    xtype: 'combobox',
                    editable: false,
                    name: 'TypeProtocol',
                    fieldLabel: 'Тип протокола',
                    displayField: 'Display',
                    store: B4.enums.TypeCompetitionProtocol.getStore(),
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    anchor: '100%',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'SignDate',
                            fieldLabel: 'Дата подписания протокола',
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'component'
                        }
                    ]
                },
                
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    allowBlank: false,
                    fieldLabel: 'Файл'
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    anchor: '100%',
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'ExecDate',
                            fieldLabel: 'Дата проведения процедуры',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'timefield',
                            fieldLabel: 'Время проведения процедуры',
                            format: 'H:i',
                            submitFormat: 'Y-m-d H:i:s',
                            minValue: '07:00',
                            maxValue: '23:00',
                            name: 'ExecTime'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Note',
                    fieldLabel: 'Примечание',
                    height: 50,
                    maxLength: 500
                },
                {
                    xtype: 'checkbox',
                    name: 'IsCancelled',
                    fieldLabel: 'Конкурс признан несостоявшимся'
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
                        }, '->', {
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