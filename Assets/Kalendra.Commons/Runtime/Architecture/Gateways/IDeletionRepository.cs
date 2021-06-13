namespace Kalendra.Commons.Runtime.Architecture.Gateways
{
    public interface IDeletionRepository
    {
        bool Delete(string hashID);
    }
}