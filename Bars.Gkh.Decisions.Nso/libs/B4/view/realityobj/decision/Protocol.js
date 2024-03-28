Ext.define('B4.view.realityobj.decision.Protocol', {
    extend: 'Ext.form.Panel',

    alias: 'widget.protocoldecision',

    entity: 'Protocol',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    state: {},

    border: false,

    items: [
        {
            xtype: 'container',
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            flex: 1,
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    border: 0,
                    bodyStyle: Gkh.bodyStyle,
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'hidden',
                            name: 'Id'
                        },
                        {
                            xtype: 'hidden',
                            name: 'RealityObject'
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер',
                            name: 'DocumentNum',
                            allowBlank: false,
                            flex: 1,
                            labelWidth: 140
                        }   
                    ]
                },
                {
                    xtype: 'container',
                    border: 0,
                    bodyStyle: Gkh.bodyStyle,
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    flex: 1,
                    defaults: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        labelAlign: 'right',
                        allowBlank: false,
                        flex: 1,
                        labelWidth: 142
                    },
                    items: [
                        {
                            fieldLabel: 'Дата протокола',
                            name: 'ProtocolDate'
                        },
                        {
                            fieldLabel: 'Дата вступления в силу',
                            name: 'DateStart',
                            labelWidth: 150
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'ManOrgName',
                    fieldLabel: 'Управление домом',
                    readOnly: true,
                    labelWidth: 142
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Уполномоченное лицо',
                    name: 'AuthorizedPerson',
                    labelWidth: 142
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Телефон уполномоченного лица',
                    name: 'PhoneAuthorizedPerson',
                    labelWidth: 200
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Протокол',
                    name: 'File',
                    labelWidth: 142
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер входящего письма',
                            name: 'LetterNumber'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Дата входящего письма',
                            name: 'LetterDate',
                            format: 'd.m.Y'
                        }
                    ]
                },
            ]
        }
    ],
    setValues: function(v) {
        var me = this,
            roId = v.RealityObject ? v.RealityObject.Id : me.up('window').down('[name=RealityObject]').getValue();
        me.getForm().setValues(v);
        me.state = v.State;
        me.down('field[name=RealityObject]').setValue(roId);
    },

    getValues: function() {
        var values = this.getForm().getValues();
        values.RealityObject = { Id: values.RealityObject };

        var file = this.down('b4filefield').getValue();
        values.File = file ? { Id: file.Id } : null;
        values.State = this.state;
        return values;
    }
});