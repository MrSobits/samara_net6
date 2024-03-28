/**
 * Контроллер раздела страховых организаций
 */
Ext.define('B4.controller.BelayOrg', {
    
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.StateGridWindowColumn',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['BelayOrganization'],
    
    stores: ['BelayOrganization'],
    
    views: [
        'belayorg.Grid',
        'belayorg.AddWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },
    
    mainView: 'belayorg.Grid',
    mainViewSelector: 'belayOrgGrid',
    
    refs: [
        {
            ref: 'mainView',
            selector: 'belayOrgGrid'
        }
    ],

    aspects: [
        {
            /**
             *аспект кнопки экспорта реестра
             */
            xtype: 'b4buttondataexportaspect',
            name: 'belayOrganizationButtonExportAspect',
            gridSelector: 'belayOrgGrid',
            buttonSelector: 'belayOrgGrid #btnExport',
            controllerName: 'BelayOrganization',
            actionName: 'Export'
        },
        {
            /**
             *аспект прав доступа
             */
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.Belay.Create', applyTo: 'b4addbutton', selector: 'belayOrgGrid' },
                { name: 'Gkh.Orgs.Belay.Edit', applyTo: 'b4savebutton', selector: '#belayOrgEditPanel' },
                { name: 'Gkh.Orgs.Belay.Delete', applyTo: 'b4deletecolumn', selector: 'belayOrgGrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            /**
             *аспект взаимодействия таблицы страховых орг и формы добавления
             */
            xtype: 'gkhgrideditformaspect',
            name: 'belayOrgGridWindowAspect',
            gridSelector: 'belayOrgGrid',
            editFormSelector: '#belayOrgAddWindow',
            storeName: 'BelayOrganization',
            modelName: 'BelayOrganization',
            editWindowView: 'belayorg.AddWindow',
            controllerEditName: 'B4.controller.belayorg.Navigation',
            deleteWithRelatedEntities: true,
            otherActions: function (actions) {
                actions[this.gridSelector + ' #cbShowNotValid'] = { 'change': { fn: this.cbShowNotValidChange, scope: this } };
            },

            cbShowNotValidChange: function (cb, newValue) {
                this.controller.showNotValid = newValue;
                this.controller.getStore(this.storeName).load();
            }
        }
    ],
    
    init: function () {
        var store = this.getStore('BelayOrganization');
        store.on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('belayOrgGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('BelayOrganization').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.showNotValid = this.showNotValid;
    }
});