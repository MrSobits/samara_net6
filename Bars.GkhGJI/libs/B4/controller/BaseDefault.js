Ext.define('B4.controller.BaseDefault', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['BaseDefault'],
    stores: ['BaseDefault'],

    views: ['basedefault.Grid'],

    mainView: 'basedefault.Grid',
    mainViewSelector: 'baseDefaultGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'baseDefaultGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
              {
                  name: 'GkhGji.Inspection.BaseDefault.CheckBoxShowCloseInsp', applyTo: '#cbShowCloseInspections', selector: 'baseDefaultGrid',
                  applyBy: function (component, allowed) {
                      if (allowed) {
                          this.controller.params.showCloseInspections = false;
                          this.controller.getStore('BaseDefault').load();
                          component.show();
                      } else {
                          this.controller.params.showCloseInspections = true;
                          this.controller.getStore('BaseDefault').load();
                          component.hide();
                      }
                  }
              },
              { name: 'GkhGji.Inspection.BaseDefault.Field.InspectionNumber_Edit', applyTo: '#tfInspectionNumber', selector: '#baseDefaultEditPanel' },
              {
                  name: 'GkhGji.Inspection.BaseDefault.Field.ReasonErpChecking_Edit', applyTo: 'b4enumcombo[name=ReasonErpChecking]', selector: '#baseDefaultEditPanel',
                  applyBy: function (component, allowed) {
                      component.setVisible(allowed);
                      component.setDisabled(!allowed);
                  }
              }
            ]
        },
        {
            xtype: 'b4_state_contextmenu',
            name: 'baseDefaultStateTransferAspect',
            gridSelector: 'baseDefaultGrid',
            menuSelector: 'baseDefaultStateMenu',
            stateType: 'gji_inspection'
        },
        {
            /*
             * аспект взаимодействия таблицы плановых проверок юр лиц формы добавления и Панели редактирования,
             * открывающейся в боковой вкладке
             */
            xtype: 'gkhgrideditformaspect',
            name: 'baseDefaultGridWindowAspect',
            gridSelector: 'baseDefaultGrid',
            storeName: 'BaseDefault',
            modelName: 'BaseDefault',
            controllerEditName: 'B4.controller.basedefault.Navigation',
            otherActions: function (actions) {
                actions['baseDefaultGrid #cbShowCloseInspections'] = { 'change': { fn: this.onChangeCheckbox, scope: this } };
            },
            onChangeCheckbox: function (field, newValue) {
                this.controller.params.showCloseInspections = newValue;
                this.controller.getStore('BaseDefault').load();
            }
        }
    ],

    init: function () {
        this.getStore('BaseDefault').on('beforeload', this.onBeforeLoad, this);
        this.params = {};
        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('baseDefaultGrid');
        this.bindContext(view);
        this.application.deployView(view);
    },

    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.showCloseInspections = this.params.showCloseInspections;
        }
    }
});