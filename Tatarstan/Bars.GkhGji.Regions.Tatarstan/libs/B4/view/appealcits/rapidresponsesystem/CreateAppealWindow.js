Ext.define('B4.view.appealcits.rapidresponsesystem.CreateAppealWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 600,
    bodyPadding: 5,
    closable: false,
    closeAction: 'destroy',
    alias: 'widget.rapidresponsesystemappealcreatewindow',
    itemId: 'rapidresponsesystemappealcreatewindow',
    title: 'Формирование обращения в СОПР',
    modal: true,
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.appealcits.RealityObject'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'AppealCitsRealityObject',
                    textProperty: 'Address',
                    store: 'B4.store.appealcits.RealityObject',
                    fieldLabel: 'Адрес дома',
                    columns: [
                        { header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    labelAlign: 'right',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    itemId: 'containerInfo',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Для выбора адреса проверяемого дома необходимо, чтобы был заполнен адрес во вкладке "Место возникновения проблемы" </span>'
                },
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