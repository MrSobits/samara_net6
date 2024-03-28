namespace Bars.GkhDi.Import.Sections
{
    using Castle.Windsor;

    public interface ISectionImport
    {
        string Name { get; }

        IWindsorContainer Container { get; set; }

        void ImportSection(ImportParams importParams);
    }
}
