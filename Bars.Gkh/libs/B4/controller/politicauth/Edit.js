Ext.define('B4.controller.politicauth.Edit', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GkhEditPanel',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: ['PoliticAuthority'],
    stores: [
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'politicauth.EditPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Gkh.Orgs.PoliticAuth.GoToContragent.View',
                    applyTo: 'buttongroup[action=GoToContragent]',
                    selector: '#politicAuthEditPanel',
                    applyBy: function (component, allowed) {
                        if (component) {
                            component.setVisible(allowed);
                        }
                    }
                }
            ]
        },
        {
            xtype: 'gkheditpanel',
            name: 'politicAuthEditPanelAspect',
            editPanelSelector: '#politicAuthEditPanel',
            modelName: 'PoliticAuthority',
            listeners: {
                aftersetpaneldata: function (asp, record, panel) {
                    var fieldMunicipalities = panel.down('#politicAuthMunicipalitiesTrigerField');

                    if (record.getId() > 0) {
                        asp.controller.mask('Загрузка', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('GetInfo', 'PoliticAuthority', {
                            politicAuthId: record.getId()
                        })).next(function (response) {
                            asp.controller.unmask();
                            //десериализуем полученную строку
                            var obj = Ext.JSON.decode(response.responseText);

                            fieldMunicipalities.updateDisplayedText(obj.municipalityNames);
                            fieldMunicipalities.setValue(obj.municipalityIds);
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        fieldMunicipalities.setValue(null);
                        fieldMunicipalities.updateDisplayedText(null);
                    }
                }
            }
        },
        {
            /*
            аспект взаимодействия триггер-поля муниципальное образование с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем инспекторов через серверный метод /PoliticAuthority/AddMunicipalities
            */
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'politicAuthMunicipalityMultiSelectWindowAspect',
            fieldSelector: '#politicAuthEditPanel #politicAuthMunicipalitiesTrigerField',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#politicAuthMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            textProperty: 'Name',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield'} }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) { recordIds.push(rec.get('Id')); });
                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddMunicipalities', 'PoliticAuthority', {
                        municipalityIds: recordIds,
                        politicAuthId: asp.controller.params.get('Id')
                    })).next(function () {
                        asp.controller.unmask();
                        Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        }
    ],
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    mainView: 'politicauth.EditPanel',
    mainViewSelector: '#politicAuthEditPanel',

    onLaunch: function () {
        if (this.params) {
            this.getAspect('politicAuthEditPanelAspect').setData(this.params.get('Id'));
        }
    }
});