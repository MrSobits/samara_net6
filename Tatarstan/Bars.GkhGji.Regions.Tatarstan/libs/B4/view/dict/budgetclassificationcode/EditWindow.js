Ext.define('B4.view.dict.budgetclassificationcode.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    alias: 'widget.budgetclassificationcodeeditwindow',
    layout: 'form',
    width: 550,
    bodyPadding: 5,
    title: 'КБК',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.ArticleLawGji',
],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            xtype: 'container',
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                labelWidth: 140,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Kbk',
                    fieldLabel: 'КБК',
                    allowBlank: false
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Municipalities',
                    fieldLabel: 'Муниципальные образования',
                    flex: 1,
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ArticleLawGji',
                    name: 'ArticleLaw',
                    fieldLabel: 'Статья закона',
                    editable: false,
                    textProperty: 'Name',
                    selectionMode: 'MULTI',
                    allowBlank: false,
                    columns: [
                        {
                            text: 'Статьи для отбора',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                    ],
                    windowCfg: {
                        title: 'Выбор статей закона'
                    }
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                    },
                    padding: '0 0 5 0',
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'StartDate',
                            fieldLabel: 'Период действия с',
                            format: 'd.m.Y',
                            labelWidth: 140,
                            allowBlank: false,
                        },
                        {
                            xtype: 'datefield',
                            name: 'EndDate',
                            fieldLabel: 'по',
                            format: 'd.m.Y',
                            labelWidth: 60,
                            flex: 0.5,
                            allowBlank: false,
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'tbfill'
                                },
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