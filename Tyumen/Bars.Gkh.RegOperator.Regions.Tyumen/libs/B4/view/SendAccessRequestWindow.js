Ext.define('B4.view.SendAccessRequestWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.sendaccessrequestwin',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    maxHeight: 175,
    minWidth: 400,
    minHeight: 175,
    bodyPadding: 5,
    bodyPadding: 5,
    title: 'Отправить запрос на доступ к редактированию',
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Close',
        'B4.form.FileField',
    ],
    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            defaults:
            {
                labelAlign: 'right',
                labelWidth: 70,
                anchor: '100%'
            },
            items: [
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    itemId: 'tbFile',
                    fieldLabel: 'Основание',
                    possibleFileExtensions: '',
                },
                {
                    xtype: 'label',
                    text: 'Примечание',
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    itemId: 'tbDescription',
                    labelWidth: 0,
                    maxLength: 300
                },            
            ],
            dockedItems:
            [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items:
                    [
                        {
                            xtype: 'button',
                            text: 'Отправить',
                            textAlign: 'left',
                            action: 'SendRequest',
                            iconCls: 'icon-requestemail-button',
                            icon: B4.Url.content('content/img/icons/email_start.png'),
                            tooltip: {
                                width: 200,
                                text: 'Отправить запрос на редактирование'
                             }
                           },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items:
                            [
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