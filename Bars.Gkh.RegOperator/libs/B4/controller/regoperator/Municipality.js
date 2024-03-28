Ext.define('B4.controller.regoperator.Municipality', {
    extend: 'B4.base.Controller',
    params: null,
    requires: ['B4.aspects.GkhGridMultiSelectWindow'],

    models: ['regoperator.Municipality'],
    stores: ['regoperator.Municipality'],
    views: ['regoperator.MunicipalityGrid', 'SelectWindow.MultiSelectWindow'],

    mainView: 'regoperator.MunicipalityGrid',
    mainViewSelector: '#regoperatorMunicipalityGrid',
    
    mixins: {
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhRegOp.FormationRegionalFund.RegOperator.Municipality.Create', applyTo: 'b4addbutton', selector: '#regoperatorMunicipalityGrid' },
                { name: 'GkhRegOp.FormationRegionalFund.RegOperator.Municipality.Delete', applyTo: 'b4deletecolumn', selector: '#regoperatorMunicipalityGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'aspect',
            modelName: 'regoperator.Municipality',
            storeName: 'regoperator.Municipality',
            gridSelector: '#regoperatorMunicipalityGrid',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#regoperatorMunicipalitySelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
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

                    asp.controller.mask('Сохранение', asp.controller.getMainComponent());

                    Ext.Array.each(records.items,
                        function (item) {
                            recordIds.push(item.get('Id'));
                        }, this);
                    
                    B4.Ajax.request({
                        url: B4.Url.action('AddMunicipalities', 'RegOperatorMunicipality'),
                        method: 'POST',
                        params: {
                            objectIds: Ext.encode(recordIds),
                            regOperatorId: asp.controller.params.get('Id')
                        }
                    }).next(function (response) {
                        asp.controller.unmask();
                        asp.controller.getStore('regoperator.Municipality').load();
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

    init: function () {
        this.getStore('regoperator.Municipality').on('beforeload', this.onBeforeLoad, this);
        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('regoperator.Municipality').load();
    },

    onBeforeLoad: function (store, operation) {
        operation.params.regOperatorId = this.params.get('Id');
    }
});