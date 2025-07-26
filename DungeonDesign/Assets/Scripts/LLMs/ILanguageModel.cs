using System.Threading.Tasks;

public interface ILanguageModel
{
    Task<string> GetResponseAsync(string prompt);
}
