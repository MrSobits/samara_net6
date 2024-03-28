Ext.define('B4.controller.contragentclw.Municipality', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'contragentclw.Municipality'
    ],

    stores: [
        'contragentclw.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'contragentclw.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
      {
            ref: 'mainView',
            selector: 'contragentclwmunicipalitygrid'
      }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.ContragentClw.Municipality.Create', applyTo: 'b4addbutton', selector: 'contragentclwmunicipalitygrid' },
                {
                    name: 'Gkh.Orgs.ContragentClw.Municipality.Delete', applyTo: 'b4deletecolumn', selector: 'contragentclwmunicipalitygrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'contragentClwMunicipalityAspect',
            gridSelector: 'contragentclwmunicipalitygrid',
            storeName: 'contragentclw.Municipality',
            modelName: 'contragentclw.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#contragentClwMunicipalityMultiSelectWindow',
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
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipalities', 'ContragentClw'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                contragentClwId: asp.controller.getContextValue(asp.controller.getMainView(), 'contragentClwId')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getMainView().getStore().load();
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


    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('contragentclwmunicipalitygrid');
        me.bindContext(view);
        me.setContextValue(view, 'contragentClwId', id);
        me.application.deployView(view, 'contragent_clw');
        
        view.getStore().filter('contragentClwId', id);
    }
});