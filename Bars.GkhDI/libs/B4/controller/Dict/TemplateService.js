Ext.define('B4.controller.dict.TemplateService', {
    extend: 'B4.base.Controller',

    requires:
    [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GridEditWindow',
        'B4.enums.TypeGroupServiceDi',
        'B4.aspects.permission.dict.TemplateService'
    ],

    models:
    [
        'dict.TemplateService',
        'dict.templateservice.OptionFields'
    ],
    stores:
    [
        'dict.TemplateService',
        'dict.templateservice.OptionFields'
    ],
    views: [
        'dict.templateservice.Grid',
        'B4.aspects.ButtonDataExport',
        'dict.templateservice.EditWindow'
    ],

    mainView: 'dict.templateservice.Grid',
    mainViewSelector: 'templateServiceGrid',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context:'B4.mixins.Context'
    },
    
    editWindowSelector: '#templateServiceEditWindow',

    refs: [
        {
            ref: 'mainView',
            selector: 'templateServiceGrid'
        }
    ],

    aspects: [
        {
            xtype: 'templateserviceperm'
        },
        {
            /*
            * аспект кнопки экспорта реестра
            */
            xtype: 'b4buttondataexportaspect',
            name: 'templateServiceExportAspect',
            gridSelector: 'templateServiceGrid',
            buttonSelector: 'templateServiceGrid #btnExport',
            controllerName: 'TemplateService',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'templateServiceGridWindowAspect',
            gridSelector: 'templateServiceGrid',
            editFormSelector: '#templateServiceEditWindow',
            storeName: 'dict.TemplateService',
            modelName: 'dict.TemplateService',
            editWindowView: 'dict.templateservice.EditWindow',
            onSaveSuccess: function (asp, record) {
                asp.constructOptionsFields(asp, record);
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    asp.controller.setCurrentId(record);
                    asp.controller.showResourceTypeFields();
                }
            },
            otherActions: function(action) {
                var me = this.controller;
                action['#templateServiceEditWindow combobox[name=TypeGroupServiceDi]'] = { 'change': { fn: me.showResourceTypeFields, scope: me } };
            },
            constructOptionsFields: function (asp, record) {
                asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                B4.Ajax.request(B4.Url.action('ConstructOptionsFields', 'TemplateService', {
                    kindService: record.get('KindServiceDi'),
                    templateServiceId: record.get('Id')
                })).next(function () {
                    asp.controller.setCurrentId(record);
                    asp.controller.unmask();
                }).error(function () {
                    asp.controller.unmask();
                });
            }
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'templateServiceOptionFieldsAspect',
            storeName: 'dict.templateservice.OptionFields',
            modelName: 'dict.templateservice.OptionFields',
            gridSelector: '#templateServiceOptionFieldsGrid',
            saveButtonSelector: '#templateServiceOptionFieldsGrid #optionFieldsGridSaveButton',
            listeners: {
                beforesave: function (asp, store) {
                    store.each(function (rec) {
                        if (!rec.get('Id')) {
                            rec.set('TemplateService', asp.controller.templateServiceId);
                        }
                    });
                    return true;
                }
            }
        }
    ],

    init: function () {
        var storeTemplateServiceOptionFields = this.getStore('dict.templateservice.OptionFields');
        storeTemplateServiceOptionFields.on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('templateServiceGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.TemplateService').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.templateServiceId) {
            operation.params.templateServiceId = this.templateServiceId;
        }
    },

    setCurrentId: function (rec) {
        var me = this;

        me.templateServiceId = rec.get('Id');

        var editWindow = Ext.ComponentQuery.query(this.editWindowSelector)[0];
        var grid = editWindow.down('#templateServiceOptionFieldsGrid');

        var storeTemplateServiceOptionFields = this.getStore('dict.templateservice.OptionFields');
        storeTemplateServiceOptionFields.removeAll();

        if (this.templateServiceId > 0) {
            storeTemplateServiceOptionFields.load();
            grid.setDisabled(false);
            //Запрет на изменение вида услуги, так как при вызове подрузки данных для id первоначально сохраненого с одним видом услуги не найдетс записи в таблице другого вида услуги.
            editWindow.down('#cbKindServiceDi').setDisabled(true);
        } else {
            grid.setDisabled(true);
            editWindow.down('#cbKindServiceDi').setDisabled(false);
        }
    },

    showResourceTypeFields: function () {
        var me = this,
            win = Ext.ComponentQuery.query(me.editWindowSelector)[0],
            serviceType = win.down('combobox[name=TypeGroupServiceDi]').getValue(),
            communalResourceType = win.down('b4combobox[name=CommunalResourceType]'),
            housingResourceType = win.down('b4combobox[name=HousingResourceType]');

        if (communalResourceType) {
            communalResourceType.setVisible(serviceType === B4.enums.TypeGroupServiceDi.Communal);
        }

        if (housingResourceType) {
            housingResourceType.setVisible(serviceType === B4.enums.TypeGroupServiceDi.Housing);
        }
    }
});