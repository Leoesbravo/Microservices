namespace LEscogidoShopWebProject.Services.Interfaces
{
    public interface ItokenProvider
    {
        void SetToken(string token);
        string? GetToken();
        void ClearToken();
    }
}
