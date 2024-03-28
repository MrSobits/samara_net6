Ext.define('B4.aspects.permission.service.State', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.servicestateperm',

    permissions: [
        // Добавление, удаление, изменение
        { name: 'GkhDi.DisinfoRealObj.Service.Add', applyTo: '#serviceAddButton', selector: '#serviceGrid' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#capitalRepairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#communalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#controlServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#housingServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Edit', applyTo: 'b4savebutton', selector: '#repairServiceEditWindow' },
        {
            name: 'GkhDi.DisinfoRealObj.Service.Delete', applyTo: 'b4deletecolumn', selector: '#serviceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.Service.Copy', applyTo: 'actioncolumn[text="Копировать"]', selector: '#serviceGrid',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
    
        // Коммунальная услуга
        { name: 'GkhDi.DisinfoRealObj.Service.Communal.TypeOfProvisionService', applyTo: '#cbTypeOfProvisionService', selector: '#communalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Communal.Provider', applyTo: '#cntProvider', selector: '#communalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Communal.VolumePurchasedResources', applyTo: '#nfVolumePurchasedResources', selector: '#communalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Communal.PricePurchasedResources', applyTo: '#nfPricePurchasedResources', selector: '#communalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Communal.KindElectricitySupply', applyTo: '#cbKindElectricitySupply', selector: '#communalServiceEditWindow' },
        {
            name: 'GkhDi.DisinfoRealObj.Service.Communal.ConsumptionNormsPanel', applyTo: '#tbContainer', selector: '#communalServiceEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        {
            name: 'GkhDi.DisinfoRealObj.Service.Communal.ConsumptionNormsNpaGrid', applyTo: '#cnNpaGrid', selector: '#communalServiceEditWindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },
        
        // Жилищная услуга
        { name: 'GkhDi.DisinfoRealObj.Service.Housing.TypeOfProvisionService', applyTo: '#cbTypeOfProvisionService', selector: '#housingServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Housing.Provider', applyTo: '#cntProvider', selector: '#housingServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Housing.Periodicity', applyTo: '#sflPeriodicity', selector: '#housingServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Housing.Equipment', applyTo: '#cbEquipment', selector: '#housingServiceEditWindow' },
        
        // Ремонт услуга
        { name: 'GkhDi.DisinfoRealObj.Service.Repair.TypeOfProvisionService', applyTo: '#cbTypeOfProvisionService', selector: '#repairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Repair.Provider', applyTo: '#cntProvider', selector: '#repairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Repair.UnitMeasure', applyTo: '#sflUnitMeasure', selector: '#repairServiceEditWindow' },
        
        // Капремонт услуга
        { name: 'GkhDi.DisinfoRealObj.Service.CapRepair.TypeOfProvisionService', applyTo: '#cbTypeOfProvisionService', selector: '#capitalRepairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.CapRepair.Provider', applyTo: '#cntProvider', selector: '#capitalRepairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.CapRepair.RegionalFund', applyTo: '#cbRegionalFund', selector: '#capitalRepairServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.CapRepair.AddWork', applyTo: 'b4addbutton', selector: 'workcaprepgrid' },
        
        // Управление МКД услуга
        { name: 'GkhDi.DisinfoRealObj.Service.Control.Provider', applyTo: '#cntProvider', selector: '#controlServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Control.UnitMeasure', applyTo: '#sflUnitMeasure', selector: '#controlServiceEditWindow' },
        
        // Дополнительная услуга
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.Periodicity', applyTo: '#sflPeriodicity', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.Provider', applyTo: '#cntProvider', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.OGRN', applyTo: '#tfOgrn', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.DateRegistration', applyTo: '#dfDateRegistartion', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.Document', applyTo: '#tfDocument', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.Number', applyTo: '#tfDocumentNumber', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.DateFrom', applyTo: '#dfDocumentFrom', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.DateStart', applyTo: '#dfDateStart', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.DateEnd', applyTo: '#dfDateEnd', selector: '#additionalServiceEditWindow' },
        { name: 'GkhDi.DisinfoRealObj.Service.Additional.Sum', applyTo: '#tfTotal', selector: '#additionalServiceEditWindow' }
    ]
});