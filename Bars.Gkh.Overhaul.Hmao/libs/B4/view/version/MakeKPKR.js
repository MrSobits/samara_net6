Ext.define('B4.view.version.MakeKPKR', {
    extend: 'B4.form.Window',
    alias: 'widget.versmakekpkrwin',
    mixins: ['B4.mixins.window.ModalMask'],
    bodyPadding: 5,
    minWidth: 400,
    title: 'Создать КПКР из ДПКР',
    closable: false,
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults:
            {
                labelWidth: 200,
                labelAlign: 'right',
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'vbox',
                    defaults: {
                        labelWidth: 250,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'StartYear',
                            fieldLabel: 'Год начала:',
                            allowDecimals: true,
                            allowBlank: false,
                            minValue: 2014,
                            maxValue: 3000
                        },
                        {
                            xtype: 'numberfield',
                            name: 'YearCount',
                            fieldLabel: 'Количество лет для формирования:',
                            allowDecimals: true,
                            allowBlank: false,
                            minValue: 1,
                            maxValue: 255
                        },
                        {
                            xtype: 'checkbox',
                            name: 'FirstYearPSD',
                            fieldLabel: 'Формирование ПСД за период в первом этапе',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                         {
                            xtype: 'checkbox',
                            name: 'EathWorkPSD',
                            fieldLabel: 'Формирование отдельного ПСД и СК для каждой работы',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                        {
                            xtype: 'checkbox',
                            name: 'FirstYearWithoutWork',
                            fieldLabel: 'В первый год КПКР нет работ',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                        {
                            xtype: 'checkbox',
                            name: 'SKWithWorks',
                            fieldLabel: 'Стройконтроль вместе с работами',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                        {
                            xtype: 'checkbox',
                            name: 'PSDWithWorks',
                            fieldLabel: 'ПСД вместе с работами',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                        {
                            xtype: 'checkbox',
                            name: 'OneProgramCR',
                            fieldLabel: 'Одна программа на весь период',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
                        },
                        {
                            xtype: 'checkbox',
                            name: 'PSDNext3',
                            fieldLabel: 'Создаь ПСД на следующий трехлетний плат в последнем этапе',
                            allowBlank: false,
                            editable: true,
                            labelWidth: 400,
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
                                    xtype: 'button',
                                    text: 'Создать КПКР',
                                    tooltip: 'Создать КПКР',
                                    action: 'makeKPKR',
                                    iconCls: 'icon-accept'
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