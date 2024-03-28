Ext.define('B4.controller.regop.owner.PersonalAccountOwner', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.Import',
        'B4.aspects.FieldRequirementAspect',
        'B4.view.regop.owner.PersonalAccountOwnerGrid',
        'B4.view.regop.owner.PersonalAccountOwnerEditWindow',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.StateContextMenu',
        'B4.aspects.regop.AccountGridEdit',
        'B4.view.regop.owner.RoomAreaShareAddWindow',
        'B4.view.regop.owner.RoomAreaShareGrid',
        'B4.view.regop.owner.ActivityStageEditWindow',
        'B4.view.contragent.ActivityStageGrid',
        'B4.aspects.GridEditWindow',
        'B4.enums.ActivityStageOwner',
        'B4.view.regop.owner.RegistrationAddressAddWindow'
    ],

    stores: [
        'regop.owner.PersonalAccountOwner',
        'regop.personal_account.BasePersonalAccount',
        'realityobj.Room',
        'regop.owner.RoomAreaShare',
        'contragent.ActivityStage'
    ],
    views: [
        'regop.owner.PersonalAccountOwnerGrid',
        'regop.owner.PersonalAccountOwnerEditWindow',
        'regop.owner.PersonalAccountGrid',
        'regop.owner.PersonalAccountAddWindow',
        'regop.owner.RoomAreaShareAddWindow',
        'regop.owner.RoomAreaShareGrid',
        'B4.view.regop.owner.ActivityStageEditWindow',
        'B4.view.contragent.ActivityStageGrid'
    ],
    models: [
        'regop.owner.PersonalAccountOwner',
        'regop.owner.IndividualAccountOwner',
        'regop.owner.LegalAccountOwner',
        'regop.personal_account.BasePersonalAccount',
        'contragent.ActivityStage'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    mainViewSelector: 'paownergrid',

    aspects: [
        {
            xtype: 'importaspect',
            importBtnSelector: 'paownergrid b4importbutton',
            controllerName: 'PersonalAccountOwner'
        },
        {
            xtype: 'requirementaspect',
            requirements: [
                { name: 'GkhRegOp.PersonalAccountOwner.Field.BirthDate_Rqrd', applyTo: 'datefield[name=BirthDate]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.RegistrationAddress_Rqrd', applyTo: 'b4selectfield[name=RegistrationAddress]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentityType_Rqrd', applyTo: 'b4enumcombo[name=IdentityType]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentitySerial_Rqrd', applyTo: 'textfield[name=IdentitySerial]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.IdentityNumber_Rqrd', applyTo: 'textfield[name=IdentityNumber]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Patronymic_Rqrd', applyTo: 'textfield[name=SecondName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.CreateDate_Rqrd', applyTo: '#dfOpenDate', selector: 'paowneraccountaddwin [section="add"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.CreateDate_Rqrd', applyTo: '#dfOpenDate', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractNumber_Rqrd', applyTo: '#tfContractNumber', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractDate_Rqrd', applyTo: '#dfContractDate', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.ContractDocument_Rqrd', applyTo: '#ffContractDocument', selector: 'paowneraccountaddwin [section="edit"]' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.DateDocumentIssuance_Rqrd', applyTo: 'datefield[name=DateDocumentIssuance]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Account.Field.Gender_Rqrd', applyTo: 'b4enumcombo[name=Gender]', selector: 'paownereditwin' }
            ]
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'ownerviewpermissionaspect',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setVisible(allowed);
                }
            },
            permissions: [
                { name: 'GkhRegOp.PersonalAccountOwner.Field.PrivilegedCategory_View', applyTo: 'textfield[name=PrivilegedCategory]', selector: 'paownereditwin' },

                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_View', applyTo: 'textfield[name=Surname]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_View', applyTo: 'textfield[name=FirstName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_View', applyTo: 'textfield[name=SecondName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BirthDate_View', applyTo: 'datefield[name=BirthDate]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_View', applyTo: 'textfield[name=BirthPlace]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityType_View', applyTo: 'b4enumcombo[name=IdentityType]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentitySerial_View', applyTo: 'textfield[name=IdentitySerial]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityNumber_View', applyTo: 'textfield[name=IdentityNumber]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.Gender_View', applyTo: 'b4enumcombo[name=Gender]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.DateDocumentIssuance_View', applyTo: 'datefield[name=DateDocumentIssuance]', selector: 'paownereditwin' },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.ActivityStage_View',
                    applyOn: {
                        event: 'show',
                        selector: 'paownereditwin'
                    },
                    applyTo: 'fieldset[name=ActivityStage]',
                    selector: 'paownereditwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BillingAddress_View',
                    applyOn: {
                        event: 'show',
                        selector: 'paownereditwin'
                    },
                    applyTo: 'fieldset[name=BillingAddress]',
                    selector: 'paownereditwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                },

                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_View', applyTo: 'b4selectfield[name=Contragent]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_View', applyTo: 'textfield[name=proxyInn]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_View', applyTo: 'textfield[name=proxyKpp]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_View', applyTo: 'checkbox[name=PrintAct]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Address_View', applyTo: 'b4selectfield[name=Address]', selector: 'paownereditwin' },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Create', applyTo: 'b4addbutton', selector: 'paowneraccountgrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Edit', applyTo: 'b4savebutton', selector: 'paowneraccountaddwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.HouseInfo.Edit', applyTo: 'changevalbtn', selector: 'paowneraccountaddwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkhpermissionaspect',
            name: 'ownereditpermissionaspect',
            applyBy: function (component, allowed) {
                if (component) {
                    component.setReadOnly(!allowed);
                }
            },
            permissions: [
                { name: 'GkhRegOp.PersonalAccountOwner.Field.PrivilegedCategory_Edit', applyTo: 'textfield[name=PrivilegedCategory]', selector: 'paownereditwin' },

                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.Surname_Edit', applyTo: 'textfield[name=Surname]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.FirstName_Edit', applyTo: 'textfield[name=FirstName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.SecondName_Edit', applyTo: 'textfield[name=SecondName]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BirthDate_Edit', applyTo: 'datefield[name=BirthDate]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BirthPlace_Edit', applyTo: 'textfield[name=BirthPlace]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityType_Edit', applyTo: 'b4enumcombo[name=IdentityType]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentitySerial_Edit', applyTo: 'textfield[name=IdentitySerial]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.IdentityNumber_Edit', applyTo: 'textfield[name=IdentityNumber]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.Gender_Edit', applyTo: 'b4enumcombo[name=Gender]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.DateDocumentIssuance_Edit', applyTo: 'datefield[name=DateDocumentIssuance]', selector: 'paownereditwin' },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.BillingAddress_Edit',
                    applyTo: 'fieldset[name=BillingAddress]',
                    selector: 'paownereditwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.ActivityStage_Edit',
                    applyTo: 'b4addbutton',
                    selector: 'paownereditwin activitystagegrid',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Field.Individ.ActivityStage_Edit',
                    applyTo: 'b4savebutton',
                    selector: 'activitystageeditwinowner',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },

                {
                    name: 'GkhRegOp.PersonalAccountOwner.Account.PersonalAccount_Edit',
                    applyTo: 'b4savebutton',
                    selector: 'paowneraccountaddwin',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setDisabled(!allowed);
                        }
                    }
                },

                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Contragent_Edit', applyTo: 'b4selectfield[name=Contragent]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Inn_Edit', applyTo: 'textfield[name=proxyInn]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Kpp_Edit', applyTo: 'textfield[name=proxyKpp]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.PrintAct_Edit', applyTo: 'checkbox[name=PrintAct]', selector: 'paownereditwin' },
                { name: 'GkhRegOp.PersonalAccountOwner.Field.Legal.Address_Edit', applyTo: 'b4selectfield[name=Address]', selector: 'paownereditwin' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'accountOwnerGridEditCtxWindowAspect',
            gridSelector: 'paownergrid',
            editFormSelector: 'paownereditwin',
            modelName: 'regop.owner.PersonalAccountOwner',
            storeName: 'regop.owner.PersonalAccountOwner',
            editWindowView: 'regop.owner.PersonalAccountOwnerEditWindow',
            getModel: function (rec) {
                var me = this,
                    type = rec ? rec.get('OwnerType') : 0;
                switch (type) {
                    case 0:
                        return me.controller.getModel('regop.owner.IndividualAccountOwner');
                    case 1:
                        return me.controller.getModel('regop.owner.LegalAccountOwner');
                }
                return me.callParent(arguments);
            },
            saveRecordHasNotUpload: function (rec) {
                var me = this;
                var frm = me.getForm();
                me.mask('Сохранение', frm);
                rec.save({ id: rec.getId() })
                    .next(function (result) {
                        me.unmask();
                        me.updateGrid();

                        var model = me.getModel(rec);

                        if (result.responseData && result.responseData.data) {
                            var data = result.responseData.data;
                            if (data.length > 0) {
                                var id = data[0] instanceof Object ? data[0].Id : data[0];
                                model.load(id, {
                                    success: function (newRec) {
                                        me.setFormData(newRec);
                                        me.fireEvent('savesuccess', me, newRec);
                                    }
                                });
                            } else {
                                me.fireEvent('savesuccess', me);
                            }
                        } else {
                            me.fireEvent('savesuccess', me);
                        }
                    }, this)
                    .error(function (result) {
                        me.unmask();
                        me.fireEvent('savefailure', result.record, result.responseData);

                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
            },
            onSaveSuccess: function (asp, record) {
                var grid = asp.controller.getPersonalAccountGrid();

                asp.controller.getOwnerIdField().setValue(record.get('Id'));
                grid.setDisabled(false);
                asp.controller.getOwnerTypeCombo().setDisabled(true);
            },
            editRecord: function (record, editMode) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                model = this.getModel(record);

                id ? model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: this
                }) : this.setFormData(new model({ Id: 0 }));

                this.getForm().getForm().isValid();
                this.getForm().editMode = editMode;
            },
            listeners: {
                aftersetformdata: function (asp, record, form) {
                    var grid = asp.controller.getPersonalAccountGrid(),
                        id = +(asp.controller.getOwnerIdField().getValue()),
                        ownerTypeField,
                        value,
                        billingAddressTypes = [10, 20, 30],
                        stageGrid = asp.controller.getActivityStageGrid();

                    id ? grid.getStore().load({
                        params: {
                            fromOwner: true,
                            ownerId: id
                        }
                    }) : grid.getStore().removeAll();
                    grid.setDisabled(id == 0);
                    asp.controller.getOwnerTypeCombo().setDisabled(id > 0);

                    stageGrid.getStore().load();

                    if (record.getId() == 0) {
                        // Если добавляют новую запись проставляем тип абонента как Счет физ.лица
                        ownerTypeField = form.down('b4enumcombo[name="OwnerType"]');
                        value = ownerTypeField.getStore().find('Name', 'Individual');

                        if (value != -1) {
                            ownerTypeField.setValue(value);
                        }
                    }

                    if (record.data
                        && !record.data.OwnerType
                        && billingAddressTypes.indexOf(record.data.BillingAddressType) >= 0) {
                        form.down('#isAddress' + record.data.BillingAddressType).setValue(true);
                    }

                    var RegistrationAddress = record.raw.RegistrationAddress,
                        RegistrationRoom = record.raw.RegistrationRoom,
                        addressInfoTextFld = form.down('[name="RegistrationAddressShowInfo"]');

                    if (RegistrationAddress) {
                        var addressInfoText = RegistrationAddress.Address;

                        if (RegistrationRoom) {
                            addressInfoText += ', кв. '+ RegistrationRoom.RoomNum;
                        }
                        addressInfoTextFld.setValue(addressInfoText);
                    }
                    var isFactAddrSelected = form.down('#isAddress10').checked;
                    var factAddrDocField = form.down('#docPersAccFiasFactAddressField');
                    if (factAddrDocField) factAddrDocField.setReadOnly(!isFactAddrSelected);
                },
                beforesave: function (asp, record) {
                    if (!record.data.OwnerType) {
                        var form = asp.getForm();
                        record.data.BillingAddressType =
                            form.down('#isAddress10').checked ? form.down('#isAddress10').inputValue :
                            form.down('#isAddress20').checked ? form.down('#isAddress20').inputValue :
                            form.down('#isAddress30').checked ? form.down('#isAddress30').inputValue : 0;
                    }
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'ownerActivityStageGridEditWindowAspect',
            gridSelector: 'paownereditwin activitystagegrid',
            editFormSelector: 'activitystageeditwinowner',
            modelName: 'contragent.ActivityStage',
            editWindowView: 'regop.owner.ActivityStageEditWindow',
            getRecordBeforeSave: function (record) {
                if (record && record.get('Id') > 0) {
                    return record;
                }

                var me = this,
                    ownerId = me.controller.getOwnerIdField().getValue();
                record.set('EntityId', ownerId);
                record.set('EntityType', B4.enums.ActivityStageOwner.Individual);
                return record;
            },
            otherActions: function (actions) {
                actions['activitystageeditwinowner [name=ActivityStageType]'] = {
                    'change': {
                        fn: this.disableFields, scope: this
                    }
                };
            },
            disableFields: function (field, value) {
                var grid = field.up(),
                    dateField = grid.down('[name=DateEnd]'),
                    disabled = !(value === B4.enums.ActivityStageType.Bankrupt);

                dateField.setDisabled(disabled);
            }
        },
        {
            xtype: 'accounteditgrid'
        }
    ],
    refs: [
        {
            ref: 'persAccWin',
            selector: 'paowneraccountaddwin'
        },
        {
            ref: 'mainView',
            selector: 'paownergrid'
        },
        {
            ref: 'changeValBtns',
            selector: 'paowneraccountaddwin changevalbtn'
        },
        {
            ref: 'ownerIdField',
            selector: 'paownereditwin hiddenfield[name=Id]'
        },
        {
            ref: 'personalAccountGrid',
            selector: 'paownereditwin paowneraccountgrid'
        },
        {
            ref: 'realtySelectField',
            selector: 'paowneraccountaddwin b4selectfield[name=RealityObject]'
        },
        {
            ref: 'realtyRegistrationSelectField',
            selector: 'registrationaddressaddwin b4selectfield[name=RealityObject]'
        },
        {
            ref: 'roomInfoTextField',
            selector: 'paowneraccountaddwin textfield[name=RoomInfo]'
        },
        {
            ref: 'closeDateContainer',
            selector: 'paowneraccountaddwin container[name=CloseDateContainer]'
        },
        {
            ref: 'areaShareField',
            selector: 'paowneraccountaddwin areasharefield[name=AreaShare]'
        },
        {
            ref: 'historyGrid',
            selector: 'paowneraccountaddwin paowneraccounthistorygrid'
        },
        {
            ref: 'ownerTypeCombo',
            selector: 'paownereditwin b4enumcombo[name=OwnerType]'
        },
        {
            ref: 'roomHiddenField',
            selector: 'paowneraccountaddwin [name=Rooms]'
        },
        {
            ref: 'areaShareAddWindow',
            selector: 'roomareashareaddwindow'
        },
        {
            ref: 'activityStageGrid',
            selector: 'paownereditwin activitystagegrid'
        },
        {
            ref: 'registrationAddress',
            selector: 'paownereditwin [name=RegistrationAddress]'
        },
        {
            ref: 'registrationRoom',
            selector: 'paownereditwin [name=RegistrationRoom]'
        },
        {
            ref: 'registrationaddressshowinfo',
            selector: 'paownereditwin [name=RegistrationAddressShowInfo]'
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('paownergrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();

        var allRows = view.urlToken.indexOf('regop_personal_acc_owner/onlyroom/') <= -1;
        me.disableFilter(allRows);
    },

    init: function () {
        var me = this;
        me.control({
            'paowneraccountaddwin b4selectfield[name=RealityObject]': {
                change: me.onAccountOwnerRealtySelected
            },
            'paowneraccountaddwin #roomAddBtn': {
                click: me.onRoomAddButtonClick
            },
            'paowneraccountaddwin b4selectfield[name=Room]': {
                beforeload: me.onRoomSelectFieldBeforeLoad
            },
            'registrationaddressaddwin b4selectfield[name=Room]': {
                beforeload: me.onRoomRegistrationSelectFieldBeforeLoad
            },
            'paownereditwin paowneraccountgrid': {
                render: function (grid) {
                    grid.getStore().on('beforeload', function (store, operation) {
                        operation.params.fromOwner = true;
                        operation.params.ownerId = me.getOwnerIdField().getValue();
                    });
                }
            },
            'roomareasharegrid': {
                render: {
                    fn: me.onRoomAreaShareGridRender,
                    scope: me
                }
            },
            '[itemName=contragentCa]': {
                change: me.onSelectValueChange
            },
            'b4selectfield[name=Address]': {
                windowcreated: me.onSelectAddressActivate
            },
            'roomareashareaddwindow b4closebutton': {
                click: me.onRoomAddWinClose
            },
            'roomareashareaddwindow #selectBtn': {
                click: me.onRoomAddWinSelectBtnClick
            },
            'registrationaddressaddwin b4savebutton': {
                click: me.onRegistrationAddressAddWinSelectBtnClick
            },
            'paowneraccountaddwin [action=redirecttoroom]': {
                click: me.goToRoom
            },
            'paowneraccountaddwin [action=redirecttopersonalaccount]': {
                click: me.goToPersonalAccount
            },
            'paownergrid': { 'render': { fn: me.onMainViewRender, scope: me } },

            'paownergrid #btnClearAllFilters': { click: { fn: me.goToPAOwnersGrid, scope: me } },

            'paownereditwin activitystagegrid': { 'store.beforeload': { fn: me.onBeforeLoadActivityStage, scope: me } }
        });

        me.callParent(arguments);
    },

    disableFilter: function (disable) {
        var elBtn = this.getMainView().down('#btnClearAllFilters');

        elBtn.setDisabled(disable);

        if (disable) {
            elBtn.el.fadeOut({
                duration: .25,
                callback: this.afterHide,
                scope: this
            });
        } else {
            elBtn.el.fadeIn({
                duration: .25,
                callback: this.afterHide,
                scope: this
            });
        }
    },

    afterHide: function () {
        this.getMainView().getDockedComponent('toptoolbar').doLayout();
    },

    onMainViewRender: function (grid) {
        var me = this;
        grid.getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
    },

    onStoreBeforeLoad: function (store, operation) {
        var me = this,
            grid = me.getMainView(),
            roomId = 0,
            roomIdFromUrl;
        if (grid.urlToken.indexOf('regop_personal_acc_owner/onlyroom/') > -1) {
            roomIdFromUrl = grid.urlToken.replace('regop_personal_acc_owner/onlyroom/', '');
            if (roomIdFromUrl !== '' && roomIdFromUrl.length > 0) {
                if (roomIdFromUrl.indexOf('/') > -1) {
                    roomId = roomIdFromUrl.substring(0, roomIdFromUrl.indexOf('/'));
                } else {
                    roomId = roomIdFromUrl;
                }
            }
        }

        operation.params.RoomId = roomId;
    },

    onBeforeLoadActivityStage: function (store, operation) {
        var me = this,
            ownerId = me.getOwnerIdField().getValue();

        operation.params.entityId = ownerId;
        operation.params.entityType = B4.enums.ActivityStageOwner.Individual;
    },

    goToRoom: function () {
        var me = this;
        var accWin = me.getPersAccWin();
        var innerParams = accWin.params;
        me.application.getRouter().redirectTo('realityobjectedit/' + innerParams.realityObjectId + '/roomedit/' + innerParams.realityObjectRoomId);
    },

    goToPAOwnersGrid: function () {
        B4.getBody().remove(B4.getBody().getActiveTab(), true);
        this.application.redirectTo('regop_personal_acc_owner');
    },

    goToPersonalAccount: function () {
        var me = this,
            accWin = me.getPersAccWin(),
            innerParams = accWin.params;
        me.application.getRouter().redirectTo('personal_acc_details/' + innerParams.personalAccountId);
    },

    edit: function (id, ownerType) {
        var me = this,
            view = this.getMainView() || Ext.widget('paownergrid');

        if (view && !view.rendered) {
            me.bindContext(view);
            me.application.deployView(view);
        }

        me.getAspect('accountOwnerGridEditCtxWindowAspect').editRecord(Ext.create('B4.model.regop.owner.PersonalAccountOwner', { Id: id, OwnerType: +ownerType }), true);
    },

    onSelectAddressActivate: function (field) {
        var contragentSelect, contragentSelectStore, rec;

        function loadAddressFieldStore(record) {
            field.getStore().loadRawData([
                {
                    BillingAddressType: 10,
                    AddressType: 'Фактический адрес',
                    Address: record ? record.data.FactAddress : ''
                },
                {
                    BillingAddressType: 20,
                    AddressType: 'Адрес за пределами субъекта',
                    Address: record ? record.data.AddressOutsideSubject : ''
                },
                {
                    BillingAddressType: 30,
                    AddressType: 'Электронная почта',
                    Address: record ? record.data.Email : ''
                },
                {
                    BillingAddressType: 40,
                    AddressType: 'Юридический адрес',
                    Address: record ? record.data.TrueJuridicalAddress : ''
                },
                {
                    BillingAddressType: 50,
                    AddressType: 'Почтовый адрес',
                    Address: record ? record.data.MailingAddress : ''
                }
            ], false);

        }

        contragentSelect = field.up('fieldset').down('[itemName=contragentCa]');

        if (contragentSelect) {
            contragentSelectStore = contragentSelect.getStore();
            if (contragentSelectStore.getCount() > 0) {
                rec = contragentSelectStore.findRecord('Id', contragentSelect.getValue());
                loadAddressFieldStore(rec);
            } else {
                contragentSelectStore.load({
                    callback: function () {
                        var r = contragentSelectStore.findRecord('Id', contragentSelect.getValue());
                        loadAddressFieldStore(r);
                    },
                    params: {
                        Id: contragentSelect.getValue(),
                        exceptLegalAccountOwners: false
                    }
                });
            }
        }
    },

    /**
     * Дикий хак для фикса неуловимого бага. По непонятным причинам обязательность полей инициализируется неправильно.
     * Происходит это один раз из десяти на одном окружении из десяти.
     * Аспект отрабатывает только при первом создании окна, похоже, позже что-то перетирает allowBlank у контролов.
     * Тут мы применяем аспект еще раз.
     */
    fixRequirementASpect: function () {
        var me = this;
        for (var i = 0; i < me.aspectCollection.items.length; ++i) {
            if (me.aspectCollection.items[i].xtype == 'requirementaspect') {
                me.aspectCollection.items[i].init(me);
                break;
            }
        }
    },

    onSelectValueChange: function (field, val, oldVal) {
        //Ловим момент открытия окна. Очень некрасиво ;(
        if (val === undefined && oldVal === undefined) {
            this.fixRequirementASpect();
        }

        var fl = field.up('fieldset');
        if (val) {
            fl.down('textfield[name=proxyInn]').setValue(val.Inn);
            fl.down('textfield[name=proxyKpp]').setValue(val.Kpp);
        }

        if (!val && oldVal) {
            fl.down('textfield[name=proxyInn]').setValue('');
            fl.down('textfield[name=proxyKpp]').setValue('');
            fl.down('b4selectfield[name=Address]').setValue('');
        }
    },

    onAccountOwnerRealtySelected: function (field, val) {
        var me = this,
            fieldset = field.up('fieldset');

        if (Ext.isObject(val) && val.Id > 0) {

            me.mask('Подождите...', Ext.ComponentQuery.query('paowneraccountaddwin')[0]);

            var roomHiddenField = me.getRoomHiddenField();
            if (roomHiddenField) {
                roomHiddenField.setValue(null);
            }

            B4.Ajax.request({
                url: B4.Url.action('GetTarifForRealtyObject', 'BasePersonalAccount'),
                params: {
                    realtyObjectId: val.Id
                }
            }).next(function (response) {
                if (!response) {
                    return;
                }
                var res = Ext.decode(response.responseText),
                    nfTariff = fieldset.down('numberfield[name=Tariff]');

                if (nfTariff && res.data) {
                    nfTariff.setValue(res.data);
                }

                me.unmask();
            });
        }
    },

    onRoomAreaShareGridRender: function (grid) {
        var me = this;

        grid.getStore().on('beforeload', me.onRoomAreaShareGridStoreBeforeLoad, me);
        grid.getStore().on('load', me.onRoomAreaShareGridStoreLoad, me);
    },

    onRoomAreaShareGridStoreBeforeLoad: function (store, operation) {
        var me = this;

        operation.params.realtyId = me.getRealtySelectField().getValue();
        operation.params.ownerId = me.getOwnerIdField().getValue();
    },

    onRoomAreaShareGridStoreLoad: function (store, records) {
        var me = this,
            roomHiddenFieldValue = me.getRoomHiddenField().getValue();

        if (!Ext.isEmpty(roomHiddenFieldValue)) {
            var addedRooms = Ext.decode(roomHiddenFieldValue);
            Ext.each(records, function (item) {
                for (var i = 0; i < addedRooms.length; i++) {
                    if (item.data.Id === addedRooms[i].Id) {
                        records[item.index].set('Checked', addedRooms[i].Checked);
                        records[item.index].set('AreaShare', addedRooms[i].AreaShare);
                    }
                }
            });
        }
    },

    onRoomSelectFieldBeforeLoad: function (selectfield, operation) {
        operation.params.realtyId = this.getRealtySelectField().getValue();
    },

    onRoomRegistrationSelectFieldBeforeLoad: function (selectfield, operation) {
        operation.params.realtyId = this.getRealtyRegistrationSelectField().getValue();
    },

    onRoomAddButtonClick: function (btn) {
        var me = this,
            roomAreaShareGrid;

        me.roomAreaShareAddWindow = me.getAreaShareAddWindow() || Ext.widget('roomareashareaddwindow');

        me.roomAreaShareAddWindow.show();

        roomAreaShareGrid = me.roomAreaShareAddWindow.down('roomareasharegrid');
        roomAreaShareGrid.getStore().load();
    },

    onRoomAddWinClose: function (btn) {
        var window = btn.up('roomareashareaddwindow'),
            grid = window.down('roomareasharegrid');
        grid.clearHeaderFilters();
        window.close();
    },

    onRoomAddWinSelectBtnClick: function (btn) {
        var me = this,
            grid = btn.up('roomareashareaddwindow').down('roomareasharegrid'),
            checkedArr = [],
            items = grid.getStore().data.items,
            validate = true,
            displayText = '',
            roomInfoTextFld,
            roomHiddenField;

        grid.clearHeaderFilters();

        Ext.each(items, function (item) {
            if (item.get('Checked')) {
                checkedArr.push(item.data);

                if (!Ext.isEmpty(displayText)) {
                    displayText += ', ';
                }

                if (item.get('RoomNum')) {
                    displayText += 'кв. ' + item.get('RoomNum');
                }

                if (displayText != '' && item.get('ChamberNum')) {
                    displayText += ', ' + 'ком. ' + item.get('ChamberNum');
                }
                else if (item.get('ChamberNum')) {
                    displayText += 'ком. ' + item.get('ChamberNum');
                }

                validate = !!item.get('AreaShare');
            }

            return validate;
        });

        if (!validate) {
            B4.QuickMsg.msg('Ошибка!', 'Для всех выбранных записей необходимо указать долю собственности', 'error');
            return;
        }

        roomInfoTextFld = me.getRoomInfoTextField();
        roomHiddenField = me.getRoomHiddenField();

        roomInfoTextFld.setValue(displayText);
        roomHiddenField.setValue(Ext.encode(checkedArr));

        btn.up('roomareashareaddwindow').close();
    }
    ,
    onRegistrationAddressAddWinSelectBtnClick: function (btn) {
        var me = this,
            ro,
            room,
            fields = btn.up('registrationaddressaddwin').down('fieldset'),
            items = fields.items.items,
            validate = false,
            displayText = '';

        Ext.each(items, function (item) {
            if (item.value != null) {
                if (item.value.Address) {
                    displayText += item.value.Address;
                    ro = item.value.Id;
                }
                else if (item.value.RoomNum) {
                    displayText += ', кв. ' + item.value.RoomNum;
                    room = item.value.Id;
                }
                validate = true;
            }

            return validate;
        });

        if (!validate) {
            B4.QuickMsg.msg('Ошибка!', 'Для всех выбранных записей необходимо указать долю собственности', 'error');
            return;
        }

        var addressInfoTextFld = me.getRegistrationaddressshowinfo();
        var hiddenROFld = me.getRegistrationAddress();
        var hiddenRoomFld = me.getRegistrationRoom(); 

        addressInfoTextFld.setValue(displayText);
        hiddenROFld.setValue(Ext.encode(ro));
        hiddenRoomFld.setValue(Ext.encode(room));

        btn.up('registrationaddressaddwin').close();
    }
});