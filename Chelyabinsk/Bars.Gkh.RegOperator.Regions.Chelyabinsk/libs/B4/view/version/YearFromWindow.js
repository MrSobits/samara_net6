Ext.define('B4.view.version.YearFromWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.versionorderyearwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    bodyPadding: 5,
    minWidth: 400,
    title: 'Расчет очередности начиная с года',
    closable: false,
    requires: [
        'B4.form.SelectField',
        
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    bodyPadding: 5,
                    title: 'Год начала расчета',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 30
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            border: false,
                            layout: 'hbox',
                            defaults: {
                                format: 'd.m.Y',
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'YearFrom',
                                    fieldLabel: 'Год с',
                                    readOnly: false
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'YearTo',
                                    fieldLabel: 'Год по',
                                    readOnly: false
                                }
                            ]
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
                                    xtype: 'button',
                                    text: 'Рассчитать',
                                    tooltip: 'Рассчитать',
                                    action: 'copy',
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