Ext.define('B4.controller.dict.CompetentOrgGji', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.GkhButtonMultiSelectWindow',
        'B4.aspects.permission.GkhInlineGridPermissionAspect',
        'B4.form.ComboBox'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['dict.CompetentOrgGji'],
    stores: [
        'dict.CompetentOrgGji',
        'dict.RevenueSourceGjiForSelect',
        'dict.RevenueSourceGjiForSelected',
        'contragent.ContragentForSelect',
        'contragent.ContragentForSelected'
    ],

    views: ['dict.competentorggji.Grid'],

    mainView: 'dict.competentorggji.Grid',
    mainViewSelector: 'competentOrgGjiGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'competentOrgGjiGrid'
        }
    ],

    aspects: [
        {
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'competentOrgGjiGrid',
            permissionPrefix: 'GkhGji.Dict.CompetentOrg'
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'competentOrgGjiGridAspect',
            storeName: 'dict.CompetentOrgGji',
            modelName: 'dict.CompetentOrgGji',
            gridSelector: 'competentOrgGjiGrid'
        },
        {   /* 
               Аспект взаимодействия кнопки 'Импорт источников поступлений'
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'componentOrgImportRevenueAspect',
            buttonSelector: 'competentOrgGjiGrid [name=importRevenueSource]',
            multiSelectWindowSelector: '#competentOrgGjiImportRevenueMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'dict.RevenueSourceGjiForSelect',
            storeSelected: 'dict.RevenueSourceGjiForSelected',
            storeName: 'dict.CompetentOrgGji',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор источника',
            titleGridSelect: 'Источники для отбора',
            titleGridSelected: 'Выбранные источники',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddRevenueSource', 'CompetentOrgGji', {
                        objectIds: Ext.encode(recordIds)
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Импорт завершен успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        },
        {   /* 
               Аспект взаимодействия кнопки 'Импорт источников поступлений'
             */
            xtype: 'gkhbuttonmultiselectwindowaspect',
            name: 'componentOrgImportContragentAspect',
            buttonSelector: 'competentOrgGjiGrid [name=importContragent]',
            multiSelectWindowSelector: '#competentOrgGjiImportContragentMultiSelectWindow',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            storeSelect: 'contragent.ContragentForSelect',
            storeSelected: 'contragent.ContragentForSelected',
            storeName: 'dict.CompetentOrgGji',
            columnsGridSelect: [
                {
                    header: 'Муниципальное образование', xtype: 'gridcolumn', dataIndex: 'Municipality', flex: 1, filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Краткое наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Краткое наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, sortable: false },
                { header: 'ИНН', xtype: 'gridcolumn', dataIndex: 'Inn', flex: 1, sortable: false }
            ],
            titleSelectWindow: 'Выбор контрагентов',
            titleGridSelect: 'Контрагенты для отбора',
            titleGridSelected: 'Выбранные контрагенты',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    Ext.each(records.items, function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                    B4.Ajax.request(B4.Url.action('AddContragents', 'CompetentOrgGji', {
                        objectIds: Ext.encode(recordIds)
                    })).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore(asp.storeName).load();
                        Ext.Msg.alert('Сохранение!', 'Импорт завершен успешно');
                        return true;
                    }).error(function () {
                        asp.controller.unmask();
                    });

                    return true;
                }
            }
        }
    ],

    index: function () {
        var view = this.getMainView() || Ext.widget('competentOrgGjiGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.CompetentOrgGji').load();
    }
});