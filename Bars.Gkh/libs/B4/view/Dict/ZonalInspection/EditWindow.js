﻿Ext.define('B4.view.dict.zonalinspection.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.Panel'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 950,
    height: 460,
    itemId: 'zonalInspectionEditWindow',
    title: 'Зональная жилищная инспекция',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    layout: 'fit',
                    items: [
                        {
                            xtype: 'panel',
                            title: 'Основная информация',
                            bodyStyle: Gkh.bodyStyle,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'panel',
                                    layout: 'form',
                                    split: false,
                                    border: false,
                                    bodyStyle: Gkh.bodyStyle,
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '5px 10px',
                                            layout: 'column',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    columnWidth: 0.5,
                                                    layout: 'anchor',
                                                    items: [
                                                        {
                                                            xtype: 'fieldset',
                                                            title: '1 гос.язык',
                                                            defaults: {
                                                                anchor: '100%',
                                                                labelWidth: 160,
                                                                labelAlign: 'right'
                                                            },
                                                            items: [
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Name',
                                                                    fieldLabel: 'Наименование',
                                                                    allowBlank: false,
                                                                    maxLength: 300
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'ZoneName',
                                                                    fieldLabel: 'Зональное наименование',
                                                                    maxLength: 300
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'BlankName',
                                                                    fieldLabel: 'Наименование для бланка',
                                                                    maxLength: 300
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'ShortName',
                                                                    fieldLabel: 'Краткое наименование'
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Address',
                                                                    fieldLabel: 'Адрес',
                                                                    maxLength: 300
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 0 5',
                                                    columnWidth: 0.5,
                                                    layout: 'anchor',
                                                    items: [
                                                        {
                                                            xtype: 'fieldset',
                                                            title: '2 гос.язык',
                                                            defaults: {
                                                                anchor: '100%',
                                                                labelWidth: 160,
                                                                labelAlign: 'right',
                                                                maxLength: 300
                                                            },
                                                            items: [
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'NameSecond',
                                                                    fieldLabel: 'Наименование'
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'ZoneNameSecond',
                                                                    fieldLabel: 'Зональное наименование'
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'BlankNameSecond',
                                                                    fieldLabel: 'Наименование для бланка',
                                                                    maxLength: 300
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'ShortNameSecond',
                                                                    fieldLabel: 'Краткое наименование'
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'AddressSecond',
                                                                    fieldLabel: 'Адрес'
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '0 15px 15px 0',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'column',
                                                    items: [
                                                        {
                                                            xtype: 'container',
                                                            margin: '0 0 0 10px',
                                                            columnWidth: 0.5,
                                                            layout: 'anchor',
                                                            defaults: {
                                                                anchor: '100%',
                                                                labelWidth: 170,
                                                                labelAlign: 'right',
                                                                width: '443px !important'
                                                            },
                                                            items: [
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Email',
                                                                    fieldLabel: 'E-mail',
                                                                    maxLength: 50
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Phone',
                                                                    fieldLabel: 'Телефон',
                                                                    maxLength: 50
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'Okato',
                                                                    fieldLabel: 'ОКАТО',
                                                                    maxLength: 30
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'IndexOfGji',
                                                                    fieldLabel: 'Индекс',
                                                                    hidden: true
                                                                },
                                                                {
                                                                    xtype: 'textfield',
                                                                    name: 'DepartmentCode',
                                                                    fieldLabel: 'Код',
                                                                    maxLength: 30
                                                                }
                                                            ]
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: 'column',
                                                    items: [
                                                        {
                                                            xtype: 'container',
                                                            margin: '0 0 0 10px',
                                                            columnWidth: 0.5,
                                                            layout: 'anchor',
                                                            defaults: {
                                                                anchor: '100%',
                                                                labelWidth: 170,
                                                                labelAlign: 'right',
                                                                width: '443px !important'
                                                            },
                                                            items: [
                                                                {
                                                                    xtype: 'checkbox',
                                                                    itemId: 'cbIsNotGZHI',
                                                                    name: 'IsNotGZHI',
                                                                    fieldLabel: 'Не ГЖИ',
                                                                    allowBlank: false,
                                                                    disabled: false,  
                                                                    editable: true
                                                                },
                                                            ]
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'zonalinspectionmunicipalitygrid',
                            margins: -1
                        },
                        {
                            xtype: 'zonalinspectioninspectorsgrid',
                            margins: -1
                        },
                        {
                            xtype: 'panel',
                            //padding: '10 0 0 0',
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Падежи наименование',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'column',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '10 0 0 0',
                                            columnWidth: 0.5,
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    title: 'Падежи Наименование',
                                                    defaults: {
                                                        anchor: '100%',
                                                        labelWidth: 100,
                                                        labelAlign: 'right',
                                                        maxLength: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NameGenetive',
                                                            fieldLabel: 'Родительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NameDative',
                                                            fieldLabel: 'Дательный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NameAccusative',
                                                            fieldLabel: 'Винительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NameAblative',
                                                            fieldLabel: 'Творительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'NamePrepositional',
                                                            fieldLabel: 'Предложный'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            padding: '10 0 0 5',
                                            columnWidth: 0.5,
                                            layout: 'anchor',
                                            items: [
                                                {
                                                    xtype: 'fieldset',
                                                    title: 'Падежи Краткое наименование',
                                                    defaults: {
                                                        anchor: '100%',
                                                        labelWidth: 100,
                                                        labelAlign: 'right',
                                                        maxLength: 300
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ShortNameGenetive',
                                                            fieldLabel: 'Родительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ShortNameDative',
                                                            fieldLabel: 'Дательный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ShortNameAccusative',
                                                            fieldLabel: 'Винительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ShortNameAblative',
                                                            fieldLabel: 'Творительный'
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ShortNamePrepositional',
                                                            fieldLabel: 'Предложный'
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
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