Ext.define('B4.view.dict.normativedoc.EditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.normativeDocEditWindow',
    
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',
    width: 600,
    height: 500,
    bodyPadding: 5,
    itemId: 'normativeDocEditWindow',
    title: 'Нормативно-правовой документ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.NormativeDocCategory',
        'B4.view.dict.normativedoc.NormativeDocItemGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'vbox',
                    align: 'stretch',
                    items: [
                        {
                            xtype: 'container',
                            width: '100%',
                            layout: 'anchor',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FullName',
                                    fieldLabel: 'Полное наименование',
                                    anchor: '100%',
                                    //allowBlank: false, это свойство проставляется в момент выставления пермишенов в контроллере посмотрите 
                                    maxLength: 1000
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    anchor: '100%',
                                    allowBlank: false,
                                    maxLength: 300
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'Code',
                                    anchor: '100%',
                                    fieldLabel: 'Код',
                                    allowBlank: false,
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowDecimals: false,
                                    nanText: '{value} не является числом'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    anchor: '100%',
                                    fieldLabel: 'Категория',
                                    enumName: 'B4.enums.NormativeDocCategory',
                                    name: 'Category'
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Период действия',
                                    name: 'Validity',
                                    hidden: true,
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                                pack: 'end'
                                            },
                                            defaults: {
                                                labelWidth: 15,
                                                labelAlign: 'right',
                                                width: 180
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateFrom',
                                                    padding: '0 90 0 0',
                                                    fieldLabel: 'с:',
                                                    format: 'd.m.Y'
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'DateTo',
                                                    fieldLabel: 'по:',
                                                    format: 'd.m.Y'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'normativeDocItemGrid',
                            flex: 1,
                            width: '100%'
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});