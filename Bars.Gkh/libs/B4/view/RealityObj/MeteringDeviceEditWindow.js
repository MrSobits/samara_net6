Ext.define('B4.view.realityobj.MeteringDeviceEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.realityobjmetdeviceeditwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'anchor',
    width: 500,
    minWidth: 400,
    height: 430,
    minHeight: 420,
    maxHeight: 420,
    bodyPadding: 5,
    title: 'Прибор учета',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.MeteringDevice',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 300
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'MeteringDevice',
                    fieldLabel: 'Тип прибора',
                    store: 'B4.store.dict.MeteringDevice',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'datefield',
                    name: 'DateRegistration',
                    fieldLabel: 'Дата постановки на учет',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание'
                },
                {
                    xtype: 'textarea',
                    name: 'SerialNumber',
                    fieldLabel: 'Заводской номер прибора учёта',
                    flex: 1,
                    allowBlank: false
                },
                {
                    xtype: 'b4combobox',
                    floating: false,
                    name: 'AddingReadingsManually',
                    fieldLabel: 'Внесение показаний в ручном режиме',
                    displayField: 'Display',
                    items: B4.enums.YesNoNotSet.getItems(),
                    valueField: 'Value',
                    editable: false
                },
                 {
                     xtype: 'b4combobox',
                     floating: false,
                     name: 'NecessityOfVerificationWhileExpluatation',
                     fieldLabel: 'Обязательности проверки в рамках эксплуатации прибора учета',
                     displayField: 'Display',
                     items: B4.enums.YesNoNotSet.getItems(),
                     valueField: 'Value',
                     editable: false
                 },
                {
                    xtype: 'textarea',
                    name: 'PersonalAccountNum',
                    fieldLabel: 'Номер лицевого счёта',
                    flex: 1,
                    maxLength: 25
                },
                {
                    xtype: 'datefield',
                    name: 'DateFirstVerification',
                    fieldLabel: 'Дата первичной проверки',
                    format: 'd.m.Y'
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});