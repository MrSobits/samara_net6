Ext.define('B4.controller.passport.House', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.StateContextMenu'
    ],
    mixins: {
        context: 'B4.mixins.Context',
        maks: 'B4.mixins.MaskBody'
    },

    views: [
        'passport.house.Grid',
        'passport.house.Panel',
        'passport.house.info.CombinedPassportGrid',
        'passport.house.info.PassportPanel',
        'passport.NotCreatedGrid'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'housepaspgrid' },
        { ref: 'infoPanel', selector: 'housepanel' },
        { ref: 'notCreatedGrid', selector: 'notcreatedgrid' }
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'housestatetransfermenu',
            gridSelector: 'infohousepassportgrid',
            menuSelector: 'houseprovpaspgridstatemenu',
            stateType: 'houseproviderpassport'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh1468.Passport.House.Create', applyTo: 'b4addbutton', selector: 'housepaspgrid' }
            ]
        }],

    init: function () {
        var me = this;
        
        me.control({
            'housepaspgrid combobox[index=Year]': {
                change: {
                    fn: me.onComboBoxChange,
                    scope: me
                }
            },

            'housepaspgrid combobox[index=Month]': {
                change: {
                    fn: me.onComboBoxChange,
                    scope: me
                }
            },

            'housepaspgrid actioncolumn': {
                click: {
                    fn: me.gotopassport,
                    scope: me
                }
            },

            'housepaspgrid numbercolumn[index=NumberNotCreated]': {
                click: {
                    fn: me.gotonotcreatedcontragents,
                    scope: me
                }
            },

            'housepaspgrid b4updatebutton': {
                click: {
                    fn: me.updateGrid,
                    scope: me
                }
            },

            'infohousepassportgrid actioncolumn': {
                click: {
                    fn: me.editpassport,
                    scope: me
                }
            },

            'infohousepassportgrid b4updatebutton': {
                click: {
                    fn: me.updateGrid2,
                    scope: me
                }
            },

            'infohousepassportpanel b4updatebutton': {
                click: {
                    fn: me.updateCombinedPassportGrid,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },

    updateCombinedPassportGrid: function(btn) {
        this.getInfoPanel().down('combinedpassportgrid').getStore().load();
    },

    updateGrid: function(btn) {
        btn.up('housepaspgrid').getStore().load();
    },

    updateGrid2: function(btn) {
        btn.up('infohousepassportgrid').getStore().load();
    },

    gotopassport: function(grid, rowIndex, colIndex, param, param2, rec) {
        Ext.History.add('housepassports_' + rec.getId() + '/');
    },

    gotonotcreatedcontragents: function (grid, rowIndex, colIndex, param, param2, rec) {
        this.openNotCreatedContrGrid(rec);
    },
    
    openNotCreatedContrGrid: function (rec) {
        try {
            var view = this.getNotCreatedGrid() || Ext.widget('notcreatedgrid');
            
            //this.bindContext(view);
            this.application.deployView(view);

            var store = view.getStore();
            var ids = rec.get('ContragentsNotCreated');
            if (ids == "") {
                ids = "0";
            }
            store.on('beforeload', function (st, options) {
                options.params.Id = ids;
                options.params.showAll = true;
            }, this);

            store.load();

        } catch (e) {
            console.log(e);
        }
    },

    editpassport: function (grid, rowIndex, colIndex, param, param2, rec) {
        Ext.History.add('housepasspeditor/' + rec.getId() + '/');
    },

    index: function() {
        var me = this,
            year = new Date().getFullYear(),
            month = new Date().getMonth() + 1,
            view = me.getMainPanel() || Ext.widget('housepaspgrid', {
                year: year,
                month: month
            });

        me.bindContext(view);
        me.application.deployView(view);
     
        me.onComboBoxChange();
    },
    
    onComboBoxChange: function() {
        var panel = this.getMainPanel(),
            cbYear = panel.down('combobox[index=Year]'),
            cbMonth = panel.down('combobox[index=Month]'),
            store = panel.getStore();

        store.clearFilter(true);
        store.filter([
            { property: "year", value: cbYear.getValue() },
            { property: "month", value: cbMonth.getValue() }
        ]);
    },

    // Функция для открытия информации по паспорту ЖД
    openHousePanel: function (id) {
        try
        {
            var view = this.getInfoPanel() || Ext.widget('housepanel', {
                id: id
            }), store;

            this.bindContext(view);
            this.application.deployView(view);

            store = view.down('infohousepassportgrid').getStore();
            store.clearFilter(true);
            store.filter([
                { property: "passport", value: id }
            ]);

            var storeHp = view.down('combinedpassportgrid').getStore();

            storeHp.on('beforeload', function(st, options) {
                options.params.passport = id;
                options.params.stateId = view.down('infohousepassportpanel').down('b4combobox[name=State]').getValue();
            }, this);
            
            storeHp.load();

        } catch (e) {
            console.log(e);
        }
    }
});