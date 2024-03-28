Ext.define('B4.controller.housinginspection.Municipality',
    {
        extend: 'B4.base.Controller',
        requires: ['B4.aspects.GkhGridMultiSelectWindow'],

        models: ['housinginspection.Municipality'],
        stores: ['housinginspection.Municipality'],
        views: ['housinginspection.MunicipalityGrid', 'SelectWindow.MultiSelectWindow'],

        mainView: 'housinginspection.MunicipalityGrid',
        mainViewSelector: 'housinginspectionmunicipalitygrid',

        mixins: {
            context: 'B4.mixins.Context',
            mask: 'B4.mixins.MaskBody'
        },

        aspects: [
            {
                xtype: 'gkhgridmultiselectwindowaspect',
                name: 'housinginspectionaspect',
                modelName: 'regoperator.Municipality',
                gridSelector: 'housinginspectionmunicipalitygrid',
                multiSelectWindow: 'SelectWindow.MultiSelectWindow',
                multiSelectWindowSelector: '#housinginspectionMunicipalitySelectWindow',
                storeSelect: 'dict.MunicipalityForSelect',
                storeSelected: 'dict.MunicipalityForSelected',
                columnsGridSelect: [
                    {
                        header: 'Наименование',
                        xtype: 'gridcolumn',
                        dataIndex: 'Name',
                        flex: 1,
                        filter: { xtype: 'textfield' }
                    }
                ],
                columnsGridSelected: [
                    { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
                ],
                titleSelectWindow: 'Выбор муниципальных образований',
                titleGridSelect: 'Муниципальные образования для отбора',
                titleGridSelected: 'Выбранные муниципальные образования',
                listeners: {
                    getdata: function(asp, records) {
                        var recordIds = [];

                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                        Ext.Array.each(records.items,
                            function(item) {
                                recordIds.push(item.get('Id'));
                            },
                            this);

                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipalities', 'HousingInspectionMunicipality'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                housingInspectionId: asp.controller.getContextValue(asp.controller.getMainView(), 'housinginspectionId')
                            }
                        }).next(function(response) {
                            asp.controller.unmask();
                            asp.controller.getMainView().getStore().load();
                            Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                            return true;
                        }).error(function() {
                            asp.controller.unmask();
                        });

                        return true;
                    }
                }
            }
        ],

        init: function() {
            this.callParent(arguments);
        },

        index: function(id) {
            var me = this,
                view = me.getMainView() || Ext.widget('housinginspectionmunicipalitygrid');

            me.bindContext(view);
            me.setContextValue(view, 'housinginspectionId', id);
            me.application.deployView(view, 'housinginspection_info');

            var store = view.getStore();

            store.on('beforeload', me.onBeforeLoad, me);
            store.load();
        },

        onBeforeLoad: function(store, operation) {
            var params = { housingInspectionId: this.getContextValue(this.getMainView(), 'housinginspectionId') };
            Ext.apply(operation.params, params);
        }
    });