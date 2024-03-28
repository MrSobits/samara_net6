Ext.define('B4.controller.publicservorg.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
           'B4.aspects.GkhEditPanel',
           'B4.aspects.GkhGridMultiSelectWindow'
    ],

    models: [
        'PublicServiceOrg',
        'publicservorg.Municipality'
    ],
    stores: [
        'publicservorg.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],
    views: ['publicservorg.EditPanel',
            'publicservorg.MunicipalityGrid',
            'SelectWindow.MultiSelectWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    refs: [
        { ref: 'mainView', selector: 'publicservorgeditpanel' }
    ],
    
    aspects: [
        {
            xtype: 'gkheditpanel',
            name: 'publicservorgEditPanelAspect',
            editPanelSelector: 'publicservorgeditpanel',
            modelName: 'PublicServiceOrg'
        },
        {
            /* 
            Аспект взаимодействия таблицы МО домов с массовой формой выбора МО
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'publicServOrgMuAspect',
            gridSelector: 'publicservorgmunicipalitygrid',
            storeName: 'publicservorg.Municipality',
            modelName: 'publicservorg.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#publicServOrgMuMultiSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
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
                            url: B4.Url.action('AddMunicipalityes', 'PublicServiceOrgMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                publicServOrgId: asp.controller.getContextValue(asp.controller.getMainView(), 'publicservorgid')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getStore(asp.storeName).load();
                            Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        }
    
    ],
    
    init: function () {
        this.getStore('publicservorg.Municipality').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },
    
    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('publicservorgeditpanel');

        me.bindContext(view);
        me.setContextValue(view, 'publicservorgid', id);
        me.application.deployView(view, 'public_servorg');

        me.getAspect('publicservorgEditPanelAspect').setData(id);
    },

    onBeforeLoad: function (store, operation) {
        operation.params.publicServOrgId = this.getContextValue(this.getMainView(), 'publicservorgid');
    }
});