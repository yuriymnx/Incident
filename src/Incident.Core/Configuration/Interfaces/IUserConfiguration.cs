namespace Incident.Core.Configuration.Interfaces;

public interface IUserConfiguration
{
    string UserName { get; set; }
    Task Save();
}