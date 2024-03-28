Ext.define('B4.aspects.permission.realityobj.RealityObjectButtonsPermission', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjbuttonsperm',

    applyBy: function (component, allowed) {
        var field = component.up('container').down('field');
        if (allowed && field && !field.hidden) {
            component.show();
        } else {
            component.hide();
        }
    },

    permissions: [

        // Кнопка "Заполнить собираемость платежей"
        { name: 'Gkh.RealityObject.Field.FillPercentDebt', applyTo: 'button[action=FillPercentDebt]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь частной собственности"
        { name: 'Gkh.RealityObject.Field.FillAreaOwned', applyTo: 'button[action=FillAreaOwned]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь муниципальной площади"
        { name: 'Gkh.RealityObject.Field.FillAreaMunicipalOwned', applyTo: 'button[action=FillAreaMunicipalOwned]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь государственной площади"
        { name: 'Gkh.RealityObject.Field.FillAreaGovernmentOwned', applyTo: 'button[action=FillAreaGovernmentOwned]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить общую  площадь жилых и нежилых помещений"
        { name: 'Gkh.RealityObject.Field.FillAreaLivingNotLivingMkd', applyTo: 'button[action=FillAreaLivingNotLivingMkd]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь жилых помещений"
        { name: 'Gkh.RealityObject.Field.FillAreaLiving', applyTo: 'button[action=FillAreaLiving]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь нежилых помещений"
        { name: 'Gkh.RealityObject.Field.FillAreaNotLivingPremises', applyTo: 'button[action=FillAreaNotLivingPremises]', selector: 'realityobjEditPanel' },

        // Кнопка "Заполнить площадь,находящихся в собственности граждан"
        { name: 'Gkh.RealityObject.Field.FillAreaLivingOwned', applyTo: 'button[action=FillAreaLivingOwned]', selector: 'realityobjEditPanel' }
    ]
});