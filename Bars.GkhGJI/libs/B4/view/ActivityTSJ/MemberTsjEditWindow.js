Ext.define('B4.view.activitytsj.MemberTsjEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.activityTsjMemberEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],
        
    width: 500,    
    height: 140,
    minWidth: 500,
    minHeight: 140,
    maxWidth: 700,
    maxHeight: 200,
    bodyPadding: 5,
    itemId: 'activityTsjMemberEditWindow',
    title: 'Реестр членов ТСЖ/ЖСК',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyPadding: 5,
            margins: -1,
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    editable: false,
                    name: 'Year',
                    itemId: 'cbxYear',
                    flex: 1,
                    fieldLabel: 'Год',
                    items: (function () {
                        var result = [];
                        for (var i = 2014; i <= 2025; i++) {
                            result.push([i, i]);
                        }
                        return result;
                    })(),
                    value: 2014,
                    allowBlank: false
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    itemId: 'ffStatuteFile',
                    fieldLabel: 'Файл реестра',
                    allowBlank: false
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
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton',
                                    text: ' Закрыть'
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