Ext.define('B4.controller.realityobj.OwnerProtocol', {
    extend: 'B4.controller.MenuItemController',

    params: {},

    requires: [
        'B4.Url', 'B4.Ajax',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GridEditWindow',
        'B4.enums.PropertyOwnerDecisionType',
        'B4.aspects.permission.realityobj.OwnerProtocol'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'BasePropertyOwnerDecision',
        'PropertyOwnerProtocols',
        'PropertyOwnerDecisionWork',
        'MinAmountDecision',
        'dict.WorkSelect',
        'dict.WorkSelected',
        'OwnerAccountContragent'
    ],

    models: [
        'PropertyOwnerProtocols',
        'BasePropertyOwnerDecision',
        'PropertyOwnerDecisionWork',
        'RegOpAccountDecision',
        'SpecialAccountDecision',
        'SpecialAccountDecisionNotice',
        'MinAmountDecision',
        'dict.Work',
        'RealityObject',
        'ListServicesDecision',
        'MinFundSizeDecision',
        'PrevAccumulatedAmountDecision',
        'CreditOrganizationDecision',
        'OwnerAccountDecision'
    ],

    views: [
        'realityobj.protocol.Grid',
        'realityobj.protocol.Panel',
        'realityobj.protocol.DecisionGrid',
        'longtermprobject.propertyownerprotocols.EditWindow',
        'longtermprobject.propertyownerdecision.work.Grid',
        'longtermprobject.propertyownerdecision.Grid',
        'longtermprobject.propertyownerdecision.SpecAccEditWindow',
        'longtermprobject.propertyownerdecision.RegOpEditWindow',
        'longtermprobject.propertyownerdecision.SpecAccNoticePanel',
        'longtermprobject.propertyownerdecision.MinAmountEditWindow',
        'longtermprobject.propertyownerdecision.ListServicesEditWindow',
        'longtermprobject.propertyownerdecision.OwnerAccountEditWindow',
        'longtermprobject.propertyownerdecision.CreditOrgEditWindow',
        'longtermprobject.propertyownerdecision.MinFundSizeEditWindow',
        'longtermprobject.propertyownerdecision.PreAmountEditWindow',
        'longtermprobject.propertyownerdecision.AddWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',
    refs: [
        {
            ref: 'mainView',
            selector: 'roprotpanel'
        },
        {
            ref: 'grid',
            selector: 'roprotocolgrid'
        },
        {
            ref: 'decisionGrid',
            selector: 'roprotdecisiongrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'protocolaspect',
            gridSelector: 'roprotocolgrid',
            editFormSelector: '#propertyownerprotocolsEditWindow',
            storeName: 'PropertyOwnerProtocols',
            modelName: 'PropertyOwnerProtocols',
            editWindowView: 'longtermprobject.propertyownerprotocols.EditWindow',
            listeners: {
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                    }
                    return true;
                }
            }
        },

         //На специальном счете
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionspecaccaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionspecaccwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'SpecialAccountDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.SpecAccEditWindow',

             otherActions: function (actions) {
                 var me = this;

                 actions['longtermdecisionspecaccwindow combobox[name="TypeOrganization"]'] = { 'change': { fn: me.onTypeOrganizationChange, scope: me } };
                 actions['longtermdecisionspecaccwindow b4selectfield[name="ManagingOrganization"]'] = { 'beforeload': { fn: me.beforeLoadManOrg, scope: me } };
                 actions['longtermdecisionspecaccwindow b4selectfield[name="CreditOrg"]'] = { 'change': { fn: me.onChangeCreditOrg, scope: me } };
             },

             rowDblClick: function (view, record) {
                 var methodFormFund = record.get('MethodFormFund');
                 var decisionType = record.get('Decision');
                 if (decisionType === 10 && methodFormFund === 20) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     var aspNotice, model;

                     asp.controller.params.decisionId = record.getId();
                     aspNotice = this.controller.getAspect('longTermSpecAccNoticePanelAspect');

                     model = aspNotice.getModel();
                     model.load(record.getId(), {
                         success: function (rec) {
                             aspNotice.setPanelData(rec);
                         },
                         scope: this
                     });

                     form.setTitle(B4.enums.PropertyOwnerDecisionType.displayRenderer(record.get('PropertyOwnerDecisionType')));

                 },
                 aftersetformdata: function (asp, rec, form) {

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }
                     asp.controller.params.decisionId = rec.getId();

                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var methodFormFund = record.get('MethodFormFund');
                     var decisionType = record.get('Decision');
                     if (decisionType === 10 && methodFormFund === 20) {
                         return true;
                     }
                     return false;
                 }
             },
             onChangeCreditOrg: function (fld, newValue) {
                 var form = fld.up(),
                     mailAddrField = form.down('textfield[name="MailingAddress"]'),
                     innField = form.down('textfield[name="Inn"]'),
                     kppField = form.down('textfield[name="Kpp"]'),
                     ogrnField = form.down('textfield[name="Ogrn"]'),
                     okpoField = form.down('textfield[name="Okpo"]'),
                     bikField = form.down('textfield[name="Bik"]'),
                     corrAccountField = form.down('textfield[name="CorrAccount"]');

                 if (newValue != null) {
                     mailAddrField.setValue(newValue.MailingAddress);
                     innField.setValue(newValue.Inn);
                     kppField.setValue(newValue.Kpp);
                     ogrnField.setValue(newValue.Ogrn);
                     okpoField.setValue(newValue.Okpo);
                     bikField.setValue(newValue.Bik);
                     corrAccountField.setValue(newValue.CorrAccount);
                 } else {
                     mailAddrField.setValue(null);
                     innField.setValue(null);
                     kppField.setValue(null);
                     ogrnField.setValue(null);
                     okpoField.setValue(null);
                     bikField.setValue(null);
                     corrAccountField.setValue(null);
                 }
             },
             beforeLoadManOrg: function (sender, options) {
                 var typeManOrg = this.getForm().down('combobox[name="TypeOrganization"]').getValue();

                 options.params = {};
                 options.params.tsjOnly = typeManOrg == 20;
                 options.params.jskOnly = typeManOrg == 40;
             },
             editWindowElementsMovement: function (editWindow, typeOrganization) {
                 if (editWindow) {
                     var sfManagingOrganization = editWindow.down('b4selectfield[name="ManagingOrganization"]');

                     if (sfManagingOrganization) {

                         sfManagingOrganization.setValue(0);
                         if (typeOrganization == 20 || typeOrganization == 40) {
                             sfManagingOrganization.show();
                             sfManagingOrganization.allowBlank = false;
                         } else if (typeOrganization == 60) {
                             sfManagingOrganization.hide();
                             sfManagingOrganization.allowBlank = true;
                         } else if (typeOrganization == 50) {
                             sfManagingOrganization.hide();
                             sfManagingOrganization.allowBlank = true;
                         }
                     }
                 }
             },
             onTypeOrganizationChange: function (sender, newVal) {
                 if (Ext.isEmpty(newVal)) {
                     return;
                 }

                 var editWindow = sender.up();

                 this.editWindowElementsMovement(editWindow, newVal);
             }
         },


         //На счете регионального оператора
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionregopaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionregopwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'RegOpAccountDecision',
             editWindowView: 'B4.view.longtermprobject.propertyownerdecision.RegOpEditWindow',

             rowDblClick: function (view, record) {
                 var methodFormFund = record.get('MethodFormFund');
                 var decisionType = record.get('Decision');
                 if (decisionType === 10 && methodFormFund === 10) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     asp.controller.params.decisionId = record.getId();
                     form.setTitle(B4.enums.PropertyOwnerDecisionType.displayRenderer(record.get('PropertyOwnerDecisionType')));
                 },
                 aftersetformdata: function (asp, rec, form) {

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }
                     asp.controller.params.decisionId = rec.getId();
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var methodFormFund = record.get('MethodFormFund');
                     var decisionType = record.get('Decision');
                     if (decisionType === 10 && methodFormFund === 10) {
                         return true;
                     }
                     return false;
                 }
             }
         },

         //Установление минимального размера взноса на кап.ремонт
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionminamountaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionminamountwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'MinAmountDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.MinAmountEditWindow',

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 20) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 20) {
                         return true;
                     }
                     return false;
                 }
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

         //Установление фактических дат проведения КР
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionlistservicesaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionlistserviceswindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'ListServicesDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.ListServicesEditWindow',

             otherActions: function (actions) {
                 var me = this;

                 actions['longtermdecisionlistserviceswindow combobox'] = {
                     'render': {
                         fn: function (grid) {
                             grid.getStore().on('beforeload', me.onWorkStoreBeforeLoad, me);
                         },
                         scope: this
                     }
                 };
                 actions['longtermdecisionlistserviceswindow listservicesworksgrid'] = {
                     'render': {
                         fn: function (grid) {
                             grid.getStore().on('beforeload', me.onBeforeLoadListServiceWorksGrid, me);
                         },
                         scope: this
                     }
                 };
             },

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 30) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {
                     var decisionType = rec.get('PropertyOwnerDecisionType'),
                         grid;

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                     grid = form.down('listservicesworksgrid');
                     grid.getStore().load();
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 30) {
                         return true;
                     }
                     return false;
                 }
             },
             onBeforeLoadListServiceWorksGrid: function (sender, options) {
                 options.params = {};
                 options.params.decisionId = this.controller.params.decisionId;
             },
             onWorkStoreBeforeLoad: function (store) {
                 var id = this.getForm().getRecord().getId();

                 Ext.apply(store.getProxy().extraParams, { decision_id: id });
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

         //Выбор (смена) владельца специального счета
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionowneraccountaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionowneraccountwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'OwnerAccountDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.OwnerAccountEditWindow',

             otherActions: function (actions) {
                 var me = this;

                 actions['longtermdecisionowneraccountwindow'] = { 'show': { fn: me.onContragentAllowBlank, scope: me } };
                 actions['longtermdecisionowneraccountwindow combobox[name="OwnerAccountType"]'] = { 'change': { fn: me.onChangeOwnerAccountType, scope: me } };
                 actions['longtermdecisionowneraccountwindow b4selectfield[name="Contragent"]'] = { 'beforeload': { fn: me.beforeLoadPropertyOwnerAccountContragent, scope: me } };
             },

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 40) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 40) {
                         return true;
                     }
                     return false;
                 }
             },
             beforeLoadPropertyOwnerAccountContragent: function (field, options) {
                 var form = field.up(),
                     valueType = form.down('combobox[name="OwnerAccountType"]').getValue();

                 options.params = {};
                 options.params.typeOwner = valueType;
             },
             onChangeOwnerAccountType: function (field, newValue) {
                 var form = field.up(),
                     sfContragent = form.down('b4selectfield[name="Contragent"]');

                 sfContragent.allowBlank = newValue == 50;
                 sfContragent.validate();
             },
             onContragentAllowBlank: function () {
                 this.getForm().down('b4selectfield[name="Contragent"]').allowBlank = true;
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

         //Выбор кредитной организации
         {
             xtype: 'grideditwindowaspect',
             name: 'decisioncreditorgaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisioncreditorgwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'CreditOrganizationDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.CreditOrgEditWindow',

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 50) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 50) {
                         return true;
                     }
                     return false;
                 }
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

         //Установление минимального размера фонда КР
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionminfundsizeaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionminfundsizewindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'MinFundSizeDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.MinFundSizeEditWindow',

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 60) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {
                     var decisionType = rec.get('PropertyOwnerDecisionType'),
                         grid;

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                     if (decisionType === 30) {
                         grid = form.down('listservicesworksgrid');
                         grid.getStore().load();
                     }
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },
                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 60) {
                         return true;
                     }
                     return false;
                 }
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

        //Установление ранее накопленной суммы на КР (до 01.04.2014)
         {
             xtype: 'grideditwindowaspect',
             name: 'decisionpreamountaspect',
             gridSelector: 'roprotdecisiongrid',
             editFormSelector: 'longtermdecisionpreamountwindow',
             storeName: 'BasePropertyOwnerDecision',
             modelName: 'PrevAccumulatedAmountDecision',
             editWindowView: 'longtermprobject.propertyownerdecision.PreAmountEditWindow',

             rowDblClick: function (view, record) {
                 var decisionType = record.get('Decision');
                 if (decisionType === 70) {
                     this.superclass.rowDblClick.apply(this, arguments);
                 }
             },

             listeners: {
                 beforesetformdata: function (asp, record, form) {
                     if (record.phantom) {
                         this.setOrgFormData(form);
                     }
                 },
                 aftersetformdata: function (asp, rec, form) {
                     var decisionType = rec.get('PropertyOwnerDecisionType'),
                         grid;

                     if (asp.controller.params.decisId) {
                         form.down('.tabpanel').setActiveTab(1);
                         delete asp.controller.params.decisId;
                     }

                     asp.controller.params.decisionId = rec.getId();
                     if (decisionType === 30) {
                         grid = form.down('listservicesworksgrid');
                         grid.getStore().load();
                     }
                 },
                 beforesave: function (asp, obj) {
                     if (!obj.getId()) {
                         obj.set('RealityObject', asp.controller.params.realityObjectId);
                         obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                     }
                     return true;
                 },
                 savesuccess: function (asp, rec) {
                     var decisionType = rec.get('PropertyOwnerDecisionType');
                     if (decisionType === 30) {
                         asp.getForm().down('listservicesworksgrid').getStore().sync();
                     }
                 },


                 beforegridaction: function (asp, grid, action) {
                     return false;
                 },
                 beforerowaction: function (asp, grid, action, record) {
                     var decisionType = record.get('Decision');
                     if (decisionType === 70) {
                         return true;
                     }
                     return false;
                 }
             },
             setOrgFormData: function (editWindow) {
                 var me = this;
                 if (editWindow) {
                     var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                     me.controller.mask('Загрузка...', editWindow);

                     B4.Ajax.request({
                         url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                         params: {
                             realObjId: me.controller.params.realityObjectId
                         }
                     }).next(function (resp) {
                         var res = Ext.decode(resp.responseText);
                         orgFormField.setValue(res);
                         me.controller.unmask();
                     }).error(function (e) {
                         B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                         me.controller.unmask();
                     });
                 }
             }
         },

        //аспект для добавления
        {
            xtype: 'grideditwindowaspect',
            name: 'decisionaddaspect',
            gridSelector: 'roprotdecisiongrid',
            editFormSelector: 'longtermdecisionaddwindow',
            storeName: 'BasePropertyOwnerDecision',
            modelName: 'BasePropertyOwnerDecision',
            editWindowView: 'longtermprobject.propertyownerdecision.AddWindow',

            otherActions: function (actions) {
                var me = this;
                actions['longtermdecisionaddwindow b4savebutton'] = { click: { fn: me.saveDecision, scope: me } };
                actions['longtermdecisionaddwindow b4closebutton'] = { click: { fn: me.closeWindowHandler, scope: me } };
                actions['longtermdecisionaddwindow combobox[name="MoOrganizationForm"]'] = { 'change': { fn: me.onChangeOrgForm, scope: me } };
                actions['longtermdecisionaddwindow combobox[name="PropertyOwnerDecisionType"]'] = { 'change': { fn: me.onChangeDecType, scope: me } };
            },

            rowDblClick: function (view, record) { },

            listeners: {
                beforesetformdata: function (asp, record, form) {
                    if (record.phantom) {
                        this.setOrgFormData(form);
                    }
                },
                aftersetformdata: function (asp, rec, form) {
                    var decisionType = rec.get('PropertyOwnerDecisionType'),
                        grid;

                    if (asp.controller.params.decisId) {
                        form.down('.tabpanel').setActiveTab(1);
                        delete asp.controller.params.decisId;
                    }

                    asp.controller.params.decisionId = rec.getId();
                    if (decisionType === 30) {
                        grid = form.down('listservicesworksgrid');
                        grid.getStore().load();
                    }
                },
                beforesave: function (asp, obj) {
                    if (!obj.getId()) {
                        obj.set('RealityObject', asp.controller.params.realityObjectId);
                        obj.set('PropertyOwnerProtocol', asp.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                    }
                    return true;
                },
                savesuccess: function (asp, rec) {
                    var decisionType = rec.get('PropertyOwnerDecisionType');
                    if (decisionType === 30) {
                        asp.getForm().down('listservicesworksgrid').getStore().sync();
                    }
                },
                beforerowaction: function (asp, grid, action, record) {
                    return false;
                }
            },
            onChangeOrgForm: function (field, newValue) {
                var formFundField = field.up('window').down('combobox[name="MethodFormFund"]');

                if (newValue == 10 || newValue == 20) {
                    formFundField.setValue(10);
                    formFundField.setReadOnly(true);
                }
            },
            saveDecision: function () {
                var me = this, rec, form = this.getForm();
                if (form.getForm().isValid()) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());
                    me.controller.mask('Загрузка...', form);
                    rec.set('PropertyOwnerProtocol', me.controller.getGrid().getSelectionModel().getSelection()[0].getId());
                    rec.set('RealityObject', me.controller.params.realityObjectId);

                    B4.Ajax.request({
                        url: B4.Url.action('SaveDecision', 'LongTermPrObject'),
                        params: {
                            records: Ext.encode([rec.data])
                        }
                    }).next(function () {
                        me.controller.unmask();
                        me.getGrid().getStore().load();
                        form.close();
                    }).error(function () {
                        me.controller.unmask();
                    });
                }
            },
            setOrgFormData: function (editWindow) {
                var me = this;
                if (editWindow) {
                    var orgFormField = editWindow.down('combobox[name="MoOrganizationForm"]');
                    me.controller.mask('Загрузка...', editWindow);

                    B4.Ajax.request({
                        url: B4.Url.action('GetOrgForm', 'LongTermPrObject'),
                        params: {
                            realObjId: me.controller.params.realityObjectId
                        }
                    }).next(function (resp) {
                        var res = Ext.decode(resp.responseText);
                        orgFormField.setValue(res);
                        me.controller.unmask();
                    }).error(function (e) {
                        B4.QuickMsg.msg('Предупреждение', e.message, 'warning');
                        me.controller.unmask();
                    });
                }
            },
            onChangeDecType: function (field, newValue) {
                var form = field.up(),
                    formFundField = form.down('combobox[name="MethodFormFund"]'),
                    orgForm = form.down('combobox[name="MoOrganizationForm"]').getValue();

                if (newValue == 20 || newValue == 30 || newValue == 40
                   || newValue == 50 || newValue == 60 || newValue == 70) {
                    //formFundField.setValue(null);
                    formFundField.setReadOnly(true);
                    formFundField.allowBlank = true;
                } else if (orgForm == 10 || orgForm == 20) {
                    formFundField.setValue(10);
                    formFundField.setReadOnly(true);
                } else {
                    formFundField.setReadOnly(false);
                    formFundField.allowBlank = false;
                }

                formFundField.validate();
            }
        },

        {
            /*
            * Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'longTermSpecAccNoticeAspect',
            stateButtonSelector: 'longtermdecisionspecaccnoticepanel #btnState',
            listeners: {
                transfersuccess: function (asp) {
                    asp.controller.getAspect('longTermSpecAccNoticePanelAspect').setData(asp.controller.params.decisionId);
                }
            }
        },
        {
            // Аспект уведомления
            xtype: 'gkheditpanel',
            name: 'longTermSpecAccNoticePanelAspect',
            editPanelSelector: 'longtermdecisionspecaccnoticepanel',
            modelName: 'SpecialAccountDecisionNotice',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('longTermSpecAccNoticeAspect').setStateData(rec.get('Id'), rec.get('State'));
                },
                beforesave: function (asp, rec) {
                    if (Ext.isEmpty(rec.get('SpecialAccountDecision'))) {
                        rec.set('SpecialAccountDecision', asp.controller.params.decisionId);
                    }

                    return true;
                }
            },
            saveRecordHasUpload: function (rec) {
                var me = this,
                    panel = me.getPanel();

                me.mask('Сохранение', panel);
                panel.submit({
                    url: rec.getProxy().getUrl({ action: rec.phantom ? 'create' : 'update' }),
                    params: {
                        records: Ext.encode([rec.getData()])
                    },
                    success: function () {
                        me.unmask();
                        var model = me.getModel();

                        model.load(me.controller.params.decisionId, {
                            success: function (newRec) {
                                me.onPreSaveSuccess(me, newRec);
                                me.setPanelData(newRec);
                            }
                        });
                    },
                    failure: function (form, action) {
                        me.unmask();
                        me.fireEvent('savefailure', rec, action.result.message);
                        Ext.Msg.alert('Ошибка сохранения!', action.result.message);
                    }
                });
            }
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'propertyOwnerDecisionWorksGridAspect',
            gridSelector: '#propertyOwnerDecisionWorkGrid',
            storeName: 'PropertyOwnerDecisionWork',
            modelName: 'PropertyOwnerDecisionWork',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#propertyOwnerDecisionWorksMultiSelectWindow',
            storeSelect: 'dict.WorkSelect',
            storeSelected: 'dict.WorkSelected',
            titleSelectWindow: 'Выбор работ',
            titleGridSelect: 'Работы',
            titleGridSelected: 'Выбранные работы',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 }
            ],

            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];
                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddWorks', 'PropertyOwnerDecisionWork'),
                            method: 'POST',
                            params: {
                                objectIds: recordIds,
                                propertyOwnerDecisionId: asp.controller.propertyOwnerDecisionId
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать работы');
                        return false;
                    }
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Create', applyTo: 'b4addbutton', selector: 'roprotocolgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.enable();
                        else component.disable();
                    }
                },
                { name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Edit', applyTo: 'b4savebutton', selector: '#propertyownerprotocolsEditWindow' },
                {
                    name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Delete', applyTo: 'b4deletecolumn', selector: 'roprotocolgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'roprotocolgrid': {
                render: function (grid) {
                    grid.getStore().on('beforeload', me.onBeforeLoadDecision, me);
                },
                select: {
                    fn: me.onSelectProtocol,
                    scope: me
                },
                deselect: {
                    fn: function () {
                        var decisionGrid = me.getDecisionGrid();
                        decisionGrid.setTitle('Решения протокола');
                        decisionGrid.getStore().removeAll();
                        decisionGrid.setDisabled(true);
                    },
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            view, model,
            realityObjectId = me.params.realityObjectId = id;

        view = me.getMainView() || Ext.widget('roprotpanel');

        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');

        if (me.params.decisId) {
            model = me.getModel('SpecialAccountDecisionNotice');
            me.getAspect('decisionaspect').editRecord(new model({ Id: me.params.decisId }));
        }

        me.getGrid().getStore().load();
        me.getDecisionGrid().getStore().removeAll();
    },

    onBeforeLoadDecision: function (store, operation) {
        operation.params.roId = this.params.realityObjectId;
    },

    onSelectProtocol: function (grid, record) {
        var me = this,
            decisionGrid = me.getDecisionGrid(),
            decisionStore = decisionGrid.getStore(),
            docNumber = record.get('DocumentNumber'),
            docDate = record.get('DocumentDate'),
            typeProtocol = record.get('TypeProtocol'),
            title = 'Решения протокола';

        if (!Ext.isEmpty(docNumber)) {
            title += ' №' + docNumber;
        }

        if (!Ext.isEmpty(docDate)) {
            title += ' от ' + Ext.util.Format.dateRenderer('d.m.Y')(docDate);
        }

        decisionGrid.setTitle(title);
        if (typeProtocol != 10) {
            decisionGrid.setDisabled(false);
            decisionStore.clearFilter(true);
            decisionStore.filter('protocolId', record.getId());
        } else {
            decisionGrid.setDisabled(true);
            decisionStore.removeAll();
        }
    }
});