Ext.define('B4.view.controldate.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 650,
    height: 600,
    minWidth: 500,
    minHeight: 500,
    bodyPadding: 5,
    itemId: 'controlDateEditWindow',
    title: 'Контрольные сроки',
    closeAction: 'hide',
    maximized: true,
    requires: [
        'B4.form.SelectField',
        'B4.view.controldate.WorkGrid',
        'B4.view.dict.programcr.Grid',
        'B4.store.dict.ProgramCr',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {

            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    itemId: 'sfProgramCr',
                    fieldLabel: 'Программа кап.ремонта',
                    labelWidth: 200,
                    labelAlign: 'right',
                    readOnly: true
                },
                {
                    xtype: 'controldateworkgrid',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
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