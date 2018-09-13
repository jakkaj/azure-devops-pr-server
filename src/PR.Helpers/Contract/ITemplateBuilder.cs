namespace PR.Helpers.Contract
{
    public interface ITemplateBuilder
    {
        string Build(string templateName, string arm);
    }
}