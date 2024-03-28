Ext.define('B4.view.objectcr.ProgramSelectWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ProgramCr',
        'B4.ux.button.Save',
        'B4.ux.button.Close'
    ],

    alias: 'widget.objectcrprogramselectwindow',
    title: 'Форма заполнения',
    modal: true,
    width: 400,
    height: 180,
    layout: 'form',
    bodyPadding: 5,
    closable: true,

    windowText: null,

    initComponent: function () {
        var me = this;
        
        Ext.apply(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
                flex: 1
            },
            items: [
                {
                    xtype: 'container',
                    style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 10px 5px 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">'+ me.windowText +'</span>'
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    textProperty: 'Name',
                    fieldLabel: 'Программа КР',
                    flex: 1,
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        { text: 'Программа КР', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
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
                                    text: 'Заполнить'
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
                                    text: 'Закрыть',
                                    listeners: {
                                        'click': function() {
                                            me.close();
                                        }
                                    }
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