Ext.define('B4.controller.realityobj.ManagOrg', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.realityobj.ManagOrg'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'realityobj.DirectManagContract',
        'manorg.contract.Base'
    ],
    
    stores: [
        'realityobj.DirectManagContract',
        'realityobj.Contract'
    ],
    
    views: [
        'realityobj.ContractGrid',
        'realityobj.DirectManagEditWindow'
        //'manorg.contract.Grid'
    ],
    
    refs: [
        {
            ref: 'mainView',
            selector: 'realobjcontractgrid'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    aspects: [
        {
            xtype: 'realityobjmanagorgperm',
            name: 'realityObjManagOrgPerm'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.RealityObject.Register.ManagOrg.ServiceContract.View',
                    applyTo: '#serviceContractPanel',
                    selector: 'realobjdirmanageditwin',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Gkh.RealityObject.Register.ManagOrg.ServiceContract.Edit',
                    applyTo: '#serviceContractPanel',
                    selector: 'realobjdirmanageditwin',
                    applyBy: function (component, allowed) {
                        function setAccess(name) {
                            var cmp = component.down(name);

                            if (cmp) {
                                if (allowed && chechbox.checked) {
                                    cmp.enable();
                                } else {
                                    cmp.disable();
                                }
                            }
                        }
                        
                        if (!component) {
                            return;
                        }
                        
                        var chechbox = component.down('checkbox');
                        
                        if (chechbox) {
                            chechbox.setDisabled(!allowed);

                            setAccess('b4selectfield');
                            setAccess('datefield[name="DateStartService"]');
                            setAccess('datefield[name="DateEndService"]');
                            setAccess('b4filefield');
                        }
                    }
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'realityobjManagOrgGridWindowAspect',
            gridSelector: 'realobjcontractgrid',
            editFormSelector: 'realobjdirmanageditwin',
            storeName: 'realityobj.Contract',
            modelName: 'realityobj.DirectManagContract',
            editWindowView: 'realityobj.DirectManagEditWindow',
            otherActions: function (actions) {
                var me = this;
                actions['realobjdirmanageditwin checkbox[name="IsServiceContract"]'] = {
                    'change': { fn: me.onChangeCheckBox, scope: me }
                };
            },
            rowDblClick: function (view, record) {
                var me = this;
                if (record.data.TypeContractManOrgRealObj == 40) {
                    me.editRecord(record);
                }
            },
            onChangeCheckBox: function (field, newValue) {
                var form = field.up(),
                    manOrgField = form.down('b4selectfield[name="ManagingOrganization"]'),
                    dateStartServField = form.down('datefield[name="DateStartService"]'),
                    dateEndServField = form.down('datefield[name="DateEndService"]'),
                    servFileField = form.down('b4filefield[name="ServContractFile"]'),
                    forceDisable = field.disabled;

                manOrgField.setDisabled(forceDisable || !newValue);
                dateStartServField.setDisabled(forceDisable || !newValue);
                dateEndServField.setDisabled(forceDisable || !newValue);
                servFileField.setDisabled(forceDisable || !newValue);

                if (!newValue) {
                    manOrgField.setValue(null);
                    dateStartServField.setValue(null);
                    dateEndServField.setValue(null);
                    servFileField.setValue(null);
                }
            },
            listeners: {
                beforesave: function (asp, record) {
                    var isServContract = record.get('IsServiceContract');

                    if (!isServContract) {
                        record.set('ManagingOrganization', null);
                        record.set('DateStartService', null);
                        record.set('DateEndService', null);
                        record.set('ServContractFile', null);
                    }

                    return true;
                },
                getdata: function (me, record) {
                    record.set('RealityObjectId', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                    record.set('TypeContractManOrgRealObj', 40);
                },
                beforerowaction: function (asp, grid, action, record) {
                    switch (action.toLowerCase()) {
                        case 'doubleclick':
                        case 'edit':
                            if (record.data.TypeContractManOrgRealObj != 40) {
                                if (record.get('ManagingOrganization')) {
                                    Ext.History.add(Ext.String.format('managingorganizationedit/{0}/contract?contractId={1}&type={2}', record.get('ManagingOrganization'), record.getId(), record.get('TypeContractManOrgRealObj')));
                                }
                                return false;
                            }
                            return true;
                        case 'delete':
                            if (record.data.TypeContractManOrgRealObj != 40) {
                                Ext.Msg.alert('Ошибка!', 'В данном разделе можно удалять только записи по непосредственному управлению. Для удаления остальных необходимо перейти в Участники процесса - Управляющие организации - раздел \"Управление домами\".');
                                return false;
                            }
                            return true;
                    }
                    return false;
                }
            },
            gridAction: function (grid, action) {
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'adddirectmanag':
                            {
                                this.editRecord();
                            }
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
            }
        }
    ],

    index: function (id) {
        var me = this,
            store,
            view = me.getMainView() || Ext.widget('realobjcontractgrid');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        me.getAspect('realityObjManagOrgPerm').setPermissionsByRecord({ getId: function () { return id; } });
        store = view.getStore();
        store.filters.clear();
        store.filter([
            {
                property: "realityObjectId",
                value: id
            }
        ]);
    }
});