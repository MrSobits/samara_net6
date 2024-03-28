Ext.define('B4.view.version.ActualizeProgramCrWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.versionactualizeprogramwindow',
    title: 'Выбор программы кап. ремонта для актуализации',
    modal: true,
    width: 400,
    height: 200,
    layout: 'form',
    bodyPadding: 5,
    closable: true,
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.ux.button.Save'
    ],
    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 0px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Выберите программу кап. ремонта, на основании которой необходимо актуализировать долгосрочную программу кап. ремонта</span>'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа кап. ремонта',
                    flex: 1,
                    store: Ext.create('B4.store.dict.ProgramCr', { storeId: 'actualizeProgramCrWindowProgramCrStore' }),
                    columns: [
                        { text: 'Программа КР', dataIndex: 'Name', flex: 1 }
                    ],
                    editable: false,
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
                            items: [
                                {
                                    xtype: 'b4savebutton',
                                    text: 'Продолжить'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Отмена'
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