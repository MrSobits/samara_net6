/**
* Контроллер раздела протокола
*/
Ext.define('B4.controller.specialobjectcr.Protocol', {
    extend: 'B4.controller.MenuItemController',
    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.FieldRequirementAspect',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    models: [
        'specialobjectcr.Protocol'
    ],
    stores: [
        'specialobjectcr.Protocol'
    ],
    
    views: [
        'specialobjectcr.ProtocolGrid',
        'specialobjectcr.ProtocolEditWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    params: {},

    mainView: 'specialobjectcr.ProtocolGrid',
    mainViewSelector: 'specialobjectcrprotocolgrid',

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.specialobjectcr.Navi',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'protocolObjectCrPerm',
            permissions: [
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Create',
                    applyTo: 'b4addbutton',
                    selector: 'specialobjectcrprotocolgrid'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'specialobjectcrprotocolgrid',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.SumActVerificationOfCosts',
                    applyTo: 'gkhdecimalfield[name=SumActVerificationOfCosts]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.TypeWork.View',
                    applyTo: 'tab[text="Виды работ"]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_View',
                    applyTo: 'gkhdecimalfield[name=SumActVerificationOfCosts]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.SumActVerificationOfCosts_Edit',
                    applyTo: 'gkhdecimalfield[name=SumActVerificationOfCosts]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVote_View',
                    applyTo: 'gkhdecimalfield[name=CountVote]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVote_Edit',
                    applyTo: 'gkhdecimalfield[name=CountVote]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVoteGeneral_View',
                    applyTo: 'gkhdecimalfield[name=CountVoteGeneral]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountVoteGeneral_Edit',
                    applyTo: 'gkhdecimalfield[name=CountVoteGeneral]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeClient_View',
                    applyTo: 'gkhdecimalfield[name=GradeClient]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeClient_Edit',
                    applyTo: 'gkhdecimalfield[name=GradeClient]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeOccupant_View',
                    applyTo: '#gkhdecimalfield[name=GradeOccupant]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.GradeOccupant_Edit',
                    applyTo: '#gkhdecimalfield[name=GradeOccupant]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountAccept_View',
                    applyTo: 'gkhdecimalfield[name=CountAccept]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.DecisionOms_View',
                    applyTo: 'checkbox[name=DecisionOms]',
                    selector: 'specialobjectcrprotocolwin',
                    applyBy: function(component, allowed) {
                        if (allowed)
                            component.show();
                        else
                            component.hide();
                    }
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Register.Protocol.Field.CountAccept_Edit',
                    applyTo: 'gkhdecimalfield[name=CountAccept]',
                    selector: 'specialobjectcrprotocolwin'
                }
            ]
        },
        {
            xtype: 'requirementaspect',
            applyOn: { event: 'show', selector: 'specialobjectcrprotocolwin' },
            requirements: [
                {
                    name: 'GkhCr.SpecialObjectCr.Protocol.Field.OwnerName',
                    applyTo: 'textfield[name=OwnerName]',
                    selector: 'specialobjectcrprotocolwin'
                },
                {
                    name: 'GkhCr.SpecialObjectCr.Protocol.Field.File',
                    applyTo: '[name=File]',
                    selector: 'specialobjectcrprotocolwin'
                }
            ]
        },
        {
            /**
            * Аспект взаимодействия таблицы и формы редактирования протоколов
            */
            xtype: 'grideditctxwindowaspect',
            name: 'protocolGridWindowAspect',
            gridSelector: 'specialobjectcrprotocolgrid',
            editFormSelector: 'specialobjectcrprotocolwin',
            modelName: 'specialobjectcr.Protocol',
            editWindowView: 'specialobjectcr.ProtocolEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions[this.editFormSelector + ' [name=TypeDocumentCr]'] = {
                    'change': { fn: this.changeTypeDocumentCr, scope: this }
                };

                actions[this.editFormSelector + ' [name=Contragent]'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.showAll = true;
                        }
                    }
                };
                
                actions[this.editFormSelector + ' protocoltypeworkspecialcrgrid'] = {
                    'beforeload': {
                        fn: function (store, operation) {
                            operation.params = operation.params || {};
                            operation.params.showAll = true;
                        }
                    }
                };

                actions[this.editFormSelector + ' [name=SumActVerificationOfCosts]'] = {
                    'hide': { fn: me.onShowHideCountParamsFieldset, scope: me },
                    'show': { fn: me.onShowHideCountParamsFieldset, scope: me }
                };

                actions[this.editFormSelector + ' [name=CountVoteGeneral]'] = {
                    'hide': { fn: me.onShowHideCountParamsFieldset, scope: me },
                    'show': { fn: me.onShowHideCountParamsFieldset, scope: me }
                };

                actions[this.editFormSelector + ' [name=CountAccept]'] = {
                    'hide': { fn: me.onShowHideCountParamsFieldset, scope: me },
                    'show': { fn: me.onShowHideCountParamsFieldset, scope: me }
                };

                actions[this.editFormSelector + ' [name=GradeClient]'] = {
                    'hide': { fn: me.onShowHideCountParamsFieldset, scope: me },
                    'show': { fn: me.onShowHideCountParamsFieldset, scope: me }
                };

                actions[this.editFormSelector + ' [name=GradeOccupant]'] = {
                    'hide': { fn: me.onShowHideCountParamsFieldset, scope: me },
                    'show': { fn: me.onShowHideCountParamsFieldset, scope: me }
                };
            },
            onSaveSuccess: function (aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }
            },
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.ObjectCr = asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId');
                    }

                    var form = asp.tryGetForm();
                    form.down('protocoltypeworkspecialcrgrid').getStore().load();
                },
                beforesetformdata: function (asp, record) {
                    var me = this,
                        form = asp.tryGetForm(),
                        cbTypeDocumentCr = form.down('[name=TypeDocumentCr]');

                    me.controller.setContextValue(form, 'protocolId', record.getId());

                    var storeTypeDocumentCr = cbTypeDocumentCr.getStore();
                    storeTypeDocumentCr.filters.clear();
                    storeTypeDocumentCr.filter([{ property: 'objectCrId', value: asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId') },
                        { property: 'protocolId', value: record.getId() }]
                    );

                    asp.controller.mask('Загрузка', form);
                    B4.Ajax.request(B4.Url.action('GetDates', 'SpecialProtocolCr', {
                        objectCrId: asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId')
                    })).next(function (response) {

                        var obj = Ext.JSON.decode(response.responseText),
                            form = asp.tryGetForm();
                        if (!form) {
                            me.unmask();
                            return;
                        }

                        me.controller.setContextValue(form, 'DateStart', obj.DateStart);
                        me.controller.setContextValue(form, 'DateEnd', obj.DateEnd);

                        form.down('[name=DateFrom]').setMaxValue(obj.DateEnd);
                        asp.controller.unmask();
                    }).error(function () {
                        asp.controller.unmask();
                    });
                },
                aftersetformdata: function (asp, record, panel) {
                    var form = asp.tryGetForm();
                    if (record.raw.TypeDocumentCr) {
                        var typeDocumentCr = record.raw.TypeDocumentCr.Key;

                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                    
                        B4.Ajax.request(B4.Url.action('GetDates', 'SpecialProtocolCr', {
                            objectCrId: asp.controller.getContextValue(asp.controller.getMainComponent(), 'objectcrId')
                        })).next(function (response) {
                        
                            var obj = Ext.JSON.decode(response.responseText),
                                form = asp.tryGetForm();
                            if (!form) {
                                asp.controller.unmask();
                                return;
                            }

                            form.down('[name=DateFrom]').setMinValue(typeDocumentCr == 'ProtocolNeedCr' ? '01.01.1970' : obj.DateStart);
                            form.down('[name=DateFrom]').setMaxValue(obj.DateEnd);
                            asp.controller.unmask();
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }

                    var typeWorkTab = panel.down('[name=TypeWorkTab]');

                    if (asp.controller.getContextValue(form, 'protocolId') > 0) {
                        typeWorkTab.setDisabled(false);
                    } else {
                        typeWorkTab.setDisabled(true);
                    }

                    asp.controller.getAspect('protocolObjectCrPerm').setPermissionsByRecord(record);  
                }
            },
            
            tryGetForm: function() {
                return this.componentQuery(this.editFormSelector);
            },
            
            changeTypeDocumentCr: function (newValue) {
                var me = this,
                    form = this.getForm(),
                    nfCountVote = form.down('[name=CountVote]'),
                    nfCountVoteGeneral = form.down('[name=CountVoteGeneral]'),
                    nfCountAccept = form.down('[name=CountAccept]'),
                    nfGradeClient = form.down('[name=GradeClient]'),
                    nfGradeOccupant = form.down('[name=GradeOccupant]'),
                    nfSumActVerificationOfCosts = form.down('[name=SumActVerificationOfCosts]'),
                    ownerName = form.down('[name=OwnerName]'),
                    isDisabled = newValue.valueModels[0] ? newValue.valueModels[0].raw.Key == 'ActAuditDataExpense' : false,
                    minDate = newValue.valueModels[0] && newValue.valueModels[0].raw.Key == 'ProtocolNeedCr' ? '01.01.1970' : me.controller.getContextValue(me.controller.getMainComponent(), 'DateStart');

                nfCountVote.setDisabled(isDisabled);
                nfCountVoteGeneral.setDisabled(isDisabled);
                nfCountAccept.setDisabled(isDisabled);
                nfGradeClient.setDisabled(isDisabled);
                nfGradeOccupant.setDisabled(isDisabled);
                ownerName.setDisabled(newValue.valueModels[0] && newValue.valueModels[0].raw.Key != 'Act');
                
                form.down('[name=DateFrom]').setMinValue(minDate);

                var disabledSum = !isDisabled;
                if (disabledSum == false) {
                    me.setSumActVerificationOfCosts(nfSumActVerificationOfCosts);
                } else {
                    nfSumActVerificationOfCosts.setDisabled(disabledSum);
                    nfSumActVerificationOfCosts.allowBlank = disabledSum;
                }

                me.controller.getAspect('protocolObjectCrPerm').setPermissionsByRecord({ getId: function () { return me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId'); } });
            },
            setSumActVerificationOfCosts: function (nfSumActVerificationOfCosts) {
                var me = this;

                B4.Ajax.request(B4.Url.action('GetObjectSpecificPermissions', 'Permission', {
                    permissions: Ext.encode(['GkhCr.SpecialObjectCr.Register.Protocol.SumActVerificationOfCosts_Edit']), //?
                    ids: Ext.encode([me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId')])
                })).next(function (response) {
                    me.controller.unmask();
                    var perm = Ext.decode(response.responseText)[0];
                    nfSumActVerificationOfCosts.setDisabled(!perm[0]);
                    nfSumActVerificationOfCosts.allowBlank = !perm[0];
                }).error(function () {
                    me.controller.unmask();
                });
            },
            onShowHideCountParamsFieldset: function() {
                var me = this,
                    view = me.getForm(),
                    fieldSet = view.down('[fieldSetType=CountProperties]'),
                    anyShow = false;

                fieldSet.items.items.forEach(function (container) {
                    container.items.items.forEach(function (childItem) {
                        if (!childItem.hidden) {
                            anyShow = true;
                        }
                    });
                });

                fieldSet.setVisible(anyShow);
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'protocolTypeWorkCrBtnMultiSlctWndAspect',
            buttonSelector: 'specialobjectcrprotocolwin protocoltypeworkspecialcrgrid b4addbutton[action=addProtocolTypeWorkCr]',
            multiSelectWindowSelector: '#protocolTypeWorkSpecCr2SelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'specialobjectcr.TypeWorkCrForSelect',
            storeSelected: 'specialobjectcr.TypeWorkCrForSelected',
            textProperty: 'WorkName',
            gridSelector: 'protocoltypeworkspecialcrgrid',
            columnsGridSelect: [
                 { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'WorkName', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                 { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'WorkName', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Виды работ для отбора',
            titleGridSelected: 'Выбранные виды работ',
            onBeforeLoad: function (store, operation) {
                operation.params['objectCrId'] = this.controller.getContextValue(this.controller.getMainComponent(), 'objectcrId');
            },
            listeners: {
                getdata: function (asp, records) {
                    var me = this,
                        wnd = Ext.ComponentQuery.query('specialobjectcrprotocolwin')[0],
                        ids = [];

                    Ext.Array.each(records.items,
                        function (item) {
                            ids.push(item.get('Id'));
                        }, this);

                    if (ids.length > 0) {
                        asp.controller.mask('Добавление работ', me.controller.getMainView());
                        
                        B4.Ajax.request(B4.Url.action('AddTypeWorks', 'SpecialProtocolCrTypeWork', {
                            protocolId: me.controller.getContextValue(me.controller.getMainComponent(), 'protocolId'),
                            typeWorkCrIds: ids
                        })).next(function () {

                            asp.controller.unmask();
                            wnd.down('protocoltypeworkspecialcrgrid').getStore().load();
                            return true;
                        }).error(function (e) {
                            asp.controller.unmask();
                            Ext.Msg.alert('Ошибка!', e.message);
                        });
                    }
                    
                    return true;
                }
            }
        }
    ],
    init: function () {
        var me = this;
        
        me.control({
            'specialobjectcrprotocolwin protocoltypeworkspecialcrgrid': {
                render: function (grid) {
                    var store = grid.getStore();
                    
                    if (store) {
                        store.on('beforeload', me.onBeforeLoadProtocolTypeWorkCr, me);
                        store.load();
                    }
                }
            },
            
            'specialobjectcrprotocolwin protocoltypeworkspecialcrgrid b4deletecolumn': {
                click: function (grid, b, t, y, r, record) {
                    var me = this;

                    Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                        if (result == 'yes') {
                            record.destroy()
                                .next(function() {
                                    grid.getStore().load();
                                }, me)
                                .error(function(result) {
                                    Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                                    me.unmask();
                                }, me);
                        }
                    }, me);
                }
            }
        });

        this.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('specialobjectcrprotocolgrid'),
            store;

        me.params.id = id;
        me.bindContext(view);
        me.setContextValue(view, 'objectcrId', id);
        me.application.deployView(view, 'specialobjectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);

        this.getAspect('protocolObjectCrPerm').setPermissionsByRecord({ getId: function() { return id; } });

    },
    
    onBeforeLoadProtocolTypeWorkCr: function(store, operation) {
        operation.params.objectCrId = this.getContextValue(this.getMainComponent(), 'objectcrId');
    }
});