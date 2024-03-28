Ext.define('B4.controller.realityobj.GovDecisionProtocol', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.StateContextMenu',
        'B4.form.ComboBox',
        'B4.Ajax',
        'B4.Url',
        'B4.QuickMsg',
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.StateButton',
        'B4.aspects.StateGridWindowColumn'
    ],

    views: [
        'realityobj.govdecisionprotocol.MainGrid',
        'realityobj.govdecisionprotocol.Window'
    ],

    models: [
        'RealityObjectDecisionProtocol'
    ],

    stores: [
        'realityobj.GovDecision'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'govdecisionprotocolgrid'
        },
        {
            ref: 'win',
            selector: 'govdecisionprotocolwin'
        },
        {
            ref: 'minFundField',
            selector: 'govdecisionprotocolwin textfield[name=MinFund]'
        },
        {
            ref: 'fundFormatField',
            selector: 'govdecisionprotocolwin checkbox[name=FundFormationByRegop]'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.RealityObject.Register.GovProtocolDecisionViewCreate.Create', applyTo: 'b4addbutton', selector: 'govdecisionprotocolgrid' }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            permissions: [{ name: 'Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Delete' }],
            name: 'deleteGovProtocolDecisionStatePerm'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'govDecProtocolStatePermissionAspect',
            editFormAspectName: 'govProtocolDecisionAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                { name: 'Gkh.RealityObject.Register.GovProtocolDecisionEditDelete.Edit', applyTo: 'b4savebutton', selector: 'govdecisionprotocolwin' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'govProtocolDecisionAspect',
            gridSelector: 'govdecisionprotocolgrid',
            storeName: 'realityobj.GovDecision',
            modelName: 'GovDecision',
            editWindowView: 'realityobj.govdecisionprotocol.Window',
            editFormSelector: 'govdecisionprotocolwin',
            listeners: {
                getdata: function (me, record) {
                    record.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                },
                beforesetformdata: function (me, record) {
                    if (!record.get('Id')) {
                        record.set('ProtocolDate', new Date());
                    } else {
                        record.set('RealityObject', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                    }
                },
                aftersetformdata: function (asp, record) {
                    var me = asp.controller;
                    B4.Ajax.request(B4.Url.action('GetLatest', 'DecisionStraightForward', {
                        roId: me.getContextValue(me.getMainComponent(), 'realityObjectId')
                    })).next(function (response) {
                        try {
                            var json = Ext.JSON.decode(response.responseText);
                            if (json.data) {
                                if (json.data.CrFundFormationDecision) {
                                    me.getFundFormatField().setDisabled(true);
                                } else {
                                    me.getFundFormatField().setDisabled(false);
                                }

                                if (json.data.MinFundAmountDecision) {
                                    me.getMinFundField().setValue(json.data.MinFundAmountDecision.Decision);
                                } else {
                                    me.getMinFundField().setValue(40);
                                }
                            }
                        } catch (e) {

                        }

                    });
                    
                    this.controller.getAspect('govdecisionprotocolwinStateButtonAspect').setStateData(record.get('Id'), record.get('State')); //2
                }
            },
            deleteRecord: function (record) {
                if (record.getId()) {
                    this.controller.getAspect('deleteGovProtocolDecisionStatePerm').loadPermissions(record)
                        .next(function (response) {
                            var me = this,
                                grants = Ext.decode(response.responseText);

                            if (grants && grants[0]) {
                                grants = grants[0];
                            }

                            // проверяем пермишшен колонки удаления
                            if (grants[0] == 0) {
                                Ext.Msg.alert('Сообщение', 'Удаление на данном статусе запрещено');
                            } else {
                                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                                    if (result == 'yes') {
                                        var model = this.getModel(record);

                                        var rec = new model({ Id: record.getId() });
                                        me.mask('Удаление', me.controller.getMainComponent());
                                        rec.destroy()
                                            .next(function () {
                                                this.fireEvent('deletesuccess', this);
                                                me.updateGrid();
                                                me.unmask();
                                            }, this)
                                            .error(function (result) {
                                                Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                                me.unmask();
                                            }, this);
                                    }
                                }, me);
                            }

                        }, this);
                }
            }
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'govdecisionprotocolwinStateButtonAspect',
            stateButtonSelector: 'govdecisionprotocolwin #stateDecBtn',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    //Если статус изменен успешно, то проставляем новый статус
                    asp.setStateData(entityId, newState);
                    
                    //и перезагружаем грид, т.к. в гриде нужно обновить столбец Статус
                    var editWindowAspect = asp.controller.getAspect('govProtocolDecisionAspect');
                    editWindowAspect.updateGrid();
                    
                    if (entityId) {
                        var model = this.controller.getModel('B4.model.GovDecision');
                        model.load(entityId, {
                            success: function (rec) {
                                editWindowAspect.setFormData(rec);
                                // this.controller.getAspect('requestTransferRfPerm').setPermissionsByRecord(rec);
                            },
                            scope: this
                        });
                    }
                    
                    asp.controller.getAspect('baseDefaultEditPanelAspect').setData(entityId);
                }
            }
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'govdecisionprotocolgrid': {
                'render': function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeStoreLoad, me);
                    grid.getStore().load();
                }
            },
            'checkbox[chbcontrol]': {
                'change': me.onChbControlValueChanged
            }
        });
        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('govdecisionprotocolgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    },

    onBeforeStoreLoad: function (store, operation) {
        var me = this;
        operation.params = operation.params || {};
        operation.params.objectId = me.getContextValue(me.getMainComponent(), 'realityObjectId');
    },


    onChbControlValueChanged: function (chb, newval) {
        var parentWin = chb.up('govdecisionprotocolwin'),
            fields = Ext.ComponentQuery.query('[chbgroup=' + chb.chbcontrol + ']', parentWin);

        Ext.Array.each(fields, function (f) {
            f.setDisabled(!newval);
        });

    }
});