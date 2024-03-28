Ext.define('B4.aspects.realityobj.RealityObjectLiftEditPanelAspect', {
    extend: 'B4.aspects.GridEditCtxWindow',
    requires: [
        'B4.form.RealObjStructuralElementSelectiField',

        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    alias: 'widget.realityobjlifteditpanelaspect',
        
    gridSelector: 'realityobjectliftgrid',
    editFormSelector: 'realityobjectliftwindow',
    modelName: 'realityobj.Lift',
    editWindowView: 'realityobj.LiftWindow',

    constructor: function (config) {
        Ext.apply(this, config);
        this.callParent(arguments);

        this.addEvents(
            'beforewindowcreate'
        );
    },

    editRecord: function (record) {
        var me = this,
            id = record ? record.getId() : null,
            model = this.getModel(record);

        id ? model.load(id, {
            success: function (rec) {
                me.setFormData(rec);
            },
            scope: me
        }) : me.setFormData(new model({ Id: 0 }));
    },

    setFormData: function (rec) {
        rec.phantom
            ? this.showBeforeMainWindow(rec)
            : this.continueSetFormData(rec, this.getForm());
    },

    continueSetFormData: function(rec, form) {
        if (this.fireEvent('beforesetformdata', this, rec, form) !== false) {
            form.loadRecord(rec);
            form.getForm().updateRecord();
            form.getForm().isValid();
        }

        this.fireEvent('aftersetformdata', this, rec, form);
    },

    showBeforeMainWindow: function (record) {
        var me = this,
            form = Ext.create('B4.form.Window', {
                closeAction: 'destroy',
                width: 450,
                bodyPadding: '10px 7px 5px 0',
                title: 'Выбор лифта',
                items: [
                    {
                        xtype: 'realobjstructuralelementselectifield',
                        name: 'RealityObjectStructuralElement',
                        fieldLabel: 'ООИ Лифт',
                        isGetOnlyIdProperty: false,
                        editable: false,
                        allowBlank: false,
                        anchor: '100%',
                        labelAlign: 'right',
                        windowCfg: {
                            width: 900
                        },
                        listeners: {
                            beforeload: {
                                fn: function (sf, operation, store) {
                                    operation.params.objectId = me.controller.getContextValue(me.controller.getMainView(), 'realityObjectId');
                                    operation.params.showLift = true;
                                },
                                scope: me
                            }
                        }
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
                                        listeners: {
                                            click: function () {
                                                form.getForm().updateRecord();
                                                form.close();
                                                me.continueSetFormData.call(me, form.getRecord(), me.getForm());
                                            }
                                        }
                                    }
                                ]
                            },
                            { xtype: 'tbfill' },
                            {
                                xtype: 'buttongroup',
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        listeners: {
                                            click: function () {
                                                form.close();
                                            }
                                        }
                                    }
                                ]
                            }
                        ]
                    }
                ]
            });

        form.loadRecord(record);
        form.getForm().updateRecord();
        form.show();
    },

    listeners: {
        getdata: function (asp, record) {
            var me = this;
            if (!record.data.id) {
                record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            }
        }
    },

    otherActions: function (actions) {
        actions['realityobjectliftwindow realobjstructuralelementselectifield'] = {
            'beforeload': {
                fn: function (sf, operation, store) {
                    operation.params.objectId = this.controller.getContextValue(this.controller.getMainView(), 'realityObjectId');
                    operation.params.showLift = true;
                    operation.params.liftId = this.getForm().getRecord().getId();
                },
                scope: this
            }
        };
    }
});