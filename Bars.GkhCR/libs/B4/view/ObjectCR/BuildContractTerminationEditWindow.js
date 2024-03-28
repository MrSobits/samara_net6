Ext.define('B4.view.objectcr.BuildContractTerminationEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 520,
    height: 260,
    alias: 'widget.buildcontractterminationeditwindow',
    title: 'Расторжение договора подряда',
    autoScroll: true,
    itemId: 'buildContractTerminationEditWindow',

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.store.dict.Inspector',
        'B4.store.dict.TerminationReason',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.objectcr.BuildContractTerminationGrid',
        'B4.enums.TypeContractBuild',
        'B4.view.Control.GkhButtonPrint',
        'B4.enums.YesNo'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: { type: 'vbox', align: 'stretch' },
                    padding: '5 5 5 5',
                    defaults: {
                        labelWidth: 120,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'TerminationDate',
                            fieldLabel: 'Дата расторжения',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа'
                        },
                        {
                            xtype: 'b4filefield',
                            editable: false,
                            name: 'DocumentFile',
                            fieldLabel: 'Документ-основание',
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'DictReason',
                            fieldLabel: 'Причина расторжения',
                            store: 'B4.store.dict.TerminationReason',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                            ],
                            editable: false
                        },
                        {
                            xtype: 'textarea',
                            name: 'Reason',
                            fieldLabel: 'Основание расторжения',
                            maxLength: 250
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