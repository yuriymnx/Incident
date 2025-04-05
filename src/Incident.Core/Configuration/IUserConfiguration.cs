namespace Incident.Core.Configuration;

public interface IUserConfiguration
{
    string UserName { get; set; }
    Task Save();
}