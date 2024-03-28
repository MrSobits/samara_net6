Ext.define('B4.view.dict.mandatoryreqs.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.mandatoryreqseditwindow',

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

    mixins: ['B4.mixins.window.ModalMask'],

    layout: 'anchor',   
    width: 600,
    height: 790,
    bodyPadding: 5,
    title: 'Обязательные требования',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    autoScroll: true,
                    border: false,
                    defaults: {
                        xtype: 'container',
                        padding: 5,
                        border: false
                    },
                    items: [
                        {
                            xtype: 'container',
                            defaults: {
                                anchor: '100%',
                                labelWidth: 165,
                                labelAlign: 'left',
                                border: false,
                                flex: 1,
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'MandratoryReqName',
                                    fieldLabel: 'Наименование требования',
                                    allowBlank: false,
                                    maxLength: 300,
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'TorId',
                                    fieldLabel: 'Уникальный идентификатор',
                                    regex: new RegExp('^[0-9A-Fa-f]{8}-([0-9A-Fa-f]{4}-){3}[0-9A-Fa-f]{12}$'),
                                    regexText: 'Введен некорректный GUID'
                                },
                                {
                                    xtype: 'textareafield',
                                    name: 'MandratoryReqContent',
                                    fieldLabel: 'Содержание требования',
                                    allowBlank: false,
                                    scrollable: true,
                                    maxLength: 500,
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 180,
                                        labelAlign: 'right',
                                        border: true,
                                    },
                                    title: 'Вопросы проверочного листа',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                            },
                                        },
                                        {
                                            xtype: 'mandatoryreqsquestionnpdgrid',
                                            scrollable: true,
                                            height: 200
                                        }
                                    ]
                                },
                            ]                            
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                            },
                            items: [
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 180,
                                        labelAlign: 'right',
                                        border: true,
                                    },
                                    title: 'Нормативно-правовой документ',
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'hbox',
                                            },
                                        },
                                        {
                                            xtype: 'mandatoryreqsnormativedocgrid',
                                            scrollable: true,
                                            height: 200
                                        },
                                    ]
                                },
                            ]
                        },
                        {                           
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        anchor: '100%',
                                        labelWidth: 160,
                                        labelAlign: 'left',
                                        border: false,
                                        width: 260,
                                    },
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'StartDateMandatory',
                                            fieldLabel: 'Дата начала действия *',
                                            allowBlank: false,
                                            flex: 1,
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'EndDateMandatory',
                                            fieldLabel: 'Дата окончания действия',
                                            flex: 1,
                                        },
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                    },
                                    items:[]
                                }
                            ] 
                        },
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
                            columns: 1,
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