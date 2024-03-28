Ext.define('B4.controller.passport.Oki', {
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
        'passport.oki.Grid',        
        'passport.oki.Panel',
        'passport.oki.info.CombinedPassportGrid',
        'passport.oki.info.PassportPanel',
        'passport.NotCreatedGrid'
   ],

    refs: [
        { ref: 'mainPanel', selector: 'okipaspgrid' },
        { ref: 'infoPanel', selector: 'okipanel' },
        { ref: 'notCreatedGrid', selector: 'notcreatedgrid' }
    ],
    
    aspects: [
         {
            xtype: 'b4_state_contextmenu',
            name: 'okistatetransfermenu',
            gridSelector: 'infookipassportgrid',
            menuSelector: 'okiprovpaspgridstatemenu',
            stateType: 'okiproviderpassport'
        },
        {
           xtype: 'gkhpermissionaspect',
           permissions: [
               { name: 'Gkh1468.Passport.Oki.Create', applyTo: 'b4addbutton', selector: 'okipaspgrid' }
           ]
        }],

    init: function () {
        this.control({
            'okipaspgrid combobox[index=Year]': {
                change: {
                    fn: this.onComboBoxChange,
                    scope: this
                }
            },

            'okipaspgrid combobox[index=Month]': {
                change: {
                    fn: this.onComboBoxChange,
                    scope: this
                }
            },
            
            'okipaspgrid actioncolumn': {
                click: {
                    fn: this.gotopassport,
                    scope: this
                }
            },
            
            'okipaspgrid numbercolumn[index=NumberNotCreated]': {
                click: {
                    fn: this.gotonotcreatedcontragents,
                    scope: this
                }
            },
            
            'okipaspgrid b4updatebutton': {
                click: {
                    fn: this.updateGrid,
                    scope: this
                }
            },
            
            'infookipassportgrid actioncolumn': {
                click: {
                    fn: this.editpassport,
                    scope: this
                }
            },
            
            'infookipassportgrid b4updatebutton': {
                click: {
                    fn: this.updateGrid2,
                    scope: this
                }
            },
            
            'infookipassportpanel b4updatebutton': {
            click: {
                    fn: this.updateCombinedPassportGrid,
                    scope: this
            }
        }
        });

        this.callParent(arguments);
    },

    updateCombinedPassportGrid: function (btn) {
        this.getInfoPanel().down('combinedokipassportgrid').getStore().load();
    },
    
    updateGrid: function(btn) {
        btn.up('okipaspgrid').getStore().load();
    },
    
    updateGrid2: function (btn) {
        btn.up('infookipassportgrid').getStore().load();
    },

    gotopassport: function (grid, rowIndex, colIndex, param, param2, rec) {
        Ext.History.add('okipassport_' + rec.getId() + '/');
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
        Ext.History.add('okipasspeditor/' + rec.getId() + '/');
    },

    index: function() {
        var year = new Date().getFullYear(),
            month = new Date().getMonth() + 1,
            view = this.getMainPanel() || Ext.widget('okipaspgrid', {
                year: year,
                month: month
            });

        this.bindContext(view);
        this.application.deployView(view);
        
        this.onComboBoxChange();
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

    // Функция для открытия информации по паспорту МО
    openOkiPanel: function (id) {
        try
        {
            var view = this.getInfoPanel() || Ext.widget('okipanel', {
                id: id
            }), store;

            this.bindContext(view);
            this.application.deployView(view);

            store = view.down('infookipassportgrid').getStore();
            store.clearFilter(true);
            store.filter([
                { property: "passport", value: id }
            ]);
        
            var storeOp = view.down('combinedokipassportgrid').getStore();
        
            storeOp.on('beforeload', function (st, options) {
                options.params.passport = id;
                options.params.stateId = view.down('infookipassportpanel').down('b4combobox[name=State]').getValue();
            }, this);
        
            storeOp.load();
        } catch (e) {
            console.log(e);
        }
    }
});