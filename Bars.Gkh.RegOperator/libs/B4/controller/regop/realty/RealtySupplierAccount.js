Ext.define('B4.controller.regop.realty.RealtySupplierAccount', {
    extend: 'B4.controller.MenuItemController',

    requires: [
        'B4.model.RealityObjectSupplierAccountOperation',
        'B4.model.regop.realty.RealtyObjectSupplierAccountProxy',
        'B4.aspects.FormPanel',
        'B4.aspects.permission.GkhPermissionAspect'
    ],
    
    mixins: {
        context: 'B4.mixins.Context'
    },

    views: [
        'regop.realty.RealtySupplierAccountPanel'
    ],
    
    models: [
        'RealityObjectSupplierAccountOperation',
        'regop.realty.RealtyObjectSupplierAccountProxy'
    ],

    stores: [
        'regop.realty.RealtySupplierAccountOperation'
    ],

    refs: [
        {
            ref: 'suppAccOpGrid',
            selector: 'realtysuppaccpanel realtysuppaccopgrid'
        },
        {
            ref: 'mainView',
            selector: 'realtysuppaccpanel'
        }
    ],

    parentCtrlCls: 'B4.controller.realityobj.Navi',

    aspects: [
      {
          xtype: 'gkhpermissionaspect',
          applyBy: function (component, allowed) {
              if (allowed) component.show();
              else component.hide();
          },
          permissions: [
              { name: 'Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.AccountNum_View', applyTo: '#tfAccountNum', selector: 'realtysuppaccpanel' },
              { name: 'Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.DateOpen_View', applyTo: '#dfDateOpen', selector: 'realtysuppaccpanel' },
              { name: 'Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.DateClose_View', applyTo: '#dfDateClose', selector: 'realtysuppaccpanel' },
              { name: 'Gkh.RealityObject.Register.Accounts.RealtySupplierAccount.Field.BankAccountNum_View', applyTo: '#tfBankAccountNum', selector: 'realtysuppaccpanel'}
          ]
      },
      {
          xtype: 'formpanel',
          modelName: 'regop.realty.RealtyObjectSupplierAccountProxy',
          formPanelSelector: 'realtysuppaccpanel form',
          objectId: function () {
              var me = this;
              return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
          },
          afterLoadRecord: function (asp, rec) {
              var suppAccOpStore = asp.controller.getSuppAccOpGrid().getStore();

              suppAccOpStore.clearFilter(true);
              suppAccOpStore.filter([{ property: 'accId', value: rec.get('Id') }]);
          },
          name: 'formpanel'
      }
    ],

    init: function () {
        var me = this;
        me.control({
            'realtysuppaccpanel': {
                updateme: me.updatePanel
            }
        });

        me.callParent(arguments);
    },
    
    index: function(id) {
        var me = this,
            view = this.getMainView() || Ext.widget('realtysuppaccpanel');
        
        me.bindContext(view);
        me.setContextValue(view, 'realityObjectId', id);
        me.application.deployView(view, 'reality_object_info');
    },

    updatePanel: function () {
        var me = this,
            asp = me.getAspect('formpanel');

        asp.controller = me;
        asp.loadRecord();
    }
});