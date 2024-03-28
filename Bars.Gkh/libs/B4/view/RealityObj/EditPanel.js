Ext.define('B4.view.realityobj.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.realityobjEditPanel',
    closable: true,
    minWidth: 900,
    title: 'Общие сведения',
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.FiasSelectAddress',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.store.dict.CapitalGroup',
        'B4.store.dict.TypeOwnership',
        'B4.store.dict.TypeProject',
        'B4.ux.button.Save',
        'B4.ux.button.ChangeValue',
        'B4.enums.TypeRoof',
        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.YesNo',
        'B4.enums.YesNoNotSet',
        'B4.enums.MethodFormFundCr',
        'B4.view.realityobj.RealityObjToolbar',
        'B4.store.dict.RoofingMaterial',
        'B4.store.dict.WallMaterial',
        'B4.form.field.plugin.InputMask',
        'B4.view.Control.GkhTriggerField',
        'B4.store.RealEstateType',
        'B4.form.EnumCombo',
        'B4.ux.grid.column.Enum',

        'B4.view.realityobj.TopContainer',
        'B4.view.realityobj.GeneralInfoContainer',
        'B4.view.realityobj.ArchitectureMonumentContainer',
        'B4.view.realityobj.HeatingStationContainer',
        'B4.view.realityobj.GeneralParameterContainer'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'realityobjtoolbar'
                }
            ],
            items: [
                {
                    xtype: 'realityobjtopcontainer'
                },
                {
                    xtype: 'realityobjgeneralinfocontainer'
                },
                {
                    xtype: 'realityobjarchitecturemonumentcontainer'
                },
                {
                    xtype: 'realityobjheatingstationcontainer'
                },
                {
                    xtype: 'realityobjgeneralparametercontainer'
                }
            ]
        });

        me.callParent(arguments);
    }
});
