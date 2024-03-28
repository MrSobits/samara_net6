Ext.define('B4.view.paysize.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.paysizeeditpanel',

    requires: ['B4.view.paysize.RecordGrid'],

    closable: true,
    title: 'Размер взноса на кап.ремонт',
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    trackResetOnLoad: true,

    canEdit: false,

    initComponent: function () {
        var me = this;
        //На основании размера взноса были произведены начисления, размер не доступен для редактирования
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'hidden',
                    name: 'Id'
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Период действия',
                            layout: 'hbox',
                            maxWidth: 400,
                            padding: '0 5 5 5',
                            defaults: {
                                xtype: 'datefield',
                                maxWidth: 200,
                                format: 'd.m.Y',
                                labelWidth: 50,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'DateStart',
                                    fieldLabel: 'С',
                                    allowBlank: false
                                },
                                {
                                    name: 'DateEnd',
                                    fieldLabel: 'По'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            name: 'Warning',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 5px 0px 0px 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell;">На основании размера взноса были произведены начисления,<br> размер не доступен для редактирования.</span>',
                            maxWidth: null,
                            hidden: true
                        }
                    ]
                },
                {
                    xtype: 'paysizerecordgrid',
                    flex: 1
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
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});