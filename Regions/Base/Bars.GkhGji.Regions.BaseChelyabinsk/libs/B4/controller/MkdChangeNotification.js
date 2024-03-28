Ext.define('B4.controller.MkdChangeNotification', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateButton',
        'B4.aspects.GridEditWindow',
        'B4.aspects.StateContextMenu',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'MkdChangeNotification',
        'MkdChangeNotificationFile'
    ],

    views: [
        'MkdChangeNotificationGrid',
        'MkdChangeNotificationEdit',
        'MkdChangeNotificationFileGrid',
        'MkdChangeNotificationFileEdit'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'mkdchangenotificationgrid'
        },
        {
            ref: 'editWindow',
            selector: 'mkdchangenotificationedit'
        }
    ],

    aspects: [
        {
            xtype: 'statebuttonaspect',
            name: 'mkdChangeNotificationButtonAspect',
            stateButtonSelector: 'mkdchangenotificationedit button[name=StateButton]',
            listeners: {
                transfersuccess: function (asp, entityId, newState) {
                    var editFormAspect = asp.controller.getAspect('mkdChangeNotificationGridWindowAspect'),
                        model = asp.controller.getModel('MkdChangeNotification');

                    asp.setStateData(entityId, newState);

                    model.load(entityId, {
                        success: function (rec) {
                            editFormAspect.setFormData(rec);
                        }
                    });
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdChangeNotificationGridWindowAspect',
            gridSelector: 'mkdchangenotificationgrid',
            editFormSelector: 'mkdchangenotificationedit',
            modelName: 'MkdChangeNotification',
            editWindowView: 'MkdChangeNotificationEdit',
            otherActions: function (actions) {
                var me = this;

                actions[this.editFormSelector + ' b4fiasselectcustomaddress[name=FiasAddress]'] = { 'change': { fn: me.onChangeAddress, scope: me } };
                actions[this.editFormSelector + ' b4selectfield[name=OldMkdManagementMethod]'] = { 'change': { fn: me.onChangeOldMethod, scope: me } };
                actions[this.editFormSelector + ' b4selectfield[name=OldManagingOrganization]'] = { 'change': { fn: me.onChangeOldOrganization, scope: me } };
                actions[this.editFormSelector + ' b4selectfield[name=NewMkdManagementMethod]'] = { 'change': { fn: me.onChangeNewMethod, scope: me } };
                actions[this.editFormSelector + ' b4selectfield[name=NewManagingOrganization]'] = { 'change': { fn: me.onChangeNewOrganization, scope: me } };
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this, 
                        fileGrid = asp.getForm().down('mkdchangenotificationfilegrid');

                    asp.controller.getAspect('mkdChangeNotificationButtonAspect').setStateData(record.getId(), record.get('State'));
                    asp.controller.notificationId = record.getId();

                    var store = fileGrid.getStore();
                    store.on('beforeload', me.onBeforeLoad, me);
                    store.load();

                    if (asp.controller.notificationId > 0) {
                        fileGrid.setDisabled(false);
                    } else {
                        fileGrid.setDisabled(true);
                    }
                }
            },
            onBeforeLoad: function (store, operation) {
                var me = this;
                operation.params.notificationId = me.controller.notificationId;
            },
            onChangeAddress: function (field, newValue) {
                if (!field.addressIsLoad) {
                    return;
                }

                var me = this,
                    form = me.getForm(),
                    oldMethodFld = form.down('[name=OldMkdManagementMethod]');

                if (!me.checkDirectManaging(oldMethodFld.getValue())) {
                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('GetManagingOrgByAddressName', 'MkdChangeNotification'),
                        params: {
                            addressName: newValue.AddressName
                        }
                    }).next(function(response) {
                        var obj = Ext.JSON.decode(response.responseText),
                            managingOrgFld = form.down('[name=OldManagingOrganization]'),
                            innFld = form.down('[name=OldInn]'),
                            ogrnFld = form.down('[name=OldOgrn]');

                        if (obj.Contragent) {
                            managingOrgFld.setValue(obj);
                            managingOrgFld.updateDisplayedText(obj.Contragent.Name);
                        }
                        else {
                            managingOrgFld.setValue(null);
                            innFld.setValue('');
                            ogrnFld.setValue('');
                        }
                    }).error(function() {
                    });
                }
            },

            onChangeOldMethod: function(field, newValue) {
                if (!newValue) {
                    return;
                }

                var me = this,
                    form = me.getForm(),
                    managingOrgFld = form.down('[name=OldManagingOrganization]'),
                    innFld = form.down('[name=OldInn]'),
                    ogrnFld = form.down('[name=OldOgrn]');

                if (!newValue.Name) {
                    if (me.checkDirectManaging(newValue)) {
                        managingOrgFld.disable();
                        innFld.disable();
                        ogrnFld.disable();

                        managingOrgFld.allowBlank = true;
                    }
                    return;
                }

                if (!me.checkDirectManaging(newValue.Name)) {
                    managingOrgFld.enable();
                    innFld.enable();
                    ogrnFld.enable();

                    managingOrgFld.allowBlank = false;
                } else {
                    managingOrgFld.disable();
                    innFld.disable();
                    ogrnFld.disable();

                    managingOrgFld.setValue(null);
                    innFld.setValue('');
                    ogrnFld.setValue('');

                    managingOrgFld.allowBlank = true;
                }
            },

            onChangeOldOrganization: function (field, newValue) {
                if (!newValue || !newValue.Id) {
                    return;
                }

                var me = this,
                    form = me.getForm(),
                    oldMethodFld = form.down('b4selectfield[name=OldMkdManagementMethod]');

                if (!me.checkDirectManaging(oldMethodFld.getValue())) {
                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('GetManagingOrgDetails', 'MkdChangeNotification'),
                        params: {
                            manOrgId: newValue.Id
                        }
                    }).next(function(response) {
                        var obj = Ext.JSON.decode(response.responseText),
                            innFld = form.down('[name=OldInn]'),
                            ogrnFld = form.down('[name=OldOgrn]');

                        innFld.setValue(obj.Inn);
                        ogrnFld.setValue(obj.Ogrn);
                    }).error(function() {
                    });
                }
            },

            onChangeNewMethod: function (field, newValue) {
                if (!newValue) {
                    return;
                }

                var me = this,
                    form = me.getForm(),
                    managingOrgFld = form.down('[name=NewManagingOrganization]'),
                    innFld = form.down('[name=NewInn]'),
                    ogrnFld = form.down('[name=NewOgrn]'),
                    jurAddressFld = form.down('[name=NewJuridicalAddress]'),
                    managerFld = form.down('[name=NewManager]'),
                    phoneFld = form.down('[name=NewPhone]'),
                    emailFld = form.down('[name=NewEmail]'),
                    siteFld = form.down('[name=NewOfficialSite]');

                if (!newValue.Name) {
                    if (me.checkDirectManaging(newValue)) {
                        managingOrgFld.disable();
                        innFld.disable();
                        ogrnFld.disable();
                        jurAddressFld.disable();
                        managerFld.disable();
                        phoneFld.disable();
                        emailFld.disable();
                        siteFld.disable();

                        managingOrgFld.allowBlank = true;
                    }
                    return;
                }

                if (!me.checkDirectManaging(newValue.Name)) {
                    managingOrgFld.enable();
                    innFld.enable();
                    ogrnFld.enable();
                    jurAddressFld.enable();
                    managerFld.enable();
                    phoneFld.enable();
                    emailFld.enable();
                    siteFld.enable();

                    managingOrgFld.allowBlank = false;
                } else {
                    managingOrgFld.disable();
                    innFld.disable();
                    ogrnFld.disable();
                    jurAddressFld.disable();
                    managerFld.disable();
                    phoneFld.disable();
                    emailFld.disable();
                    siteFld.disable();

                    managingOrgFld.setValue(null);
                    innFld.setValue('');
                    ogrnFld.setValue('');
                    jurAddressFld.setValue('');
                    managerFld.setValue('');
                    phoneFld.setValue('');
                    emailFld.setValue('');
                    siteFld.setValue('');

                    managingOrgFld.allowBlank = true;
                }
            },

            onChangeNewOrganization: function (field, newValue) {
                if (!newValue || !newValue.Id) {
                    return;
                }

                var me = this,
                    form = me.getForm(),
                    oldMethodFld = form.down('b4selectfield[name=NewMkdManagementMethod]');

                if (!me.checkDirectManaging(oldMethodFld.getValue())) {
                    B4.Ajax.request({
                        method: 'GET',
                        url: B4.Url.action('GetManagingOrgDetails', 'MkdChangeNotification'),
                        params: {
                            manOrgId: newValue.Id
                        }
                    }).next(function (response) {
                        var obj = Ext.JSON.decode(response.responseText),
                            innFld = form.down('[name=NewInn]'),
                            ogrnFld = form.down('[name=NewOgrn]'),
                            jurAddressFld = form.down('[name=NewJuridicalAddress]'),
                            managerFld = form.down('[name=NewManager]'),
                            phoneFld = form.down('[name=NewPhone]'),
                            emailFld = form.down('[name=NewEmail]'),
                            siteFld = form.down('[name=NewOfficialSite]');

                        innFld.setValue(obj.Inn);
                        ogrnFld.setValue(obj.Ogrn);
                        jurAddressFld.setValue(obj.JuridicalAddress);
                        managerFld.setValue(obj.Manager);
                        phoneFld.setValue(obj.Phone);
                        emailFld.setValue(obj.Email);
                        siteFld.setValue(obj.OfficialWebsite);

                    }).error(function () {
                    });
                }
            },
            checkDirectManaging: function(val) {
                return val == 'Непосредственное управление';
            }
        },
        {
            xtype: 'b4_state_contextmenu',
            stateType: 'gji_mkd_change_notification',
            menuSelector: 'qqq',
            gridSelector: 'mkdchangenotificationgrid'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhGji.MkdChangeNotification.Create',
                    applyTo: 'b4addbutton',
                    selector: 'mkdchangenotificationgrid'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'mkdchangenotificationgrid'
                }
            ]
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'mkdChangeNotificationStatePermissionAspect',
            editFormAspectName: 'mkdChangeNotificationGridWindowAspect',
            setPermissionEvent: 'aftersetformdata',
            permissions: [
                {
                    name: 'GkhGji.MkdChangeNotification.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.Date.Edit',
                    applyTo: '[name=Date]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.FiasAddress.Edit',
                    applyTo: '[name=FiasAddress]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.NotificationCause.Edit',
                    applyTo: '[name=NotificationCause]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.InboundNumber.Edit',
                    applyTo: '[name=InboundNumber]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.RegistrationDate.Edit',
                    applyTo: '[name=RegistrationDate]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.OldMkdManagementMethod.Edit',
                    applyTo: '[name=OldMkdManagementMethod]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.OldManagingOrganization.Edit',
                    applyTo: '[name=OldManagingOrganization]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.NewMkdManagementMethod.Edit',
                    applyTo: '[name=NewMkdManagementMethod]',
                    selector: 'mkdchangenotificationedit'
                },
                {
                    name: 'GkhGji.MkdChangeNotification.Fields.NewManagingOrganization.Edit',
                    applyTo: '[name=NewManagingOrganization]',
                    selector: 'mkdchangenotificationedit'
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'mkdChangeNotificationButtonExportAspect',
            gridSelector: 'mkdchangenotificationgrid',
            buttonSelector: 'mkdchangenotificationgrid #btnExport',
            controllerName: 'MkdChangeNotification',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdChangeNotificationFileGridWindowAspect',
            gridSelector: 'mkdchangenotificationfilegrid',
            editFormSelector: 'mkdchangenotificationfileedit',
            modelName: 'MkdChangeNotificationFile',
            editWindowView: 'MkdChangeNotificationFileEdit',
            listeners: {
                beforesave: function (asp, record) {
                    record.set('MkdChangeNotification', asp.controller.notificationId);
                    return true;
                }
            }
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'mkdChangeNotificationFileGridWindowAspect',
            gridSelector: 'mkdchangenotificationfilegrid',
            editFormSelector: 'mkdchangenotificationfileedit',
            storeName: 'MkdChangeNotificationFile',
            modelName: 'MkdChangeNotificationFile',
            editWindowView: 'MkdChangeNotificationFileEdit',
            listeners: {
                beforesave: function (asp, record) {
                    record.set('MkdChangeNotification', asp.controller.notificationId);
                },
                deletesuccess: function () {
                    this.getGrid().getStore().load();
                }
            },
            onSaveSuccess: function(aspect) {
                var form = aspect.getForm();
                if (form) {
                    form.close();
                }

                aspect.getGrid().getStore().load();
            }
        }
    ],

    init: function() {
        this.callParent(arguments);
    },
    
    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('mkdchangenotificationgrid');

        me.bindContext(view);
        me.application.deployView(view);
        view.getStore().load();
    }
});