Ext.define('B4.view.appealcits.AppCitAdmonVoilationEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 520,
    minHeight: 310,
    height: 510,
    bodyPadding: 5,
    itemId: 'appCitAdmonVoilationEditWindow',
    title: 'Форма редактирования нарушения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.dict.ViolationGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ViolationGji',
                    textProperty: 'Name',
                    name: 'ViolationGji',
                    fieldLabel: 'Нарушение',
                    editable: false,
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'ПиН', xtype: 'gridcolumn', dataIndex: 'CodePin', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 170,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                         {
                             xtype: 'datefield',
                             name: 'FactDate',
                             fieldLabel: 'Фактическая дата устранения нарушения',
                             format: 'd.m.Y',
                             labelWidth: 150 
                         },
                        {
                            xtype: 'datefield',
                            name: 'PlanedDate',
                            fieldLabel: 'Плановая дата устранения нарушения',
                            format: 'd.m.Y',
                            labelWidth: 150 
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